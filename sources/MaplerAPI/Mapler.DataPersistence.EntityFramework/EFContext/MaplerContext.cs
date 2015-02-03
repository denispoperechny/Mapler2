using DataPersistance.Facade;
using DataPersistence.Facade;
using DataPersistence.Facade.Data;
using Mapler.DataPersistance.Models;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Core.Objects;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.DataPersistence.EntityFramework.EFContext
{
    public class MaplerContext : DbContext, IDbContext, IUnitOfWork
    {
        private readonly object _operationLock = new Object();

        public MaplerContext()
            : this("MaplerDB")
        {
        }

        public MaplerContext(string connectionString)
            : base(connectionString)
        {
            this.Configuration.LazyLoadingEnabled = false;
            this.Configuration.ProxyCreationEnabled = false;
            // Disabled due to IDbContext.Update<T>(T entity) idea
            this.Configuration.AutoDetectChangesEnabled = false;
        }

        #region EntityFramework
        public DbSet<Company> Companies { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<MapItem> MapItems { get; set; }
        public DbSet<MapItemComment> MapItemComments { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Attachment> Attachments { get; set; }
        public DbSet<Location> Locations { get; set; }
        public DbSet<UserPass> UserPasswords { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Conventions.Remove<OneToManyCascadeDeleteConvention>();

            modelBuilder.Entity<Company>()
                   .HasMany<User>(s => s.Users)
                   .WithMany(c => c.Companies)
                   .Map(cs =>
                   {
                       cs.MapLeftKey("CompanyId");
                       cs.MapRightKey("UserId");
                       cs.ToTable("UserCompany");
                   });

            modelBuilder.Entity<Tag>()
                   .HasMany<MapItem>(s => s.MapItems)
                   .WithMany(c => c.Tags)
                   .Map(cs =>
                   {
                       cs.MapLeftKey("TagId");
                       cs.MapRightKey("MapItemId");
                       cs.ToTable("MapItemTag");
                   });
        }
        #endregion

        #region IUnitOfWork
        public void Save()
        {
            lock (_operationLock)
            {
                // As used Get().AsNoTracking(), the native change tracking mech cannot be used
                // so 'this.ChangeTracker.DetectChanges()' is not used
                this.SaveChanges();
            }
        }
        #endregion

        #region IDbContext
        public T Get<T>(Guid id) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                IQueryable<T> dataSource = this.Set<T>();
                dataSource = CustomInclude(dataSource);
                return dataSource.AsNoTracking().First(x => x.Id == id);
            }
        }

        public IEnumerable<T> GetAll<T>() where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                IQueryable<T> dataSource = this.Set<T>();
                dataSource = CustomInclude(dataSource);
                return dataSource.AsNoTracking().ToList();
            }
        }

        public IEnumerable<T> GetAll<T>(Func<T, bool> filterPredicate) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                IQueryable<T> dataSource = this.Set<T>();
                dataSource = CustomInclude(dataSource);
                return dataSource.AsNoTracking().Where(filterPredicate).ToList();
            }
        }

        public void Add<T>(T newItem) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                this.Set<T>().Add(newItem);
            }
        }

        public void Update<T>(T updatedState) where T : class, IPersistentModel
        {
            // TODO: Refactor ?
            // http://stackoverflow.com/questions/3635071/update-relationships-when-saving-changes-of-ef4-poco-objects
            lock (_operationLock)
            {
                var existingItem = CustomInclude<T>(this.Set<T>()).First(x => x.Id == updatedState.Id);
                // Update own properties
                this.Entry(existingItem).CurrentValues.SetValues(updatedState);
                
                // Update relations
                var entities = (this as IObjectContextAdapter).ObjectContext;
               
                var navigationProperties = GetNavigationProperties(existingItem);
                foreach (var navProperty in navigationProperties)
                {
                    var data = navProperty.GetValue(updatedState);

                    if (data != null && data is IEnumerable)
                    {
                        var targetCollection = navProperty.GetValue(existingItem) as IList;
                        
                        // Remove existing refs
                        var newItems = (data as IEnumerable).Cast<IPersistentModel>().ToList();
                        var existingItems = (navProperty.GetValue(existingItem) as IEnumerable).Cast<IPersistentModel>().ToList();
                        foreach (var existingRef in existingItems)
                        {
                            if (!newItems.Any(x => x.Id == existingRef.Id))
                            {
                                entities.ObjectStateManager.ChangeRelationshipState(existingItem, existingRef, navProperty.Name, EntityState.Deleted);
                                targetCollection.Remove(existingRef);
                            }
                        }

                        // Add updated refs
                        foreach (var newRef in newItems)
                        {
                            if (!existingItems.Any(x => x.Id == newRef.Id))
                            {
                                var trackedItem = GetTrackedItem(newRef.GetType(), newRef.Id);
                                targetCollection.Add(trackedItem);
                                entities.ObjectStateManager.ChangeRelationshipState(existingItem, trackedItem, navProperty.Name, EntityState.Added);
                            }
                            
                        }
                    }
                    else if (data != null)
                    {
                        var existingRef = navProperty.GetValue(existingItem) as IPersistentModel;
                        if ((data as IPersistentModel).Id != existingRef.Id)
                        {
                            var trackedItem = GetTrackedItem(data.GetType(), (data as IPersistentModel).Id);

                            entities.ObjectStateManager.ChangeRelationshipState(existingItem, navProperty.GetValue(existingItem), navProperty.Name, EntityState.Deleted);
                            navProperty.SetValue(existingItem, trackedItem);
                            entities.ObjectStateManager.ChangeRelationshipState(existingItem, trackedItem, navProperty.Name, EntityState.Added);
                        }
                    }
                    else
                    {
                        entities.ObjectStateManager.ChangeRelationshipState(existingItem, navProperty.GetValue(existingItem), navProperty.Name, EntityState.Deleted);
                        navProperty.SetValue(existingItem, null);
                    }
                }

                this.Entry(existingItem).State = EntityState.Modified;
            }
        }

        protected virtual object GetTrackedItem(Type entityType, Guid id)
        {
            return this.Set(entityType).Find(id);
        }

        public void Delete<T>(Guid id) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                var entity = this.Set<T>().First(x => x.Id == id);
                this.Set<T>().Remove(entity);
            }
        }

        public List<System.Reflection.PropertyInfo> GetNavigationProperties<T>(T entity) where T : class, IPersistentModel
        {
            //http://stackoverflow.com/questions/17886725/ef5-how-to-get-list-of-navigation-properties-for-a-domain-object

            List<System.Reflection.PropertyInfo> properties = new List<System.Reflection.PropertyInfo>();
            Type entityType = entity.GetType();
            
            var entitySetElementType = ((System.Data.Entity.Infrastructure.IObjectContextAdapter)this).ObjectContext.CreateObjectSet<T>().EntitySet.ElementType;
            foreach (var navigationProperty in entitySetElementType.NavigationProperties)
            {
                properties.Add(entityType.GetProperty(navigationProperty.Name));
            }

            return properties;
        }

        protected virtual IQueryable<T> CustomInclude<T>(IQueryable<T> dataSource) where T : class, IPersistentModel
        {
            if (typeof(T) == typeof(MapItem))
                return (dataSource as DbSet<MapItem>)
                    .Include(i => i.Tags)
                    .Include(i => i.Company)
                    .Include(i => i.Author)
                    as IQueryable<T>;

            if (typeof(T) == typeof(Company))
                return (dataSource as DbSet<Company>)
                    .Include(i => i.Users)
                    .Include(i => i.Administrator) 
                    as IQueryable<T>;

            if (typeof(T) == typeof(MapItemComment))
                return (dataSource as DbSet<MapItemComment>)
                    .Include(i => i.Author)
                    .Include(i => i.MapItem)
                    as IQueryable<T>;

            if (typeof(T) == typeof(Tag))
                return (dataSource as DbSet<Tag>)
                    .Include(i => i.Company)
                    .Include(i => i.MapItems)
                    as IQueryable<T>;

            if (typeof(T) == typeof(User))
                return (dataSource as DbSet<User>)
                    .Include(i => i.Companies)
                    as IQueryable<T>;

            if (typeof(T) == typeof(Attachment))
                return (dataSource as DbSet<Attachment>)
                    .Include(i => i.Author)
                    .Include(i => i.MapItem)
                    as IQueryable<T>;

            return dataSource;
        }

        #endregion
    }
}
