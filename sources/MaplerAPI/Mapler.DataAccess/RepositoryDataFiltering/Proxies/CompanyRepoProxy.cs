using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataAccess.RepositoryDataFiltering.Exceptions;
using Mapler.DataPersistance.Models;

namespace Mapler.DataAccess.RepositoryDataFiltering.Proxies
{
    public class CompanyRepoProxy : RepoProxyBase<Company>, IRepoBusinessProxy<Company>
    {
        private readonly IPersistentRepository<User> _userRepo;
        private readonly IPersistentRepository<Location> _locationRepo;

        public CompanyRepoProxy(IPersistentRepository<Company> companyRepo, IPersistentRepository<User> userRepo, 
            IPersistentRepository<Location> locationRepo)
            :base(companyRepo)
        {
            if (userRepo == null) throw new ArgumentNullException("userRepo");
            if (locationRepo == null) throw new ArgumentNullException("locationRepo");
            _userRepo = userRepo;
            _locationRepo = locationRepo;
        }

        protected override List<Guid> GetReadableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            // Allow all users to see all companies in the system but hide some fields
            return Repository.GetAll(x => x.IsActive).Select(x => x.Id).ToList();
        }

        protected override List<Guid> GetEditableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            // Allow edit only those companies where user is Admin
            return Repository.GetAll(x => x.IsActive && x.Administrator.Id == regularUserId)
                .Select(x => x.Id).ToList();
        }

        protected override List<Guid> GetDeletableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            // Don't allow to delete companies for users
            return new List<Guid>();
        }

        protected override void CheckFieldsOnUpdate(Company updatedStateItem, Company alreadyExistingItem)
        {
            if (alreadyExistingItem.Administrator.Id != Principal.Identity.UserId)
                throw new DataAccessRestrictedException(typeof(Company), alreadyExistingItem.Id, 
                    Principal.Identity.Name, "Only administrator can edit company details");

            var existingCompany =
                Repository.GetAll(x => x.Name.Equals(updatedStateItem.Name, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
            if (existingCompany != null)
                throw new InvalidOperationException("The company with given name already exists: " + updatedStateItem.Name);

            updatedStateItem.Created = alreadyExistingItem.Created;
            updatedStateItem.IsActive = alreadyExistingItem.IsActive;
        }

        protected override void CheckFieldsOnCreate(Company newItem)
        {
            var existingCompany =
                Repository.GetAll(x => x.Name.Equals(newItem.Name, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();
            if (existingCompany != null)
                throw new InvalidOperationException("The company with given name already exists: " + newItem.Name);

            newItem.Created = DateTime.Now;
            newItem.Id = newItem.Id == Guid.Empty ? Guid.NewGuid() : newItem.Id;
            newItem.IsActive = true;
            newItem.Administrator = _userRepo.Get(Principal.Identity.UserId);
            newItem.DefaultLocation = GetDefaultLocation();
        }

        protected override void FilterFieldsOnGet(Company item)
        {
            item.Users = item.Users.Where(x => x.IsActive).ToList();

            // For everyone excepting admin of company and its users
            if (item.Administrator.Id != Principal.Identity.UserId
                && item.Users.All(x => x.Id != Principal.Identity.UserId))
            {
                item.Administrator = null;
                item.Created = new DateTime();
                item.DefaultLocation = null;
                item.Users = new List<User>();
            }
        }

        private Location GetDefaultLocation()
        {
            var existing = _locationRepo.GetAll(x => x.IsSystemDefault && x.IsActive).FirstOrDefault();
            if (existing != null)
                return existing;

            var newLocation = new Location()
            {
                Id = Guid.NewGuid(),
                IsActive = true,
                IsSystemDefault = true,
                Latitude = 1,
                Longitude = 1,
                Zoom = 5
            };
            _locationRepo.Add(newLocation);

            return newLocation;
        }
    }
}
