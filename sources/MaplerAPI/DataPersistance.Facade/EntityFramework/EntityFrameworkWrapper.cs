using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;
using DataPersistence.Facade;

namespace DataPersistance.Facade.EntityFramework
{
    public class EntityFrameworkWrapper : IDbContext, IUnitOfWork
    {
        public T Get<T>(Guid id) where T : IPersistentModel
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>() where T : IPersistentModel
        {
            throw new NotImplementedException();
        }

        public IEnumerable<T> GetAll<T>(Func<T, bool> filterPredicate) where T : IPersistentModel
        {
            throw new NotImplementedException();
        }

        public void Add<T>(T newItem) where T : IPersistentModel
        {
            throw new NotImplementedException();
        }

        public void Update<T>(T updatedState) where T : IPersistentModel
        {
            throw new NotImplementedException();
        }

        public void Delete<T>(Guid id) where T : IPersistentModel
        {
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
