using Microsoft.Practices.Unity;
using System.Web.Http;
using System.Web.Http.Dependencies;
using DataPersistance.Facade;
using DataPersistance.Facade.MockData;
using DataPersistence.Facade;
using Mapler.API.Controllers;
using Mapler.API.Security;
using Mapler.DataAccess.RepositoryDataFiltering;
using Mapler.DataAccess.RepositoryDataFiltering.Proxies;
using Mapler.DataPersistance.Models;
using Mapler.DataPersistence.MockData;
using Mapler.Rest.Dto;
using Mapler.Rest.Dto.Mapping;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Mapler.Rest.Dto.Mapping.Mappers;
using Unity.WebApi;
using Mapler.DataPersistence.EntityFramework.EFContext;
using System;

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
            ////Mock data setup
            //container.RegisterType<MockDataContext>(new ContainerControlledLifetimeManager(),
            //    new InjectionFactory(c => MockDataFactory.GetSomeMockData()));
            //container.RegisterType<IUnitOfWork, MockDataContext>();
            //container.RegisterType<IDbContext, MockDataContext>();

            // EF
            container.RegisterType<MaplerContext>(new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => new MaplerContext()));
            container.RegisterType<IUnitOfWork, MaplerContext>();
            container.RegisterType<IDbContext, MaplerContext>();

            // Persisntent data access
            container.RegisterType<IPersistentRepository<Tag>, PersistentRepository<Tag>>();
            container.RegisterType<IPersistentRepository<Company>, PersistentRepository<Company>>();
            container.RegisterType<IPersistentRepository<User>, PersistentRepository<User>>();
            container.RegisterType<IPersistentRepository<UserPass>, PersistentRepository<UserPass>>();
            container.RegisterType<IPersistentRepository<MapItem>, PersistentRepository<MapItem>>();
            container.RegisterType<IPersistentRepository<MapItemComment>, PersistentRepository<MapItemComment>>();
            container.RegisterType<IPersistentRepository<Attachment>, PersistentRepository<Attachment>>();
            container.RegisterType<IPersistentRepository<Location>, PersistentRepository<Location>>();

            // Register data business layer
            container.RegisterType<IRepoBusinessProxy<User>, UserRepoProxy>();
            container.RegisterType<IRepoBusinessProxy<Company>, CompanyRepoProxy>();
            container.RegisterType<IRepoBusinessProxy<MapItem>, MapItemRepoProxy>();
            container.RegisterType<IRepoBusinessProxy<MapItemComment>, MapItemCommentRepoProxy>();
            container.RegisterType<IRepoBusinessProxy<Tag>, TagRepoProxy>();
            container.RegisterType<IRepoBusinessProxy<Attachment>, AttachmentRepoProxy>();

            // DTO Mappers
            container.RegisterType<IDtoMapper<TagDto, Tag>, TagMapper>();
            container.RegisterType<IDtoMapper<UserDto, User>, UserMapper>();
            container.RegisterType<IDtoMapper<CompanyDto, Company>, CompanyMapper>();
            container.RegisterType<IDtoMapper<MapItemDto, MapItem>, MapItemMapper>();
            container.RegisterType<IDtoMapper<MapItemCommentDto, MapItemComment>, MapItemCommentMapper>();
            container.RegisterType<IDtoMapper<AttachmentDto, Attachment>, AttachmentMapper>();

            //test
            //container.RegisterType<object, object>("filtered");

            // controllers
            //container.RegisterType<TagController, TagController>();

        }

    }
}