using BackupConfigurator.Entities;

namespace BackupConfigurator.Data.Repositories
{
    public class MemoryRepository<T> : IRepository<T>
        where T : class, IEntity
    {
        private List<T> _entities;

        public MemoryRepository()
        {
            _entities = new List<T>();
        }

        public IEnumerable<T> GetAll()
        {
            return _entities;
        }

        public T? GetById(int id)
        {
            return _entities.Single(e => e.Id == id);
        }

        public T Create(T createdEntity)
        {
            createdEntity.Id = _entities.Count > 0 ? _entities.Max(e => e.Id) + 1 : 1;

            _entities.Add(createdEntity);

            return createdEntity;
        }

        public T Update(int id, T updatedEntity)
        {
            T existingEntity = GetById(id) ?? throw new InvalidOperationException("Entity not found.");

            updatedEntity.CopyTo(existingEntity);

            return existingEntity;
        }

        public void Delete(int id)
        {
            T existingEntity = GetById(id) ?? throw new InvalidOperationException("Entity not found.");

            _entities.Remove(existingEntity);
        }
    }
}
