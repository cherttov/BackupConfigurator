using ParekTUI;
using System.Diagnostics;
using System.Numerics;

namespace BackupConfigurator.Presentation.Windows
{
    public abstract class BaseWindow : IWindow, IDisposable
    {
        public event Action? Closed;
        public event Action? Submitted;

        protected string _title;
        protected Application _application;
        protected IWindow? _returnWindow;

        protected Canvas _canvas;

        private List<Widget> _components;
        private int _selectedIndex;

        public BaseWindow(string title, Application application, IWindow? returnWindow = null)
        {
            _title = title;
            _application = application;
            _returnWindow = returnWindow;
            _components = new List<Widget>();
            _selectedIndex = 0;

            _canvas = new Canvas(Console.WindowWidth, Console.WindowHeight - 1);  // '- 1' bcs scroll wheel appears
        }

        public void Show() => _application.SwitchWindow(this);

        public void Render()
        {
            _canvas.Refresh();
        }

        public void HandleKey(ConsoleKeyInfo keyInfo)
        {
            if (keyInfo.Key == ConsoleKey.Escape)
                Close();
            else if (keyInfo.Key == ConsoleKey.Tab && _components.Count > 0)
            {
                _components[_selectedIndex].SetFocus(false);
                _selectedIndex = (_selectedIndex + 1) % _components.Count;
                _components[_selectedIndex].SetFocus(true);
            }
            else if (_components.Count > 0)
            {
                int cppKeyCode = 0;

                switch (keyInfo.Key)
                {
                    case ConsoleKey.Enter:
                        cppKeyCode = 13;
                        break;
                    case ConsoleKey.Backspace:
                        cppKeyCode = 8;
                        break;
                    case ConsoleKey.UpArrow:
                        cppKeyCode = 72;
                        break;
                    case ConsoleKey.DownArrow:
                        cppKeyCode = 80;
                        break;
                    default:
                        cppKeyCode = (int)keyInfo.KeyChar;
                        break;
                }

                _components[_selectedIndex].HandleInput(cppKeyCode);
            }
        }

        protected void RegisterComponent(Widget component)
        {
            _components.Add(component);

            if (_components.Count == 1)
            {
                _selectedIndex = 0;
                component.SetFocus(true);
            }
        }

        protected void Close()
        {
            Closed?.Invoke();
            _returnWindow?.Show();
            this.Dispose();
        }

        
        protected void Submit()
        {
            Submitted?.Invoke();
            Close();
        }

        
        public void Dispose()
        {
            _canvas?.Dispose();
        }
    }
}
