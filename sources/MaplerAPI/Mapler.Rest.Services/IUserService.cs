using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Mapler.Rest.Dto;
using Mapler.Rest.Services.Generic;

namespace Mapler.Rest.Services
{
    public interface IUserService : IRestService<UserDto>
    {
    }
}
