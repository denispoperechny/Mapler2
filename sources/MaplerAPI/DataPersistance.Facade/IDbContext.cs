using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;

namespace DataPersistence.Facade
{
    // TODO: Go to IQueryable instead of IEnumerable
    public interface IDbContext
    {
        T Get<T>(Guid id) where T : class, IPersistentModel;

        IEnumerable<T> GetAll<T>() where T : class, IPersistentModel;

        IEnumerable<T> GetAll<T>(Func<T, bool> filterPredicate) where T : class, IPersistentModel;

        void Add<T>(T newItem) where T : class, IPersistentModel;

        void Update<T>(T updatedState) where T : class, IPersistentModel;

        void Delete<T>(Guid id) where T : class, IPersistentModel;
    }

}
