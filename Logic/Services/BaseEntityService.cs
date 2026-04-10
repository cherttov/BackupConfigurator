using BackupConfigurator.Data.Repositories;
using BackupConfigurator.Entities;
using BackupConfigurator.Logic.Validators;

namespace BackupConfigurator.Logic.Services
{
    public class BaseEntityService<T> : IEntityService<T>
        where T : class, IEntity
    {
        protected IRepository<T> _repository;
        protected IValidator<T> _validator;

        protected BaseEntityService(IRepository<T> repository, IValidator<T> validator)
        {
            _repository = repository;
            _validator = validator;
        }

        public IEnumerable<T> GetAll()
        {
            return _repository.GetAll();
        }

        public T? GetById(int id)
        {
            return _repository.GetById(id);
        }

        public T Create(T createdEntity)
        {
            _validator.Validate(createdEntity);
            return _repository.Create(createdEntity);
        }

        public T Update(int id, T updatedEntity)
        {
            _validator.Validate(updatedEntity);
            return _repository.Update(id, updatedEntity);
        }

        public void Delete(int id)
        {
            _repository.Delete(id);
        }
    }
}
