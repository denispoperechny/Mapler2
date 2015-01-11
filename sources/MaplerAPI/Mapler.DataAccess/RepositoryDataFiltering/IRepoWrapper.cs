using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using DataPersistence.Facade.Data;

namespace Mapler.DataAccess.RepositoryDataFiltering
{
    public interface IRepoWrapper<T> : IPersistentRepository<T> where T : IPersistentModel
    {
    }
}
