namespace BackupConfigurator
{
    public interface IWindow
    {
        event Action? Closed;
        event Action? Submitted;

        void Show();
        void Render();
        void HandleKey(ConsoleKeyInfo keyInfo);
    }
}