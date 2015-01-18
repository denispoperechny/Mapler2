using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataPersistance.Models;

namespace Mapler.DataAccess.RepositoryDataFiltering.Proxies
{
    public class AttachmentRepoProxy : RepoProxyBase<Attachment>, IRepoBusinessProxy<Attachment>
    {
        private readonly IPersistentRepository<MapItem> _mapItemRepo;
        private readonly IPersistentRepository<Company> _companyRepo;

        public AttachmentRepoProxy(IPersistentRepository<Attachment> repo, IPersistentRepository<MapItem> mapItemRepo,
            IPersistentRepository<Company> companyRepo) 
            : base(repo)
        {
            if (mapItemRepo == null) throw new ArgumentNullException("mapItemRepo");
            if (companyRepo == null) throw new ArgumentNullException("companyRepo");
            _mapItemRepo = mapItemRepo;
            _companyRepo = companyRepo;
        }

        protected override List<Guid> GetReadableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            var copmanyMapItems = _mapItemRepo.GetAll(x => userCompanyIds.Contains(x.Company.Id))
                .Select(s => s.Id).ToList();

            return Repository.GetAll(x => copmanyMapItems.Contains(x.MapItem.Id))
                .Select(s => s.Id).ToList();
        }

        protected override List<Guid> GetEditableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            return GetDeletableEntityIds(regularUserId, userCompanyIds);
        }

        protected override List<Guid> GetDeletableEntityIds(Guid regularUserId, List<Guid> userCompanyIds)
        {
            // own within user's company and company admin
            var copmanyMapItems = _mapItemRepo.GetAll(x => userCompanyIds.Contains(x.Company.Id))
                .Select(s => s.Id).ToList();

            //own
            var result = Repository.GetAll(x => x.Author.Id == regularUserId && copmanyMapItems.Contains(x.MapItem.Id))
                .Select(s => s.Id).ToList();

            //admin
            var adminCompanyIds = _companyRepo.GetAll(x => x.Administrator.Id == regularUserId)
                .Select(s => s.Id).ToList();
            var adminCopmanyMapItems = _mapItemRepo.GetAll(x => adminCompanyIds.Contains(x.Company.Id))
                .Select(s => s.Id).ToList();
            result.AddRange(Repository.GetAll(x => adminCopmanyMapItems.Contains(x.MapItem.Id)).Select(s => s.Id));

            return result;
        }

        protected override void CheckFieldsOnUpdate(Attachment updatedStateItem, Attachment alreadyExistingItem)
        {
            updatedStateItem.Author = alreadyExistingItem.Author;
            updatedStateItem.Created = alreadyExistingItem.Created;
            updatedStateItem.FileName = alreadyExistingItem.FileName;
            updatedStateItem.IsActive = alreadyExistingItem.IsActive;
            updatedStateItem.MapItem = alreadyExistingItem.MapItem;
        }

        protected override void CheckFieldsOnCreate(Attachment newItem)
        {
            if (newItem.Author.Id != Principal.Identity.UserId)
                newItem.Author = UserRepository.Get(Principal.Identity.UserId);

            newItem.Id = newItem.Id == Guid.Empty ? Guid.NewGuid() : newItem.Id;
            newItem.Created = DateTime.Now;
        }

        protected override void FilterFieldsOnGet(Attachment item)
        {
            return;
        }
    }
}
