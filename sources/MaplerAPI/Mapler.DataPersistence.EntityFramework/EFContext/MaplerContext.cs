using DataPersistance.Facade;
using DataPersistence.Facade;
using DataPersistence.Facade.Data;
using Mapler.DataPersistance.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
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
        }
        #endregion

        #region IUnitOfWork
        public void Save()
        {
            lock (_operationLock)
            {
                // this.Save() ?
                this.SaveChanges();
            }
        }
        #endregion

        #region IDbContext
        public T Get<T>(Guid id) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                return GetDbSet<T>().First(x => x.Id == id);
            }
        }

        public IEnumerable<T> GetAll<T>() where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                return GetDbSet<T>();
            }
        }

        public IEnumerable<T> GetAll<T>(Func<T, bool> filterPredicate) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                return GetDbSet<T>().Where(filterPredicate);
            }
        }

        public void Add<T>(T newItem) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                GetDbSet<T>().Add(newItem);
            }
        }

        public void Update<T>(T updatedState) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                GetDbSet<T>().Add(updatedState);
            }
        }

        public void Delete<T>(Guid id) where T : class, IPersistentModel
        {
            lock (_operationLock)
            {
                var entity = GetDbSet<T>().First(x => x.Id == id);
                GetDbSet<T>().Remove(entity);
            }
        }

        protected virtual DbSet<T> GetDbSet<T>() where T : class, IPersistentModel
        {
            var targetType = typeof(T);
            var propertyInfo = typeof(MaplerContext).GetProperties().First(x => 
                x.PropertyType.GenericTypeArguments[0] == targetType 
                && x.PropertyType.Name == "DbSet`1");

            var result = propertyInfo.GetValue(this);
            return (DbSet<T>)result;
        }

        #endregion
    }
}
