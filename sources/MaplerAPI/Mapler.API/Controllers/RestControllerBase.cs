using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Web.Http;
using System.Web.Http.Cors;
using DataPersistance.Facade;
using DataPersistence.Facade.Data;
using Mapler.API.Security;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Mapler.Rest.Services.Generic;
using Elmah.Contrib.WebApi;

namespace Mapler.API.Controllers
{
    [EnableCors(origins: "*", headers: "*", methods: "*")]
    [ElmahHandleErrorApiAttribute]
    [MaplerBasicAuthentication]
    [Authorize]
    public abstract class RestControllerBase<T, U> : ApiController, IRestService<T> where U : class, IPersistentModel
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
            var entity = Repository.Get(id);
            var result = DtoMapper.Map(entity);
            // TODO: try to use IHttpActionResult and NotFound() result
            //if (result == null)
            //    return NotFound();
            // IHttpActionResult 
            // Ok(result)
            // NotFound()
            return result;
        }

        // POST api/values?getMultiple=true
        // TODO: Review
        /// <summary>
        /// POST action to get multiple records by Id-s set.
        /// </summary>
        /// <param name="getMultiple">Not used parameter. Added to make action distinct to Post() action.</param>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public virtual List<T> GetMultiple(bool getMultiple, [FromBody]IEnumerable<Guid> ids)
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
            if (persistItem == null)
                throw new InvalidOperationException("Cannot find item Id: " + id);

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