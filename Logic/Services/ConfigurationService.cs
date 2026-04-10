using BackupConfigurator.Data.Repositories;
using BackupConfigurator.Entities;
using BackupConfigurator.Logic.Validators;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace BackupConfigurator.Logic.Services
{
    public class ConfigurationService : BaseEntityService<Configuration>
    {
        private const string DefaultFileName = "config.json";
        private string _filePath;

        public ConfigurationService(IRepository<Configuration> repository, string? initialPath = null)
            : base(repository, new ConfigurationValidator())
        {
            _filePath = initialPath is null 
                ? Path.Combine(AppDomain.CurrentDomain.BaseDirectory, DefaultFileName) 
                : ResolveToFilePath(initialPath);

            // Try to load existing file to repo
            LoadFromFile();
        }

        private static string ResolveToFilePath(string path)
        {
            if (Directory.Exists(path))
                return Path.Combine(path, DefaultFileName);

            if (Path.HasExtension(path))
            {
                var dir = Path.GetDirectoryName(path) ?? AppDomain.CurrentDomain.BaseDirectory;
                if (!Directory.Exists(dir))
                    throw new DirectoryNotFoundException("Directory for provided file path does not exist.");
                return path;
            }

            throw new ArgumentException("Provided path doesn't lead to a directory/file.");
        }

        public void AddConfiguration(Configuration configuration)
        {
            if (configuration is null)
                throw new ArgumentNullException("Added configuration cannot be null.");

            _validator.Validate(configuration);

            _repository.Create(configuration);

            SaveAll();
        }

        public Configuration UpdateConfiguration(int id, Configuration updatedConfiguration)
        {
            if (updatedConfiguration is null)
                throw new ArgumentNullException("Updated configuration cannot be null.");

            _validator.Validate(updatedConfiguration);

            var result = _repository.Update(id, updatedConfiguration);

            SaveAll();

            return result;
        }

        public void SetFilePath(string newPath)
        {
            if (string.IsNullOrWhiteSpace(newPath))
                throw new ArgumentException("Path is empty.");

            string resolved = ResolveToFilePath(newPath);

            _filePath = resolved;

            LoadFromFile();
        }

        public void LoadFromFile()
        {
            if (!File.Exists(_filePath))
                return;

            string json = File.ReadAllText(_filePath);

            if (string.IsNullOrWhiteSpace(json))
                return;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            List<Configuration>? configs = null;
            try
            {
                configs = JsonSerializer.Deserialize<List<Configuration>>(json, options);
            }
            catch (JsonException)
            {
                throw;
            }

            if (configs is null)
                return;

            // Clear current repo
            var existingIds = _repository.GetAll().Select(e => e.Id).ToList();
            foreach (var id in existingIds)
            {
                try { _repository.Delete(id);  } catch { }
            }

            // Add deserialized configs to repo
            foreach (var config in configs)
            {
                _repository.Create(config);
            }
        }

        public void SaveAll()
        {
            var list = _repository.GetAll().ToList();

            var options = new JsonSerializerOptions { WriteIndented = true };
            options.Converters.Add(new JsonStringEnumConverter(JsonNamingPolicy.CamelCase));

            string json = JsonSerializer.Serialize(list, options);

            var dir = Path.GetDirectoryName(_filePath) ?? AppDomain.CurrentDomain.BaseDirectory;
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);

            File.WriteAllText(_filePath, json);
        }
    }
}
