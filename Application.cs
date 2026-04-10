namespace BackupConfigurator
{
    public class Application
    {
        private bool _isRunning;
        private IWindow? _activeWindow;

        public Application()
        {
            _isRunning = false;
            _activeWindow = null;
        }

        public void Run(IWindow window)
        {
            _isRunning = true;
            _activeWindow = window;

            // Console window setup
            Console.Title = "Backup Configurator";
            Console.CursorVisible = false;
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.SetBufferSize(Console.WindowWidth, Console.WindowHeight);
            Console.Clear();

            while (_isRunning)
            {
                Render();
                HandleKey(Console.ReadKey(true));
            }

            (_activeWindow as IDisposable)?.Dispose();
        }

        public void Stop() => _isRunning = false;

        public void SwitchWindow(IWindow window)
        {
            Console.Clear();
            _activeWindow = window;
        }

        protected void Render() 
        {
            Console.SetCursorPosition(0, 0); // fix in cpp-engine
            _activeWindow?.Render();
        }

        protected void HandleKey(ConsoleKeyInfo keyInfo) => _activeWindow?.HandleKey(keyInfo);
    }
}
