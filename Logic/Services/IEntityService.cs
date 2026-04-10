namespace BackupConfigurator.Logic.Services
{
    public interface IEntityService<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        T Create(T createdEntity);
        T Update(int id, T updatedEntity);
        void Delete(int id);
    }
}
