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
    public class MapItemCommentMapper : MapperBase, IDtoMapper<MapItemCommentDto, MapItemComment>
    {
        private readonly IRepoBusinessProxy<User> _userRepo;
        private readonly IRepoBusinessProxy<MapItem> _mapItemRepo;

        public MapItemCommentMapper(IRepoBusinessProxy<User> userRepo, IRepoBusinessProxy<MapItem> mapItemRepo)
        {
            if (userRepo == null) throw new ArgumentNullException("userRepo");
            if (mapItemRepo == null) throw new ArgumentNullException("mapItemRepo");
            _userRepo = userRepo;
            _mapItemRepo = mapItemRepo;
        }

        public MapItemCommentDto Map(MapItemComment item)
        {
            if (item == null) throw new ArgumentNullException("item");

            return new MapItemCommentDto
            {
                Id = item.Id,
                AuthorFullName = item.Author.GetFullName(),
                AuthorId = item.Author.Id,
                Created = item.Created,
                MapItemId = item.MapItem.Id,
                Text = item.Text
            };
        }

        public MapItemComment MapBack(MapItemCommentDto item)
        {
            if (item == null) throw new ArgumentNullException("item");

            var result = new MapItemComment()
            {
                Id = item.Id,
                Author = GetPersistentItem(_userRepo, item.AuthorId),
                MapItem = GetPersistentItem(_mapItemRepo, item.MapItemId),
                Created = item.Created,
                Text = item.Text
            };

            return result;
        }

        public void UpdateBack(MapItemCommentDto dtoItem, MapItemComment persistItem)
        {
            if (dtoItem == null) throw new ArgumentNullException("dtoItem");
            if (persistItem == null) throw new ArgumentNullException("persistItem");

            if (dtoItem.Id != persistItem.Id)
                throw new MappingException("Id of source and destination objects should be the same.");

            if (dtoItem.AuthorId != persistItem.Author.Id)
                persistItem.Author = GetPersistentItem(_userRepo, dtoItem.AuthorId);
            if (dtoItem.MapItemId != persistItem.MapItem.Id)
                persistItem.MapItem = GetPersistentItem(_mapItemRepo, dtoItem.MapItemId);

            persistItem.Created = dtoItem.Created;
            persistItem.Text = dtoItem.Text;
        }

    }
}
