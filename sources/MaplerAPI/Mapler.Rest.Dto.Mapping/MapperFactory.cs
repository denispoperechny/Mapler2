using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataPersistance.Models;
using Mapler.Rest.Dto.Mapping.Exceptions;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Mapler.Rest.Dto.Mapping.Mappers;

namespace Mapler.Rest.Dto.Mapping
{
    public class MapperFactory : IDtoMapperFactory
    {
        private readonly IPersistentRepository<Company> _companyRepo;

        public MapperFactory(IPersistentRepository<Company> companyRepo)
        {
            _companyRepo = companyRepo;
        }

        public IDtoMapper<T, U> Create<T, U>()
        {
            object result = null;

            if (typeof (T) == typeof (TagDto) && typeof (U) == typeof (Tag))
                result = new TagMapper(_companyRepo);
            else
                throw new NoMapperAvailableException<T, U>();

            return (IDtoMapper<T, U>)result;
        }
    }
}
