using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using DataPersistance.Facade;
using DataPersistence.Facade.Data;
using Mapler.API.Security;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Mapler.Rest.Services.Generic;

namespace Mapler.API.Controllers
{
    [MaplerBasicAuthentication]
    [Authorize]
    public abstract class RestControllerBase<T, U> : ApiController, IRestService<T> where U : IPersistentModel
    {
        private readonly IDtoMapper<T, U> _dtoMapper;
        private readonly IPersistentRepository<U> _repository;
        private readonly IUnitOfWork _unitOfWork;

        // TODO: Add data filtering
        protected RestControllerBase(IDtoMapper<T, U> dtoMapper, IPersistentRepository<U> repository, IUnitOfWork unitOfWork)
        {
            if (dtoMapper == null) throw new ArgumentNullException("dtoMapper");
            if (repository == null) throw new ArgumentNullException("repository");
            if (unitOfWork == null) throw new ArgumentNullException("unitOfWork");

            _dtoMapper = dtoMapper;
            _repository = repository;
            _unitOfWork = unitOfWork;
        }

        protected IDtoMapper<T, U> DtoMapper
        {
            get { return _dtoMapper; }
        }

        protected IPersistentRepository<U> Repository
        {
            get { return _repository; }
        }

        protected IUnitOfWork UnitOfWork
        {
            get { return _unitOfWork; }
        }

        // GET api/values
        public virtual IEnumerable<T> Get()
        {
            return Repository.GetAll().Select(x => DtoMapper.Map(x));
        }

        // GET api/values/5
        public virtual T Get(Guid id)
        {
            return DtoMapper.Map(Repository.Get(id));
        }

        [HttpPost]
        public virtual List<T> GetMultiple(IEnumerable<Guid> ids)
        {
            ids = ids.ToList();
            var matchedItems = Repository.GetAll(x => ids.Contains(x.Id)).ToList();
            return matchedItems.Select(x => DtoMapper.Map(x)).ToList();
        }

        // POST api/values
        public virtual void Post([FromBody]T value)
        {
            Repository.Add(DtoMapper.MapBack(value));
            UnitOfWork.Save();
        }

        // PUT api/values/5
        public virtual void Put(Guid id, [FromBody]T value)
        {
            var persistItem = Repository.Get(id);
            DtoMapper.UpdateBack(value, persistItem);
            Repository.Update(persistItem);
            UnitOfWork.Save();
        }

        // DELETE api/values/5
        public virtual void Delete(Guid id)
        {
            Repository.Delete(id);
            UnitOfWork.Save();
        }
    }
}