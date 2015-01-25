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
            var dbContext = new MaplerContext("MaplerDB");

            //dbContext.ChangeTracker.Entries().First().Collection().

            var miId = Guid.Parse("5283DECB-CE3F-40A2-BBD4-B11F21DC7F13");
            var tID = Guid.Parse("B02E3C74-1568-44C2-B154-3F2B79355E11");
            var company = dbContext.Companies.Where(x => x.Id == miId).First();
            var newAdmin = dbContext.Users.Where(x => x.Id == tID).First();

            var objectContext = ((IObjectContextAdapter)dbContext).ObjectContext;
            //var relations = objectContext.ObjectStateManager.GetObjectStateEntries(EntityState.Unchanged)
            //                .Where(e => e.IsRelationship);


            company.Administrator = newAdmin;
            company.Name = "Company 111";
            (dbContext as IDbContext).Update<Company>(company);


            //objectContext.ObjectStateManager.ChangeRelationshipState(company, newAdmin, "Administrator", EntityState.Added);

            dbContext.SaveChanges();
            dbContext.Dispose();
        }

        //  mapItem.Tags.Add(newTag);
        //    objectContext.ObjectStateManager.ChangeRelationshipState(mapItem, newTag, "Tags", EntityState.Added);

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
