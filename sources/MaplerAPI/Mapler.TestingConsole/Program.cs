using DataPersistance.Facade;
using DataPersistence.Facade;
using Mapler.DataAccess.RepositoryDataFiltering;
using Mapler.DataAccess.RepositoryDataFiltering.Proxies;
using Mapler.DataPersistance.Models;
using Mapler.DataPersistence.EntityFramework.EFContext;
using Mapler.DataPersistence.MockData;
using Mapler.Security;
using System.Linq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Data.Entity.Infrastructure;

namespace Mapler.TestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Test2();
        }

        private static void Test2()
        {
            using (var dbContext = new MaplerContext("MaplerDB"))
            {

                IDbContext context = dbContext;

                var company1 = dbContext.Set(typeof(Company)).Find(Guid.Parse("5283DECB-CE3F-40A2-BBD4-B11F21DC7F13"));

                var tag = context.GetAll<Tag>().First();
                var newTag = new Tag()
                {
                    Id = tag.Id,
                    Company = tag.Company,
                    Description = tag.Description,
                    IsActive = tag.IsActive,
                    MapItems = tag.MapItems,
                    Name = tag.Name
                };

                var companyId = Guid.Parse("092F35ED-E31F-417D-8DAD-8B8ACC1E04FA");
                newTag.Name = "Test 11122";
                newTag.MapItems = new List<MapItem>(new[] { newTag.MapItems.First()});
                newTag.Company = dbContext.Companies.AsNoTracking().First(x => x.Id == companyId);


                context.Update<Tag>(newTag);

                (dbContext as IUnitOfWork).Save();
            }

        }

        private static void Test1()
        {
            using (var dbContext = new MaplerContext("MaplerDB"))
            {

                var company1 = dbContext.Set(typeof(Company)).Find(Guid.Parse("5283DECB-CE3F-40A2-BBD4-B11F21DC7F13"));

                var tag = dbContext.Tags.Include(i => i.Company).Include(i => i.MapItems).AsNoTracking().First();
                var newTag = new Tag()
                {
                    Id = tag.Id,
                    Company = tag.Company,
                    Description = tag.Description,
                    IsActive = tag.IsActive,
                    MapItems = tag.MapItems,
                    Name = tag.Name
                };

                var companyId = Guid.Parse("5283DECB-CE3F-40A2-BBD4-B11F21DC7F13");
                newTag.Name = "Test 111";
                newTag.Company = dbContext.Companies.AsNoTracking().First(x => x.Id == companyId);


                dbContext.Companies.Attach(newTag.Company);
                //dbContext.Entry(newTag.Company).State = EntityState.Unchanged;

                dbContext.Tags.Attach(newTag);
                dbContext.Entry(newTag).State = EntityState.Modified;


                (dbContext as IUnitOfWork).Save();
            }

        }

        static void FillDBMock()
        {
            var dbName = "MaplerTestDB";

            if (false)
            {
                dbName = "MaplerDB";
            }

            var dbContext = new MaplerContext(dbName);
            dbContext.Locations.AddRange(MockDataFactory.Locations);

            dbContext.SaveChanges();
            dbContext.Users.AddRange(MockDataFactory.Users);
            dbContext.SaveChanges();
            dbContext.Companies.AddRange(MockDataFactory.Companies);
            dbContext.SaveChanges();
            dbContext.Tags.AddRange(MockDataFactory.Tags);
            dbContext.SaveChanges();
            dbContext.MapItems.AddRange(MockDataFactory.MapItems);
            dbContext.SaveChanges();
            dbContext.MapItemComments.AddRange(MockDataFactory.MapItemComments);
            dbContext.SaveChanges();
            dbContext.Attachments.AddRange(MockDataFactory.Attachments);
            dbContext.SaveChanges();
            dbContext.UserPasswords.AddRange(MockDataFactory.UserPasswords);
            dbContext.SaveChanges();
        }

        private static void Authenticate(string userName, IPersistentRepository<User> userRepo, IPersistentRepository<Company> companyRepo, bool isSuper = false)
        {
            var user = userRepo.GetAll(x => x.Login == userName).First();
            var companies = companyRepo.GetAll(x => x.Administrator.Id == user.Id || x.Users.Any(a => a.Id == user.Id)).Select(s => s.Id).ToList();

            var identity = new MaplerIdentity(user.Id, user.Login);
            var principal = new MaplerPrincipal(identity, companies, null, isSuper);

            Thread.CurrentPrincipal = principal;
        }
    }
}
