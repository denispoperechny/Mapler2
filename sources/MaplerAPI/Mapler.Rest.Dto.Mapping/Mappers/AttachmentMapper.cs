using Mapler.DataPersistance.Models;
using Mapler.Rest.Dto.Mapping.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataAccess.RepositoryDataFiltering;
using Mapler.Rest.Dto.Mapping.Exceptions;

namespace Mapler.Rest.Dto.Mapping.Mappers
{
    public class AttachmentMapper : MapperBase, IDtoMapper<AttachmentDto, Attachment>
    {
        private readonly IRepoBusinessProxy<MapItem> _mapItemRepo;
        private readonly IRepoBusinessProxy<User> _userRepo;

        public AttachmentMapper(IRepoBusinessProxy<MapItem> mapItemRepo, IRepoBusinessProxy<User> userRepo)
        {
            if (mapItemRepo == null) throw new ArgumentNullException("mapItemRepo");
            if (userRepo == null) throw new ArgumentNullException("userRepo");
            _mapItemRepo = mapItemRepo;
            _userRepo = userRepo;
        }

        public AttachmentDto Map(Attachment item)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (item.MapItem == null)
                throw new MappingException("'item.MapItem' is null");

            return new AttachmentDto
            {
                Id = item.Id,
                Created = item.Created,
                Description = item.Description,
                FileName = item.FileName,
                MapItemId = item.MapItem.Id,
                AuthorId = item.Author.Id,
                AuthorName = item.Author.GetFullName()
            };
        }

        public Attachment MapBack(AttachmentDto item)
        {
            if (item == null) throw new ArgumentNullException("item");

            MapItem mapItem = GetPersistentItem(_mapItemRepo, item.MapItemId);
            User author = _userRepo.Get(item.AuthorId);

            return new Attachment
            {
                Id = item.Id,
                Created = item.Created,
                Description = item.Description,
                FileName = item.FileName,
                MapItem = mapItem,
                Author = author
            };
        }

        public void UpdateBack(AttachmentDto dtoItem, Attachment persistItem)
        {
            if (dtoItem == null) throw new ArgumentNullException("dtoItem");
            if (persistItem == null) throw new ArgumentNullException("persistItem");

            if (dtoItem.Id != persistItem.Id)
                throw new MappingException("Id of source and destination objects should be the same.");

            if (dtoItem.MapItemId != persistItem.MapItem.Id)
                persistItem.MapItem = GetPersistentItem(_mapItemRepo, dtoItem.MapItemId);
            if (dtoItem.AuthorId != persistItem.Author.Id)
                persistItem.Author = GetPersistentItem(_userRepo, dtoItem.AuthorId);

            persistItem.Created = persistItem.Created;
            persistItem.FileName = dtoItem.FileName;
            persistItem.Description = persistItem.Description;
        }

    }
}
