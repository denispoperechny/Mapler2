using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto.Mapping.Interfaces
{
    public interface IDtoMapper<T, U>
    {
        T Map(U item);

        U MapBack(T item);
    }

}
