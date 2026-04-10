using BackupConfigurator.Entities;

namespace BackupConfigurator.Logic.Helpers
{
    public class MethodHelper
    {
        public static BackupMethod ParseMethod(string raw)
        {
            if (Enum.TryParse(raw.Trim(), true, out BackupMethod method))
                return method;

            return BackupMethod.Full;
        }

        public static string ParseString(BackupMethod method)
        {
            switch (method)
            {
                case BackupMethod.Differential:
                    return "DIFFERENTIAL";
                case BackupMethod.Incremental:
                    return "INCREMENTAL";
                default:
                    return "FULL";
            }
        }
    }
}
