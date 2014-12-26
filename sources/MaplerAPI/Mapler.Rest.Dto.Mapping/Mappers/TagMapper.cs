using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataPersistance.Models;
using Mapler.Rest.Dto.Mapping.Exceptions;
using Mapler.Rest.Dto.Mapping.Interfaces;

namespace Mapler.Rest.Dto.Mapping.Mappers
{
    /// <summary>
    /// Maps TagDto to persistent Tag object.
    /// </summary>
    public class TagMapper : IDtoMapper<TagDto, Tag>
    {
        private readonly IPersistentRepository<Company> _companyRepo; 

        public TagMapper(IPersistentRepository<Company> companyRepo)
        {
            _companyRepo = companyRepo;
        }

        public TagDto Map(Tag item)
        {
            if (item == null) throw new ArgumentNullException("item");

            if (item.Company == null)
                throw new InvalidPersistentDataException("item.Company is null");

            return new TagDto
            {
                Id = item.Id,
                CompanyId = item.Company.Id,
                Name = item.Name,
                Description = item.Description
            };
        }

        public Tag MapBack(TagDto item)
        {
            if (item == null) throw new ArgumentNullException("item");

            Company tagCompany = null;
            try
            {
                tagCompany = _companyRepo.Get(item.CompanyId);
            }
            catch (Exception e)
            {
                throw new InvalidPersistentDataException("Could not get Company. Company Id: " + item.CompanyId, e);
            }

            return new Tag
            {
                Id = item.Id,
                Company = tagCompany,
                Name = item.Name,
                Description = item.Description
            };
        }
    }
}
