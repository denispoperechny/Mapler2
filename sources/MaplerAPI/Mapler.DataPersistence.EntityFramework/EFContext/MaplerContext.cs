using DataPersistance.Facade;
using DataPersistence.Facade;
using DataPersistence.Facade.Data;
using Mapler.DataPersistance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
            lock (_operationLock)
            {
                //this.Set<T>().(updatedState);
                var existingItem = CustomInclude<T>(this.Set<T>()).First(x => x.Id == updatedState.Id);
                this.Entry(existingItem).CurrentValues.SetValues(updatedState);
                this.Entry(existingItem).State = EntityState.Modified;
                UpdateReferncesState(existingItem);
            }
        }

        public void Delete<T>(Guid id) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                var entity = this.Set<T>().First(x => x.Id == id);
                this.Set<T>().Remove(entity);
            }
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

            return dataSource;
        }

        #endregion

        #region Custom change tracking

        /// <summary>
        /// Update the nested references state while the AutoChangeTracking is disabled.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="entity"></param>
        protected virtual void UpdateReferncesState<T>(T entity) where T : class, IPersistentModel
        {
            // Many-to-Many changes tracking
            // http://stackoverflow.com/questions/7490509/entity-framework-4-1-many-to-many-relationships-change-tracking
            // var objectContext = ((IObjectContextAdapter)this).ObjectContext;
            // objectContext.ObjectStateManager.ChangeRelationshipState(company, newAdmin, "Administrator", EntityState.Added);

            this.ChangeTracker.DetectChanges();
        }

        #endregion
    }
}
