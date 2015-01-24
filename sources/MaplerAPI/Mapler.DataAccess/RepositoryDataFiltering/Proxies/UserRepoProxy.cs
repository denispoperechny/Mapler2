using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using DataPersistance.Facade;
using Mapler.DataAccess.RepositoryDataFiltering.Exceptions;
using Mapler.DataPersistance.Models;
using Mapler.Security;

namespace Mapler.DataAccess.RepositoryDataFiltering.Proxies
{
    /// <summary>
    /// Allows to read only those users who belong to current user's company
    /// </summary>
    public class UserRepoProxy : RepoProxyBase<User>, IRepoBusinessProxy<User>
    {
        public UserRepoProxy(IPersistentRepository<User> userRepo)
            : base(userRepo)
        {
        }

        protected override List<Guid> GetReadableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            var companies = CompanyRepository.GetAll(x => userCompanyIds.Contains(x.Id) && x.IsActive).ToList();

            var userIds = companies.Aggregate(new List<Guid>(), (a, b) => { 
                a.AddRange(b.Users.Select(x => x.Id));
                a.Add(b.Administrator.Id);
                return a;
            });

            return userIds.Distinct().ToList();
        }

        protected override List<Guid> GetEditableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            // Allow edit only itself user
            return new List<Guid>(new [] { regularUserId });
        }

        protected override List<Guid> GetDeletableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            // User deletion allowed only for super user
            return new List<Guid>();
        }

        protected override void CheckFieldsOnUpdate(User updatedStateItem, User alreadyExistingItem)
        {
            updatedStateItem.Login = alreadyExistingItem.Login;
            updatedStateItem.IsActive = alreadyExistingItem.IsActive;
            updatedStateItem.Created = alreadyExistingItem.Created;
            updatedStateItem.IsSuperUser = alreadyExistingItem.IsSuperUser;
            updatedStateItem.Companies = alreadyExistingItem.Companies;
        }

        protected override void CheckFieldsOnCreate(User newItem)
        {
            var alreadyExistingUser =
                Repository.GetAll(x => x.Login.Equals(newItem.Login, StringComparison.InvariantCultureIgnoreCase))
                    .FirstOrDefault();

            if (alreadyExistingUser != null)
                throw new InvalidOperationException(string.Format("User with login '{0}' already exists", alreadyExistingUser.Login));

            newItem.Created = DateTime.Now;
            newItem.IsSuperUser = false;
            newItem.Companies = new List<Company>();
        }

        protected override void FilterFieldsOnGet(User item)
        {
            item.Companies = item.Companies.Where(x => Principal.Companies.Contains(x.Id)).ToList();
            return;
        }
    }
}
