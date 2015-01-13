using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using DataPersistence.Facade.Data;

namespace Mapler.DataAccess.RepositoryDataFiltering
{
    /// <summary>
    /// Encapsulates business logics
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepoBusinessProxy<T> : IPersistentRepository<T> where T : IPersistentModel
    {
    }
}
