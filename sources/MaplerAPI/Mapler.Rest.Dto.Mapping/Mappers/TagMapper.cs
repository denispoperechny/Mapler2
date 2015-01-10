﻿using System;
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
                Id = item.Id == Guid.Empty ? Guid.NewGuid() : item.Id,
                Company = tagCompany,
                Name = item.Name,
                Description = item.Description
            };
        }


        public void UpdateBack(TagDto dtoItem, Tag persistItem)
        {
            if (dtoItem == null) throw new ArgumentNullException("dtoItem");
            if (persistItem == null) throw new ArgumentNullException("persistItem");

            //persistItem.Id = dtoItem.Id;
            persistItem.Name = dtoItem.Name;
            persistItem.Description = dtoItem.Description;

            if (persistItem.Company == null || persistItem.Company.Id != dtoItem.CompanyId)
            {
                try
                {
                    persistItem.Company = _companyRepo.Get(dtoItem.CompanyId);
                }
                catch (Exception e)
                {
                    throw new InvalidPersistentDataException("Could not get Company. Company Id: " + dtoItem.CompanyId, e);
                }
            }
        }
    }
}
