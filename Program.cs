using BackupConfigurator.Data.Repositories;
using BackupConfigurator.Entities;
using BackupConfigurator.Logic.Services;
using BackupConfigurator.Logic.Validators;
using BackupConfigurator.Presentation.Windows;
using System.Diagnostics;

namespace BackupConfigurator
{
    public class Program
    {
        static void Main(string[] args)
        {
            IRepository<Configuration> repository = new MemoryRepository<Configuration>();

            ConfigurationService service = new ConfigurationService(repository);

            Application app = new Application();

            IWindow window = new MainMenuWindow(app, service);

            app.Run(window);
        }
    }
}
