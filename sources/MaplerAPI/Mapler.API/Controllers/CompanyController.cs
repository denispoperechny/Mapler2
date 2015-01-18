using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DataPersistance.Facade;
using Mapler.DataAccess.RepositoryDataFiltering;
using Mapler.DataPersistance.Models;
using Mapler.Rest.Dto;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Mapler.Rest.Services;

namespace Mapler.API.Controllers
{
    public class CompanyController : RestControllerBase<CompanyDto, Company>, ICompanyService
    {
        public CompanyController(IDtoMapper<CompanyDto, Company> dtoMapper, IRepoBusinessProxy<Company> repository, IUnitOfWork uof)
            : base(dtoMapper, repository, uof)
        {
        }
    }
}