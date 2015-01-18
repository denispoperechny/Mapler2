using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataPersistance.Facade;
using Mapler.DataAccess.RepositoryDataFiltering;
using Mapler.Rest.Dto;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Mapler.Rest.Services;
using Mapler.DataPersistance.Models;

namespace Mapler.API.Controllers
{
    public class AttachmentController : RestControllerBase<AttachmentDto, Attachment>, IAttachmentService
    {
        public AttachmentController(IDtoMapper<AttachmentDto, Attachment> dtoMapper, IRepoBusinessProxy<Attachment> repository, IUnitOfWork uof)
            : base(dtoMapper, repository, uof)
        {
        }
    }
}