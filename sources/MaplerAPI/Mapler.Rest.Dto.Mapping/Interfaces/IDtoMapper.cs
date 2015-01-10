using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Dto.Mapping.Interfaces
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">DTO type</typeparam>
    /// <typeparam name="U">Persistent type</typeparam>
    public interface IDtoMapper<T, U>
    {
        T Map(U item);

        U MapBack(T item);

        void UpdateBack(T dtoItem, U persistItem);
    }

}
