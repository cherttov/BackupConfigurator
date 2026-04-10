using System.Security.Cryptography.X509Certificates;
using System.Text.Json.Serialization;

namespace BackupConfigurator.Entities
{
    public enum BackupMethod { Full = 0, Differential = 1, Incremental = 2 }

    public class Configuration : IEntity
    {
        [JsonIgnore]
        public int Id { get; set; }
        public List<string> Sources { get; set; }
        public List<string> Targets { get; set; }
        public BackupMethod Method { get; set; }
        public string Timing { get; set; }
        public BackupRetention Retention { get; set; }

        public Configuration()
        {
            Id = 0;
            Sources = new List<string>();
            Targets = new List<string>();
            Method = BackupMethod.Full;
            Timing = string.Empty;
            Retention = new BackupRetention { Count = 0, Size = 0 };
        }

        public override string ToString()
        {
            return $"Configuration {Id}";
        }

        public void CopyTo(IEntity entity)
        {
            Configuration otherEntity = (Configuration)entity;
            otherEntity.Sources = Sources;
            otherEntity.Targets = Targets;
            otherEntity.Method = Method;
            otherEntity.Timing = Timing;
            otherEntity.Retention = Retention;
        }
    }
}
