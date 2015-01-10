using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataPersistance.Facade;
using Mapler.DataPersistance.Models;
using Mapler.Rest.Dto;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Mapler.Rest.Services;

namespace Mapler.API.Controllers
{
    public class TagController : RestControllerBase<TagDto, Tag>, ITagService
    {
        public TagController(IDtoMapper<TagDto, Tag> dtoMapper, IPersistentRepository<Tag> repository)
            : base(dtoMapper, repository)
        {
        }
    }
}