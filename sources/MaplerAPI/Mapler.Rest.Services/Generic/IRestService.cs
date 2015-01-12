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
        IEnumerable<T> Get();
        
        T Get(Guid id);

        void Post(T value);

        void Put(Guid id, T value);

        void Delete(Guid id);

        List<T> GetMultiple(IEnumerable<Guid> ids);
    }
}
