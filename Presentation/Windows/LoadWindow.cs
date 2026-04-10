using BackupConfigurator.Logic.Services;
using ParekTUI;

namespace BackupConfigurator.Presentation.Windows
{
    public class LoadWindow : BaseWindow
    {
        ConfigurationService _service;

        private SplitContainer _mainSplitContainer;

        private SplitContainer _inputSplitContainer;
        private HContainer _controlsHContainer;

        private Label _inputLabel;
        private InputBox _entryInputBox;
        private Button _acceptButton;
        private Button _cancelButton;

        public LoadWindow(Application application, IWindow returnWindow, ConfigurationService service)
            : base("Load Window", application, returnWindow)
        {
            _service = service;

            _mainSplitContainer = new SplitContainer(0, 0,
                                                     _canvas.GetSize().Width, 6,
                                                     SplitOrientation.Vertical, SplitDirection.FromEnd, 3);
            _mainSplitContainer.SetBorder(false);

            // Input boxes
            _inputSplitContainer = new SplitContainer(0, 0, 0, 0,
                                                      SplitOrientation.Horizontal, SplitDirection.FromStart, 6);
            _inputLabel = new Label(0, 0, "PATH:");
            _entryInputBox = new InputBox(0, 0, 0, 0);
            _inputSplitContainer.AddWidget(_inputLabel);
            _inputSplitContainer.AddWidget(_entryInputBox);

            // Controls
            _controlsHContainer = new HContainer(0, 0, 0, 0);
            _acceptButton = new Button(0, 0, "ACCEPT", OnAcceptButtonClicked);
            _cancelButton = new Button(0, 0, "CANCEL", () => {
                Close();
            });
            _controlsHContainer.AddWidget(_acceptButton);
            _controlsHContainer.AddWidget(_cancelButton);

            // Wiring
            _mainSplitContainer.AddWidget(_inputSplitContainer);
            _mainSplitContainer.AddWidget(_controlsHContainer);

            _canvas.AddWidget(_mainSplitContainer);

            // Selectable components
            RegisterComponent(_entryInputBox);
            RegisterComponent(_acceptButton);
            RegisterComponent(_cancelButton);
        }

        private void OnAcceptButtonClicked()
        {
            _service.SetFilePath(_entryInputBox.GetValue());
            Submit();
        }
    }
}
