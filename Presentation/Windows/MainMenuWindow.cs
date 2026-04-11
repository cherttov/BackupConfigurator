using BackupConfigurator.Entities;
using BackupConfigurator.Logic.Services;
using ParekTUI;
using System.Diagnostics;

namespace BackupConfigurator.Presentation.Windows
{
    public class MainMenuWindow : BaseWindow
    {
        private ConfigurationService _service;

        private SplitContainer _mainSplitContainer;

        private HContainer _viewHContainer;
        private HContainer _controlsHContainer;
        
        private ListBox _configsListBox;
        private int _selectedConfigIndex;
        // private ScrollContainer _configsScrollContainer

        private Button _addButton;
        private Button _editButton;
        private Button _loadButton;
        private Button _exitButton;

        public MainMenuWindow(Application application, ConfigurationService service)
            :base("Main Menu", application)
        {
            _service = service;

            _mainSplitContainer = new SplitContainer(0, 0, 
                                                       _canvas.GetSize().Width, _canvas.GetSize().Height,
                                                       SplitOrientation.Vertical, SplitDirection.FromEnd, 3);
            _mainSplitContainer.SetBorder(false);


            // View
            _viewHContainer = new HContainer(0, 0, 0, 0);
            _viewHContainer.SetBorder(false);

            _configsListBox = new ListBox(0, 0, 0, 0);
            _selectedConfigIndex = -1;
            LoadConfigurations();
            _viewHContainer.AddWidget(_configsListBox);

            // Controls
            _controlsHContainer = new HContainer(0, 0, 0, 0);
            _addButton = new Button(0, 0, "ADD", () => {
                var window = new AddWindow(_application, this, _service);
                window.Submitted += () => { LoadConfigurations(); };
                _application.SwitchWindow(window);
            });
            _editButton = new Button(0, 0, "EDIT", () => {
                var index = _configsListBox.GetSelectedIndex();
                if (index <= 0)
                    return;

                var window = new EditWindow(_application, this, _service, index + 1); // Id starts from 1
                window.Submitted += () => { LoadConfigurations(); };
                _application.SwitchWindow(window);
            });
            _loadButton = new Button(0, 0, "LOAD", () => {
                var window = new LoadWindow(_application, this, _service);
                window.Submitted += () => { LoadConfigurations(); };
                _application.SwitchWindow(window);
            });
            _exitButton = new Button(0, 0, "EXIT", () => {
                _application.Stop();
            });
            _controlsHContainer.AddWidget(_addButton);
            _controlsHContainer.AddWidget(_editButton);
            _controlsHContainer.AddWidget(_loadButton);
            _controlsHContainer.AddWidget(_exitButton);
            
            // Wire up to main layout
            _mainSplitContainer.AddWidget(_viewHContainer);
            _mainSplitContainer.AddWidget(_controlsHContainer);

            _canvas.AddWidget(_mainSplitContainer);

            // Selectable components
            RegisterComponent(_configsListBox);
            RegisterComponent(_addButton);
            RegisterComponent(_editButton);
            RegisterComponent(_loadButton);
            RegisterComponent(_exitButton);
        }

        private void LoadConfigurations()
        {
            _selectedConfigIndex = -1;

            _configsListBox.Clear();

            List<Configuration> configs = _service.GetAll().ToList();

            foreach (Configuration config in configs)
            {
                Button newConfigButton = new Button(0, 0, config.ToString(), () => {
                    _selectedConfigIndex = config.Id;
                });

                _configsListBox.AddWidget(newConfigButton);
            }
        }
    }
}
