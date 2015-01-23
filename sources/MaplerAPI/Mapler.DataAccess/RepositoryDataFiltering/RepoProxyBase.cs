using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataPersistance.Facade;
using DataPersistence.Facade.Data;
using Mapler.DataAccess.RepositoryDataFiltering.Exceptions;
using Mapler.DataPersistance.Models;
using Mapler.Security;
using Microsoft.Practices.Unity;

namespace Mapler.DataAccess.RepositoryDataFiltering
{
    /// <summary>
    /// Filters out records which are not permitted for current user. 
    /// Set IsActive property to false instead of deletion of item.
    /// Prevents updating the fields which are read only
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class RepoProxyBase<T> : IRepoBusinessProxy<T>, IPersistentRepository<T> where T : class, IPersistentModel
    {
        private readonly IPersistentRepository<T> _repo;
        private readonly Guid _initialUserId;

        //private Lazy<List<Guid>> ReadableIds = null;
        //private Lazy<List<Guid>> EditableIds = null;
        //private Lazy<List<Guid>> DeletableIds = null; 

        protected RepoProxyBase(IPersistentRepository<T> repo)
        {
            if (repo == null) throw new ArgumentNullException("repo");
            _repo = repo;
            //_initialUserId = Principal.Identity.UserId;
        }

        protected IPersistentRepository<T> Repository
        {
            get { return _repo; }
        }

        [Dependency]
        protected IPersistentRepository<User> UserRepository { get; set; }

        [Dependency]
        protected IPersistentRepository<Company> CompanyRepository { get; set; }

        protected MaplerPrincipal Principal
        {
            get
            {
                MaplerPrincipal result = Thread.CurrentPrincipal as MaplerPrincipal;
                if (result == null)
                    throw new InvalidOperationException("Current principal is not MaplerPrincipal");

                //if (result.Identity.UserId != _initialUserId)
                //    throw new InvalidOperationException("Multiple users for one Proxy are currently not supported due to internal caches");

                return result;
            }
        }

        public T Get(Guid id)
        {
            if (Principal.IsSuperUser || GetReadableEntityIds(Principal.Identity.UserId, Principal.Companies).Contains(id))
            {
                var result = _repo.GetAll(x => x.Id == id && x.IsActive).FirstOrDefault();
                if (result == null)
                    throw new InvalidOperationException("Cannot find entity Id: " + id);
                FilterFieldsOnGet(result);
                return result;
            } 
            else
                throw new DataAccessRestrictedException(typeof(User), id, Principal.Identity.Name);
        }

        public IEnumerable<T> GetAll()
        {
            List<T> items = null;
            if (Principal.IsSuperUser)
                items = _repo.GetAll(x => x.IsActive).ToList();
            else
                items = _repo.GetAll(x => GetReadableEntityIds(Principal.Identity.UserId, Principal.Companies).Contains(x.Id) && x.IsActive).ToList();

            foreach (var item in items)
                FilterFieldsOnGet(item);
            return items;
        }

        public IEnumerable<T> GetAll(Func<T, bool> filterPredicate)
        {
            if (Principal.IsSuperUser)
                return _repo.GetAll(filterPredicate);

            return _repo.GetAll(x => GetReadableEntityIds(Principal.Identity.UserId, Principal.Companies).Contains(x.Id)).Where(filterPredicate);
        }

        public void Add(T newItem)
        {
            newItem.Id = Guid.Empty == newItem.Id ? Guid.NewGuid() : newItem.Id;
            newItem.IsActive = true;
            CheckFieldsOnCreate(newItem);

            _repo.Add(newItem);
        }

        public void Update(T updatedState)
        {
            // TODO: Add logging

            var existingItem = _repo.Get(updatedState.Id);
            if (existingItem == null)
            {
                Add(updatedState);
                return;
            }

            if (Principal.IsSuperUser || GetEditableEntityIds(Principal.Identity.UserId, Principal.Companies).Contains(updatedState.Id))
            {
                CheckFieldsOnUpdate(updatedState, existingItem);
                _repo.Update(updatedState);
            }
            else
                throw new InvalidOperationException(
                    string.Format("User '{0}' not permitted to update '{1}', Id: {2}",
                    Principal.Identity.Name, typeof(T), updatedState.Id));
        }

        public void Delete(Guid id)
        {
            // TODO: Add logging

            if (Principal.IsSuperUser || GetDeletableEntityIds(Principal.Identity.UserId, Principal.Companies).Contains(id))
            {
                var itemToDeactivate = _repo.Get(id);
                if (itemToDeactivate == null)
                    throw new InvalidOperationException("Entity not foud Id: " + id);
                itemToDeactivate.IsActive = false;
            }
            else
                throw new InvalidOperationException(
                    string.Format("User '{0}' not permitted to delete '{1}', Id: {2}", 
                    Principal.Identity.Name, typeof(T), id));
        }

        protected abstract List<Guid> GetReadableEntityIds(Guid regularUserId, List<Guid> userCompanyIds);
        protected abstract List<Guid> GetEditableEntityIds(Guid regularUserId, List<Guid> userCompanyIds);
        protected abstract List<Guid> GetDeletableEntityIds(Guid regularUserId, List<Guid> userCompanyIds);

        /// <summary>
        /// Callback to update properties manually on editingan Entity
        /// </summary>
        /// <param name="updatedStateItem"></param>
        /// <param name="alreadyExistingItem"></param>
        protected abstract void CheckFieldsOnUpdate(T updatedStateItem, T alreadyExistingItem);

        /// <summary>
        /// Callback to update properties manually on adding new Entity
        /// </summary>
        /// <param name="newItem"></param>
        protected abstract void CheckFieldsOnCreate(T newItem);

        /// <summary>
        /// Exclude Not Active nested entities if needed or fide data in fields.
        /// </summary>
        /// <param name="item"></param>
        protected abstract void FilterFieldsOnGet(T item);

    }
}
