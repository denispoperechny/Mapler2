using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;
using DataPersistence.Facade;

namespace DataPersistance.Facade
{
    public class PersistentRepository<T> : IPersistentRepository<T> where T : IPersistentModel
    {
        private readonly IDbContext _context;

        public PersistentRepository(IDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns>null if item has not been found</returns>
        public T Get(Guid id)
        {
            return _context.Get<T>(id);
        }

        public IEnumerable<T> GetAll()
        {
            return _context.GetAll<T>();
        }

        public IEnumerable<T> GetAll(Func<T, bool> filterPredicate)
        {
            return _context.GetAll<T>(filterPredicate);
        }

        public void Add(T newItem)
        {
            _context.Add(newItem);
        }

        public void Update(T updatedState)
        {
            _context.Update(updatedState);
        }

        public void Delete(Guid id)
        {
            _context.Delete<T>(id);
        }
    }
}
