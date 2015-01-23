using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using DataPersistence.Facade.Data;
using Mapler.DataAccess.RepositoryDataFiltering;
using Mapler.Rest.Dto.Mapping.Exceptions;
using Mapler.Rest.Dto.Mapping.Interfaces;

namespace Mapler.Rest.Dto.Mapping.Mappers
{
    public abstract class MapperBase
    {
        protected T GetPersistentItem<T>(IRepoBusinessProxy<T> repository, Guid id) where T : class, IPersistentModel
        {
            var result = repository.Get(id);
            if (result == null)
                throw new MappingException(string.Format("Failed to fetch '{0}' Id: {1}", typeof(T), id));
            return result;
        }

        protected List<T> GetPersistentItems<T>(IRepoBusinessProxy<T> repository, IEnumerable<Guid> ids) where T : class, IPersistentModel
        {
            var staticIds = ids.Distinct().ToList();
            var records = repository.GetAll(x => staticIds.Contains(x.Id)).ToList();

            if (staticIds.Count != records.Count)
                throw new MappingException(string.Format("Failed to fetch '{0}' Ids: {1}", typeof(T),
                    string.Join(", ", staticIds.Select(x => !records.Any(a => a.Id == x)))));

            return records;
        }
    }
}
