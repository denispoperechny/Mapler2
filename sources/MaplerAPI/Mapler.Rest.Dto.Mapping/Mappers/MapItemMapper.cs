using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataAccess.RepositoryDataFiltering;
using Mapler.DataPersistance.Models;
using Mapler.Rest.Dto.Mapping.Exceptions;
using Mapler.Rest.Dto.Mapping.Interfaces;

namespace Mapler.Rest.Dto.Mapping.Mappers
{
    public class MapItemMapper : MapperBase, IDtoMapper<MapItemDto, MapItem>
    {
        private readonly IRepoBusinessProxy<User> _userRepo;
        private readonly IRepoBusinessProxy<Tag> _tagRepo;
        private readonly IRepoBusinessProxy<Company> _companyRepo;
        private readonly IRepoBusinessProxy<MapItemComment> _commentRepo;
        private readonly IRepoBusinessProxy<Attachment> _attachmentRepo;

        public MapItemMapper(IRepoBusinessProxy<User> userRepo, IRepoBusinessProxy<Tag> tagRepo,
            IRepoBusinessProxy<Company> companyRepo, IRepoBusinessProxy<MapItemComment> commentRepo,
            IRepoBusinessProxy<Attachment> attachmentRepo)
        {
            if (userRepo == null) throw new ArgumentNullException("userRepo");
            if (tagRepo == null) throw new ArgumentNullException("tagRepo");
            if (companyRepo == null) throw new ArgumentNullException("companyRepo");
            if (commentRepo == null) throw new ArgumentNullException("commentRepo");
            if (attachmentRepo == null) throw new ArgumentNullException("attachmentRepo");
            _userRepo = userRepo;
            _tagRepo = tagRepo;
            _companyRepo = companyRepo;
            _commentRepo = commentRepo;
            _attachmentRepo = attachmentRepo;
        }

        public MapItemDto Map(MapItem item)
        {
            if (item == null) throw new ArgumentNullException("item");

            return new MapItemDto
            {
                Id = item.Id,
                AuthorFullName = item.Author.GetFullName(),
                AuthorId = item.Author.Id,
                Color = item.Color,
                CompanyId = item.Company.Id,
                CompanyName = item.Company.Name,
                Created = item.Created,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                Name = item.Name,
                TagIds = item.Tags.Select(x => x.Id).ToList(),
                CommentIds = _commentRepo.GetAll(x => x.MapItem.Id == item.Id)
                    .Select(s => s.Id).ToList(),
                AttachmentIds = _attachmentRepo.GetAll(x => x.MapItem.Id == item.Id)
                    .Select(s => s.Id).ToList()
            };
        }

        public MapItem MapBack(MapItemDto item)
        {
            if (item == null) throw new ArgumentNullException("item");

            if (item.TagIds == null)
                throw new MappingException("'item.TagIds' is null");

            return new MapItem
            {
                Id = item.Id,
                Author = GetPersistentItem(_userRepo, item.AuthorId),
                Company = GetPersistentItem(_companyRepo, item.CompanyId),
                Tags = GetPersistentItems(_tagRepo, item.TagIds),
                Color = item.Color,
                Created = item.Created,
                Latitude = item.Latitude,
                Longitude = item.Longitude,
                Name = item.Name
            };
        }

        public void UpdateBack(MapItemDto dtoItem, MapItem persistItem)
        {
            if (dtoItem == null) throw new ArgumentNullException("dtoItem");
            if (persistItem == null) throw new ArgumentNullException("persistItem");
            if (dtoItem.Id != persistItem.Id)
                throw new MappingException("Id of source and destination objects should be the same.");

            if (dtoItem.TagIds == null)
                throw new MappingException("'item.TagIds' is null");

            if (dtoItem.AuthorId != persistItem.Author.Id)
                persistItem.Author = GetPersistentItem(_userRepo, dtoItem.AuthorId);
            if (dtoItem.CompanyId != persistItem.Company.Id)
                persistItem.Company = GetPersistentItem(_companyRepo, dtoItem.CompanyId);

            persistItem.Tags = GetPersistentItems(_tagRepo, dtoItem.TagIds);
            persistItem.Color = dtoItem.Color;
            persistItem.Created = dtoItem.Created;
            persistItem.Latitude = dtoItem.Latitude;
            persistItem.Longitude = dtoItem.Longitude;
            persistItem.Name = dtoItem.Name;
        }

    }
}
