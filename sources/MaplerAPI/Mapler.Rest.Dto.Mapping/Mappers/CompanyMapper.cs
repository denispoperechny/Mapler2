using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataAccess.RepositoryDataFiltering;
using Mapler.DataPersistance.Models;
using Mapler.Rest.Dto.Mapping.Exceptions;
using Mapler.Rest.Dto.Mapping.Interfaces;

namespace Mapler.Rest.Dto.Mapping.Mappers
{
    public class CompanyMapper : MapperBase, IDtoMapper<CompanyDto, Company>
    {
        private readonly IRepoBusinessProxy<User> _userRepo;

        public CompanyMapper(IRepoBusinessProxy<User> userRepo)
        {
            if (userRepo == null) throw new ArgumentNullException("userRepo");
            _userRepo = userRepo;
        }

        public CompanyDto Map(Company item)
        {
            if (item == null) throw new ArgumentNullException("item");

            return new CompanyDto
            {
                Id = item.Id,
                AdministratorId = item.Administrator == null ? Guid.Empty : item.Administrator.Id,
                Created = item.Created,
                Name = item.Name,
                UserIds = item.Users == null ? new List<Guid>() : item.Users.Select(x => x.Id).ToList()
            };
        }

        public Company MapBack(CompanyDto item)
        {
            if (item == null) throw new ArgumentNullException("item");
            if (item.UserIds == null)
                throw new MappingException("'item.UserIds' is null");

            return new Company
            {
                Id = item.Id,
                Administrator = GetPersistentItem(_userRepo, item.AdministratorId),
                Created = item.Created,
                Name = item.Name,
                Users = GetPersistentItems(_userRepo, item.UserIds)
            };
        }

        public void UpdateBack(CompanyDto dtoItem, Company persistItem)
        {
            if (dtoItem == null) throw new ArgumentNullException("dtoItem");
            if (persistItem == null) throw new ArgumentNullException("persistItem");

            if (dtoItem.Id != persistItem.Id)
                throw new MappingException("Id of source and destination objects should be the same.");
            if (dtoItem.UserIds == null)
                throw new MappingException("'dtoItem.UserIds' is null");

            persistItem.Created = persistItem.Created;
            persistItem.Name = persistItem.Name;
            persistItem.Administrator = GetPersistentItem(_userRepo, dtoItem.AdministratorId);
            persistItem.Users = GetPersistentItems(_userRepo, dtoItem.UserIds);
        }
    }
}
