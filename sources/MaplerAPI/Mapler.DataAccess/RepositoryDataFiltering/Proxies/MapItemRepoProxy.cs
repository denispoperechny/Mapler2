using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataPersistance.Models;

namespace Mapler.DataAccess.RepositoryDataFiltering.Proxies
{
    public class MapItemRepoProxy : RepoProxyBase<MapItem>, IRepoBusinessProxy<MapItem>
    {
        public MapItemRepoProxy(IPersistentRepository<MapItem> mapItemRepo)
            : base(mapItemRepo)
        {
        }

        protected override List<Guid> GetReadableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            return Repository.GetAll(x => userCompanyIds.Contains(x.Company.Id))
                .Select(s => s.Id).ToList();
        }

        protected override List<Guid> GetEditableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            var companies = CompanyRepository.GetAll(x => userCompanyIds.Contains(x.Id)).ToList();
            List<Guid> result = new List<Guid>();
            foreach (var company in companies)
            {
                result.AddRange(
                    company.Administrator.Id == regularUserId
                    ? Repository.GetAll(x => x.Company.Id == company.Id).Select(s => s.Id)
                    : Repository.GetAll(x => x.Company.Id == company.Id && x.Author.Id == regularUserId).Select(s => s.Id)
                    );
            }
            return result;
        }

        protected override List<Guid> GetDeletableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            var administratedCompanies = CompanyRepository.GetAll(x => x.Administrator.Id == regularUserId)
                .Select(s => s.Id).ToList();
            return Repository.GetAll(x => administratedCompanies.Contains(x.Company.Id))
                .Select(s => s.Id).ToList();
        }

        protected override void CheckFieldsOnUpdate(MapItem updatedStateItem, MapItem alreadyExistingItem)
        {
            updatedStateItem.Company = alreadyExistingItem.Company;
            updatedStateItem.Author = alreadyExistingItem.Author;
            updatedStateItem.IsActive = alreadyExistingItem.IsActive;
            updatedStateItem.Created = alreadyExistingItem.Created;
        }

        protected override void CheckFieldsOnCreate(MapItem newItem)
        {
            if (newItem.Author.Id != Principal.Identity.UserId)
                throw new InvalidOperationException("User can create MapItems only by self name.");
            if (!Principal.Companies.Contains(newItem.Company.Id))
                throw new InvalidOperationException("User can create MapItems only under self company.");

            newItem.IsActive = true;
            newItem.Created = DateTime.Now;
            newItem.Id = newItem.Id == Guid.Empty ? Guid.NewGuid() : newItem.Id;
        }

        protected override void FilterFieldsOnGet(MapItem item)
        {
            item.Tags = item.Tags.Where(x => x.IsActive).ToList();
        }
    }
}
