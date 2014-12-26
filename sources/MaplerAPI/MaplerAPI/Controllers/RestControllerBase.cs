using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using DataPersistance.Facade;
using DataPersistence.Facade.Data;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Mapler.Rest.Services.Generic;

namespace MaplerAPI.Controllers
{
    public abstract class RestControllerBase<T, U> : ApiController, IRestService<T> where U : IPersistentModel
    {
        private readonly IDtoMapper<T, U> _dtoMapper;
        private readonly IPersistentRepository<U> _repository;

        // TODO: Add data filtering
        protected RestControllerBase(IDtoMapper<T, U> dtoMapper, IPersistentRepository<U> repository)
        {
            if (dtoMapper == null) throw new ArgumentNullException("dtoMapper");
            if (repository == null) throw new ArgumentNullException("repository");

            _dtoMapper = dtoMapper;
            _repository = repository;
        }

        protected IDtoMapper<T, U> DtoMapper
        {
            get { return _dtoMapper; }
        }

        protected IPersistentRepository<U> Repository
        {
            get { return _repository; }
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

        // POST api/values
        public virtual void Post([FromBody]T value)
        {
            Repository.Add(DtoMapper.MapBack(value));
        }

        // PUT api/values/5
        public virtual void Put(Guid id, [FromBody]T value)
        {
            Repository.Update(DtoMapper.MapBack(value));
        }

        // DELETE api/values/5
        public virtual void Delete(Guid id)
        {
            Repository.Delete(id);
        }
    }
}