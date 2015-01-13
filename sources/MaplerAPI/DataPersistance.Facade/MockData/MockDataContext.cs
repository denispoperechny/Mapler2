using System;
using System.CodeDom;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataPersistence.Facade.Data;
using DataPersistence.Facade;

namespace DataPersistance.Facade.MockData
{
    public class MockDataContext : IDbContext, IUnitOfWork, IMockDataProvider
    {
        // TODO: try another thread safe mech
        private readonly Dictionary<Type, IList> _dataStorage = new Dictionary<Type, IList>();
        private readonly object _updateLock = new object();

        public MockDataContext()
        {
        }

        public void AddDataCollection<T>(List<T> data) where T : IPersistentModel
        {
            if (data == null) throw new ArgumentNullException("data");
            _dataStorage.Add(typeof(T), data );
        }

        private IList<U> GetDataCollection<U>() where U : IPersistentModel
        {
            var dataType = typeof (U);
            if (!_dataStorage.ContainsKey(dataType))
                throw new InvalidOperationException("There is no mocked data of type " + dataType);

            return (IList<U>)_dataStorage[dataType];
        }

        public T Get<T>(Guid id) where T : IPersistentModel
        {
            lock (_updateLock)
            {
                var item = GetDataCollection<T>().FirstOrDefault(x => x.Id == id);
                //if (item == null)
                //    throw new InvalidOperationException(string.Format("Not found entity of type: {0}. Id: {1}", typeof(T), id));
                
                return item;
            }
        }

        public IEnumerable<T> GetAll<T>() where T : IPersistentModel
        {
            lock (_updateLock)
            {
                return GetDataCollection<T>();
            }
        }

        public IEnumerable<T> GetAll<T>(Func<T, bool> filterPredicate) where T : IPersistentModel
        {
            if (filterPredicate == null) throw new ArgumentNullException("filterPredicate");

            lock (_updateLock)
            {
                return GetDataCollection<T>().Where(filterPredicate);
            }
        }

        public void Add<T>(T newItem) where T : IPersistentModel
        {
            if (newItem == null) throw new ArgumentNullException("newItem");
            if (newItem.Id == Guid.Empty) throw new ArgumentException("newItem.Id should not be Guid.Empty");

            var data = GetDataCollection<T>();

            lock (_updateLock)
            {
                var existingRecord = data.FirstOrDefault(x => x.Id == newItem.Id);
                if (existingRecord != null)
                    throw new InvalidOperationException("There is already existing item with Id: " + newItem.Id);

                data.Add(newItem);
            }
        }

        public void Update<T>(T updatedState) where T : IPersistentModel
        {
            var data = GetDataCollection<T>();

            lock (_updateLock)
            {
                var existingItem = data.FirstOrDefault(x => x.Id == updatedState.Id);
                if (existingItem == null)
                    throw new InvalidOperationException("There is no items to update. Id: " + updatedState.Id);

                data.Remove(existingItem);
                data.Add(updatedState);
            }
        }

        public void Delete<T>(Guid id) where T : IPersistentModel
        {
            var data = GetDataCollection<T>();

            lock (_updateLock)
            {
                var existingItem = data.FirstOrDefault(x => x.Id == id);
                if (existingItem == null) throw new InvalidOperationException("There is no data with Id: " + id);

                data.Remove(existingItem);
            }
        }

        public void Dispose()
        {
            lock (_updateLock)
            {
                _dataStorage.Clear();
            }
        }

        public void Save()
        {
            Thread.Sleep(1500);
        }

    }
}
