using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataAccess.RepositoryDataFiltering.Exceptions;
using Mapler.DataPersistance.Models;
using Mapler.Security;

namespace Mapler.DataAccess.RepositoryDataFiltering
{
    /// <summary>
    /// Allows to read only those users who belong to current user's company
    /// </summary>
    public class UserRepoWrapper : IPersistentRepository<User>, IRepoWrapper<User>
    {
        // TODO: Add logging

        private readonly IPersistentRepository<User> _userRepo;
        private readonly IPersistentRepository<Company> _companyRepo;

        public UserRepoWrapper(IPersistentRepository<User> userRepo, IPersistentRepository<Company> companyRepo)
        {
            if (userRepo == null) throw new ArgumentNullException("userRepo");
            if (companyRepo == null) throw new ArgumentNullException("companyRepo");
            _userRepo = userRepo;
            _companyRepo = companyRepo;
        }

        private MaplerPrincipal Principal
        {
            get
            {
                MaplerPrincipal result = Thread.CurrentPrincipal as MaplerPrincipal;
                if (result == null)
                    throw new InvalidOperationException("Current principal is not MaplerPrincipal");

                return result;
            }
        }



        private List<Guid> GetReadableUserIds()
        {
            // TODO: Add caching

            var companies = _companyRepo.GetAll(x => x.Users.Any(a => a.Id == Principal.Identity.UserId) 
                || x.Administrator.Id == Principal.Identity.UserId)
                .ToList();

            var userIds = companies.Aggregate(new List<Guid>(), (a, b) => { 
                a.AddRange(b.Users.Select(x => x.Id));
                return a;
            });
            userIds.AddRange(companies.Select(x => x.Administrator.Id));

            return userIds;
        }

        public User Get(Guid id)
        {
            if (Principal.IsSuperUser || GetReadableUserIds().Contains(id))
                return _userRepo.Get(id);
            else
                throw new DataAccessRestrictedException(typeof(User), id, Principal.Identity.Name);
        }

        public IEnumerable<User> GetAll()
        {
            if (Principal.IsSuperUser)
                return _userRepo.GetAll();

            var allowedIds = GetReadableUserIds();
            return _userRepo.GetAll(x => allowedIds.Contains(x.Id));
        }

        public IEnumerable<User> GetAll(Func<User, bool> filterPredicate)
        {
            if (Principal.IsSuperUser)
                return _userRepo.GetAll(filterPredicate);

            var allowedIds = GetReadableUserIds();
            return _userRepo.GetAll(x => allowedIds.Contains(x.Id)).Where(filterPredicate);
        }

        public void Add(User newItem)
        {
            // TODO: Add logging

            // User addition is allowed for any logged in user (probably system account)
            _userRepo.Add(newItem);
        }

        public void Update(User updatedState)
        {
            // TODO: Add logging

            if (Principal.IsSuperUser)
                _userRepo.Update(updatedState);

            // Allow update only itself
            if (Principal.Identity.UserId == updatedState.Id
                && Principal.Identity.Name == updatedState.Login)
                _userRepo.Update(updatedState);
        }

        public void Delete(Guid id)
        {
            // TODO: Add logging

            if (Principal.IsSuperUser)
                _userRepo.Delete(id);

            throw new DataAccessRestrictedException("User deletion not permitted");
        }
    }
}
