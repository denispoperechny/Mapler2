using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataPersistance.Models;

namespace Mapler.DataAccess.RepositoryDataFiltering.Proxies
{
    public class MapItemCommentRepoProxy : RepoProxyBase<MapItemComment>, IRepoBusinessProxy<MapItemComment>
    {
        private readonly IPersistentRepository<MapItem> _mapItemRepo;

        public MapItemCommentRepoProxy(IPersistentRepository<MapItemComment> repo, IPersistentRepository<MapItem> mapItemRepo)
            : base(repo)
        {
            if (mapItemRepo == null) throw new ArgumentNullException("mapItemRepo");
            _mapItemRepo = mapItemRepo;
        }

        protected override List<Guid> GetReadableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            var companyMapItems = _mapItemRepo.GetAll(x => userCompanyIds.Contains(x.Company.Id)).Select(s => s.Id).ToList();
            return Repository.GetAll(x => companyMapItems.Contains(x.MapItem.Id)).Select(s => s.Id).ToList();
        }

        protected override List<Guid> GetEditableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            var companyMapItems = _mapItemRepo.GetAll(x => userCompanyIds.Contains(x.Company.Id)).Select(s => s.Id).ToList();
            return Repository.GetAll(x => x.Author.Id == regularUserId && companyMapItems.Contains(x.MapItem.Id)).Select(s => s.Id).ToList();
        }

        protected override List<Guid> GetDeletableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            var companyMapItems = _mapItemRepo.GetAll(x => userCompanyIds.Contains(x.Company.Id)).Select(s => s.Id).ToList();
            return Repository.GetAll(x => x.Author.Id == regularUserId && companyMapItems.Contains(x.MapItem.Id)).Select(s => s.Id).ToList();
        }

        protected override void CheckFieldsOnUpdate(MapItemComment updatedStateItem, MapItemComment alreadyExistingItem)
        {
            updatedStateItem.Author = alreadyExistingItem.Author;
            updatedStateItem.Created = alreadyExistingItem.Created;
            updatedStateItem.IsActive = alreadyExistingItem.IsActive;
            updatedStateItem.MapItem = alreadyExistingItem.MapItem;
        }

        protected override void CheckFieldsOnCreate(MapItemComment newItem)
        {
            var mapItem = _mapItemRepo.Get(newItem.MapItem.Id);
            if (newItem.Author.Id != Principal.Identity.UserId)
                throw new InvalidOperationException("User can create MapItem Comment only by self name.");
            if (!Principal.Companies.Contains(mapItem.Company.Id))
                throw new InvalidOperationException("User can create MapItems Comment only under self company.");

            newItem.Created = DateTime.Now;
            newItem.IsActive = true;
        }

        protected override void FilterFieldsOnGet(MapItemComment item)
        {
            return;
        }
    }
}
