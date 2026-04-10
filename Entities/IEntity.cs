namespace BackupConfigurator.Entities
{
    public interface IEntity
    {
        int Id { get; set; }

        void CopyTo(IEntity entity);
    }
}
