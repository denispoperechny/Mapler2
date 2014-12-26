using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto.Mapping.Interfaces
{
    public interface IDtoMapperFactory
    {
        IDtoMapper<T, U> Create<T, U>();
    }
}
