namespace BackupConfigurator.Data.Repositories
{
    public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        T? GetById(int id);
        T Create(T createdEntity);
        T Update(int id, T updatedEntity);
        void Delete(int id);
    }
}
