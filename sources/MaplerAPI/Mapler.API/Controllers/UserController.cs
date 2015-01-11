using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataPersistance.Facade;
using Mapler.DataPersistance.Models;
using Mapler.Rest.Dto;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Mapler.Rest.Services;
using Microsoft.Practices.Unity;

namespace Mapler.API.Controllers
{
    public class UserController : RestControllerBase<UserDto, User>, IUserService
    {
        public UserController(IDtoMapper<UserDto, User> dtoMapper, [Dependency("filtered")] IPersistentRepository<User> repository, IUnitOfWork uof)
            : base(dtoMapper, repository, uof)
        {
        }
    }
}