using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;
using DataPersistence.Facade;

namespace DataPersistance.Facade
{
    public interface IPersistentRepository<T> where T : IPersistentModel
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>'null' if item with given has not been found</returns>
        T Get(Guid id);

        IEnumerable<T> GetAll();

        IEnumerable<T> GetAll(Func<T, bool> filterPredicate);

        void Add(T newItem);

        void Update(T updatedState);

        void Delete(Guid id);
    }
}
