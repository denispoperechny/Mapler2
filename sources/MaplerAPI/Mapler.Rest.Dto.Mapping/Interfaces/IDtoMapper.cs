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

        /// <summary>
        /// Creates a new instance of resulting object
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        U MapBack(T item);

        /// <summary>
        /// Updates the 'persistItem' accordingly to 'dtoItem'
        /// </summary>
        /// <param name="dtoItem"></param>
        /// <param name="persistItem"></param>
        void UpdateBack(T dtoItem, U persistItem);
    }

}
