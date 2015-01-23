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
    public class TagRepoProxy : RepoProxyBase<Tag>, IRepoBusinessProxy<Tag>
    {
        public TagRepoProxy(IPersistentRepository<Tag> repo)
            : base(repo)
        {
        }

        protected override List<Guid> GetReadableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            return Repository.GetAll(x => userCompanyIds.Contains(x.Company.Id)).Select(s => s.Id).ToList();
        }

        protected override List<Guid> GetEditableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            var companiesWhereAdmin = CompanyRepository.GetAll(x => x.Administrator.Id.Equals(regularUserId) && x.IsActive)
                .Select(s => s.Id).ToList();
            return Repository.GetAll(x => companiesWhereAdmin.Contains(x.Company.Id)).Select(s => s.Id).ToList();
        }

        protected override List<Guid> GetDeletableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            return GetEditableEntityIds(regularUserId, userCompanyIds);
        }

        protected override void CheckFieldsOnUpdate(Tag updatedStateItem, Tag alreadyExistingItem)
        {
            updatedStateItem.Company = alreadyExistingItem.Company;
            updatedStateItem.IsActive = alreadyExistingItem.IsActive;
            updatedStateItem.MapItems = alreadyExistingItem.MapItems;
        }

        protected override void CheckFieldsOnCreate(Tag newItem)
        {
            if (!Principal.Companies.Contains(newItem.Company.Id))
                throw new DataAccessRestrictedException("Current user is not a member of given company: " + newItem.Company.Id);
            
            if (Repository.GetAll(x => x.Name.Equals(newItem) || x.Company.Id == newItem.Company.Id).Any())
                throw new InvalidOperationException(string.Format("There is already exists a Tag with specified name ({0}) for given company: {1}", newItem.Name, newItem.Company.Id));

            newItem.Id = newItem.Id == Guid.Empty ? Guid.NewGuid() : newItem.Id;
            newItem.IsActive = true;
        }

        protected override void FilterFieldsOnGet(Tag item)
        {
            // Do nothing
            return;
        }
    }
}
