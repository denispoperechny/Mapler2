using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Http.Dependencies;
using DataPersistance.Facade;
using DataPersistance.Facade.MockData;
using DataPersistence.Facade;
using Mapler.API.Controllers;
using Mapler.API.Security;
using Mapler.DataPersistance.Models;
using Mapler.DataPersistence.MockData;
using Mapler.Rest.Dto;
using Mapler.Rest.Dto.Mapping;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Unity.WebApi;

namespace Mapler.API
{
    public static class UnityConfig
    {
        public static IDependencyResolver CreateContainer()
        {
            var container = new UnityContainer();
            // register all your components with the container here
            // it is NOT necessary to register your controllers

            // e.g. container.RegisterType<ITestService, TestService>();
            RegisterTypes(container);

            //container.RegisterType<MaplerBasicAuthenticationAttribute, MaplerBasicAuthenticationAttribute>();
            //var test = container.Resolve<MaplerBasicAuthenticationAttribute>();

            return new UnityDependencyResolver(container);
        }

        private static void RegisterTypes(UnityContainer container)
        {
            // Mock data setup
            container.RegisterType<MockDataContext>(new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => MockDataFactory.GetSomeMockData()));
            container.RegisterType<IUnitOfWork, MockDataContext>();
            container.RegisterType<IDbContext, MockDataContext>();

            container.RegisterType<IPersistentRepository<Tag>, PersistentRepository<Tag>>();
            container.RegisterType<IPersistentRepository<Company>, PersistentRepository<Company>>();
            container.RegisterType<IPersistentRepository<User>, PersistentRepository<User>>();
            container.RegisterType<IPersistentRepository<UserPass>, PersistentRepository<UserPass>>();

            //TEST
            //container.RegisterType<ValuesController, ValuesController>();

            container.RegisterType<IDtoMapperFactory, MapperFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDtoMapper<TagDto, Tag>>(new InjectionFactory(c => container.Resolve<IDtoMapperFactory>().Create<TagDto, Tag>()));
        }

    }
}