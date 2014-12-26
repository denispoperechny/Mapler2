using System;
using DataPersistance.Facade;
using DataPersistance.Facade.MockData;
using DataPersistence.Facade;
using Mapler.DataPersistance.Models;
using Mapler.DataPersistence.MockData;
using Mapler.Rest.Dto;
using Mapler.Rest.Dto.Mapping;
using Mapler.Rest.Dto.Mapping.Interfaces;
using Microsoft.Practices.Unity;
using Microsoft.Practices.Unity.Configuration;

namespace MaplerAPI.App_Start
{
    /// <summary>
    /// Specifies the Unity configuration for the main _container.
    /// </summary>
    public class UnityConfig
    {
        #region Unity Container
        private static Lazy<IUnityContainer> container = new Lazy<IUnityContainer>(() =>
        {
            var container = new UnityContainer();
            RegisterTypes(container);
            return container;
        });

        /// <summary>
        /// Gets the configured Unity _container.
        /// </summary>
        public static IUnityContainer GetConfiguredContainer()
        {
            return container.Value;
        }
        #endregion

        /// <summary>Registers the type mappings with the Unity _container.</summary>
        /// <param name="container">The unity _container to configure.</param>
        /// <remarks>There is no need to register concrete types such as controllers or API controllers (unless you want to 
        /// change the defaults), as Unity allows resolving a concrete type even if it was not previously registered.</remarks>
        public static void RegisterTypes(IUnityContainer container)
        {
            // NOTE: To load from web.config uncomment the line below. Make sure to add a Microsoft.Practices.Unity.Configuration to the using statements.
            // _container.LoadConfiguration();

            // TODO: Register your types here
            
            // Mock data setup
            container.RegisterType<MockDataContext>(new ContainerControlledLifetimeManager(),
                new InjectionFactory(c => MockDataFactory.GetSomeMockData()));
            container.RegisterType<IUnitOfWork, MockDataContext>();
            container.RegisterType<IDbContext, MockDataContext>();

            container.RegisterType<IPersistentRepository<Tag>, PersistentRepository<Tag>>();
            container.RegisterType<IPersistentRepository<Company>, PersistentRepository<Company>>();

            container.RegisterType<IDtoMapperFactory, MapperFactory>(new ContainerControlledLifetimeManager());
            container.RegisterType<IDtoMapper<TagDto, Tag>>(new InjectionFactory(c => container.Resolve<IDtoMapperFactory>().Create<TagDto, Tag>()));

        }
    }
}
