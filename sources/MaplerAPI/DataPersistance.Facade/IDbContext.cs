using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;

namespace DataPersistence.Facade
{
    public interface IDbContext
    {
        T Get<T>(Guid id) where T : IPersistentModel;

        IEnumerable<T> GetAll<T>() where T : IPersistentModel;

        IEnumerable<T> GetAll<T>(Func<T, bool> filterPredicate) where T : IPersistentModel;
        
        void Add<T>(T newItem) where T : IPersistentModel;

        void Update<T>(T updatedState) where T : IPersistentModel;

        void Delete<T>(Guid id) where T : IPersistentModel;
    }

}
