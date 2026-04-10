using BackupConfigurator.Entities;
using System.ComponentModel.DataAnnotations;
using System.Security.AccessControl;

namespace BackupConfigurator.Logic.Validators
{
    public class ConfigurationValidator : IValidator<Configuration>
    {
        public void Validate(Configuration entity)
        {
            if (!ValidatePaths(entity.Sources))
                throw new InvalidDataException("Sources are invalid.");

            if (!ValidatePaths(entity.Targets))
                throw new InvalidDataException("Targets are invalid.");

            if (!ValidateTiming(entity.Timing))
                throw new InvalidDataException("Timing is invalid.");

            if (!ValidateRetention(entity.Retention))
                throw new InvalidDataException("Retention is invalid.");
        }

        private bool ValidatePaths(List<string> paths)
        {
            if (paths is null || paths.Count == 0)
                return false;

            var invalidChars = Path.GetInvalidPathChars();
            
            foreach (var path in paths)
            {
                if (string.IsNullOrWhiteSpace(path))
                    return false;

                var trimmed = path.Trim();
                if (trimmed.Length > 512)
                    return false;

                if (trimmed.Any(char.IsControl))
                    return false;
            }

            return true;
        }

        private bool ValidateTiming(string timing)
        {
            if (string.IsNullOrWhiteSpace(timing))
                return false;

            string[] parts = timing.Trim().Split(' ', StringSplitOptions.RemoveEmptyEntries);

            if (parts.Length < 6 || parts.Length > 7)
                return false;

            return true;
        }

        private bool ValidateRetention(BackupRetention retention)
        {
            if (retention is null)
                return false;

            if (retention.Count < 0)
                return false;

            if (retention.Size < 0)
                return false;

            return true;
        }
    }
}
