using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.Rest.Services.Generic
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T">DTO object</typeparam>
    public interface IRestService<T>
    {
        /// <summary>
        /// GET http method. Returns all records from persistent storage.
        /// </summary>
        /// <returns></returns>
        IEnumerable<T> Get();
        
        /// <summary>
        /// GET http method.
        /// </summary>
        /// <param name="id">Item's Id</param>
        /// <returns></returns>
        T Get(Guid id);

        /// <summary>
        /// POST http method. Saves the new item to persistent storage.
        /// </summary>
        /// <param name="value"></param>
        void Post(T value);

        /// <summary>
        /// PUT http method. Updates the item.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="value"></param>
        void Put(Guid id, T value);

        /// <summary>
        /// DELETE http method.
        /// </summary>
        /// <param name="id"></param>
        void Delete(Guid id);

        /// <summary>
        /// POST http method. Action to get multiple records by Id-s set.
        /// </summary>
        /// <param name="getMultiple">Not used parameter. Added to make action distinct to Post() action.</param>
        /// <param name="ids">Id-s set to fetch resulting items collection</param>
        /// <returns></returns>
        List<T> GetMultiple(bool getMultiple, IEnumerable<Guid> ids);
    }
}
