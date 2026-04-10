using BackupConfigurator.Entities;
using BackupConfigurator.Logic.Helpers;
using BackupConfigurator.Logic.Services;
using ParekTUI;
using System.Diagnostics;

namespace BackupConfigurator.Presentation.Windows
{
    public class EditWindow : BaseWindow
    {
        private ConfigurationService _service;
        private Configuration _edited;

        private SplitContainer _mainSplitContainer;

        private VContainer _inputVContainer;
        private HContainer _controlsHContainer;

        // Input boxes
        private SplitContainer _sourceSplitContainer;
        private Label _sourceLabel;
        private InputBox _sourceInputBox;

        private SplitContainer _targetSplitContainer;
        private Label _targetLabel;
        private InputBox _targetInputBox;

        private SplitContainer _methodSplitContainer;
        private Label _methodLabel;
        private InputBox _methodInputBox;

        private SplitContainer _timingSplitContainer;
        private Label _timingLabel;
        private InputBox _timingInputBox;

        private SplitContainer _retentionCountSplitContainer;
        private Label _retentionCountLabel; 
        private InputBox _retentionCountInputBox;

        private SplitContainer _retentionSizeSplitContainer;
        private Label _retentionSizeLabel;
        private InputBox _retentionSizeInputBox;

        // Controls
        private Button _acceptButton;
        private Button _cancelButton;

        public EditWindow(Application application, IWindow returnWindow, ConfigurationService service, int selectedIndex) // Add config 
            : base("Edit Window", application, returnWindow)
        {
            _service = service;

            _edited = _service.GetById(selectedIndex) ?? throw new Exception("Selected configuration is null.");

            _mainSplitContainer = new SplitContainer(0, 0,
                                                     _canvas.GetSize().Width, _canvas.GetSize().Height,
                                                     SplitOrientation.Vertical, SplitDirection.FromEnd, 3);
            _mainSplitContainer.SetBorder(false);

            // Input boxes
            _inputVContainer = new VContainer(0, 0, 0, 0);

            _sourceSplitContainer = new SplitContainer(0, 0, 0, 0,
                                                      SplitOrientation.Horizontal, SplitDirection.FromStart, 8);
            _sourceLabel = new Label(0, 0, "SOURCE:");
            _sourceInputBox = new InputBox(0, 0, 0, 0);
            _sourceInputBox.SetValue(_edited.Sources.FirstOrDefault() ?? "");
            _sourceSplitContainer.AddWidget(_sourceLabel);
            _sourceSplitContainer.AddWidget(_sourceInputBox);

            _targetSplitContainer = new SplitContainer(0, 0, 0, 0,
                                                      SplitOrientation.Horizontal, SplitDirection.FromStart, 8);
            _targetLabel = new Label(0, 0, "TARGET:");
            _targetInputBox = new InputBox(0, 0, 0, 0);
            _targetInputBox.SetValue(_edited.Targets.FirstOrDefault() ?? "");
            _targetSplitContainer.AddWidget(_targetLabel);
            _targetSplitContainer.AddWidget(_targetInputBox);

            _methodSplitContainer = new SplitContainer(0, 0, 0, 0,
                                                      SplitOrientation.Horizontal, SplitDirection.FromStart, 8);
            _methodLabel = new Label(0, 0, "METHOD:");
            _methodInputBox = new InputBox(0, 0, 0, 0);
            _methodInputBox.SetValue(MethodHelper.ParseString(_edited.Method));
            _methodSplitContainer.AddWidget(_methodLabel);
            _methodSplitContainer.AddWidget(_methodInputBox);

            _timingSplitContainer = new SplitContainer(0, 0, 0, 0,
                                                      SplitOrientation.Horizontal, SplitDirection.FromStart, 8);
            _timingLabel = new Label(0, 0, "TIMING:");
            _timingInputBox = new InputBox(0, 0, 0, 0);
            _timingInputBox.SetValue(_edited.Timing);
            _timingSplitContainer.AddWidget(_timingLabel);
            _timingSplitContainer.AddWidget(_timingInputBox);

            _retentionSizeSplitContainer = new SplitContainer(0, 0, 0, 0,
                                                      SplitOrientation.Horizontal, SplitDirection.FromStart, 16);
            _retentionSizeLabel = new Label(0, 0, "RETENTION SIZE:");
            _retentionSizeInputBox = new InputBox(0, 0, 0, 0);
            _retentionSizeInputBox.SetValue(Convert.ToString(_edited.Retention.Size));
            _retentionSizeSplitContainer.AddWidget(_retentionSizeLabel);
            _retentionSizeSplitContainer.AddWidget(_retentionSizeInputBox);

            _retentionCountSplitContainer = new SplitContainer(0, 0, 0, 0,
                                                      SplitOrientation.Horizontal, SplitDirection.FromStart, 17);
            _retentionCountLabel = new Label(0, 0, "RETENTION COUNT:");
            _retentionCountInputBox = new InputBox(0, 0, 0, 0);
            _retentionCountInputBox.SetValue(Convert.ToString(_edited.Retention.Count));
            _retentionCountSplitContainer.AddWidget(_retentionCountLabel);
            _retentionCountSplitContainer.AddWidget(_retentionCountInputBox);

            _inputVContainer.AddWidget(_sourceSplitContainer);
            _inputVContainer.AddWidget(_targetSplitContainer);
            _inputVContainer.AddWidget(_methodSplitContainer);
            _inputVContainer.AddWidget(_timingSplitContainer);
            _inputVContainer.AddWidget(_retentionCountSplitContainer);
            _inputVContainer.AddWidget(_retentionSizeSplitContainer);

            // Controls
            _controlsHContainer = new HContainer(0, 0, 0, 0);
            _acceptButton = new Button(0, 0, "ACCEPT", OnAcceptButtonClicked);
            _cancelButton = new Button(0, 0, "CANCEL", () => {
                Close();
            });
            _controlsHContainer.AddWidget(_acceptButton);
            _controlsHContainer.AddWidget(_cancelButton);

            // Wiring
            _mainSplitContainer.AddWidget(_inputVContainer);
            _mainSplitContainer.AddWidget(_controlsHContainer);

            _canvas.AddWidget(_mainSplitContainer);

            // Selectable components
            RegisterComponent(_sourceInputBox);
            RegisterComponent(_targetInputBox);
            RegisterComponent(_methodInputBox);
            RegisterComponent(_timingInputBox);
            RegisterComponent(_retentionCountInputBox);
            RegisterComponent(_retentionSizeInputBox);

            RegisterComponent(_acceptButton);
            RegisterComponent(_cancelButton);
        }

        private void OnAcceptButtonClicked()
        {
            var config = new Configuration
            {
                Sources = new List<string> { _sourceInputBox.GetValue() },
                Targets = new List<string> { _targetInputBox.GetValue() },
                Method = MethodHelper.ParseMethod(_methodInputBox.GetValue()),
                Timing = _timingInputBox.GetValue(),
                Retention = new BackupRetention
                {
                    Count = int.TryParse(_retentionCountInputBox.GetValue(), out int countVal) ? countVal : 0,
                    Size = int.TryParse(_retentionSizeInputBox.GetValue(), out int sizeVal) ? sizeVal : 0
                }
            };

            try
            {
                _service.UpdateConfiguration(_edited.Id, config);
            }
            catch (InvalidDataException ex)
            {
                throw new InvalidDataException("Validation failed: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to save configuration: " + ex.Message);
            }

            Submit();
        }
    }
}
