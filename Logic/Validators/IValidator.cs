namespace BackupConfigurator.Logic.Validators
{
    public interface IValidator<T>
    {
        void Validate(T value);
    }
}
