using DataPersistance.Facade;
using DataPersistence.Facade;
using Mapler.DataPersistance.Models;
using Mapler.DataPersistence.EntityFramework.EFContext;
using Mapler.DataPersistence.MockData;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.TestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new MaplerContext("MaplerDB");

            var entry1 = (dbContext as IDbContext).GetAll<Company>().First();
            //var entry2 = dbContext.Companies.First();
            //var entry3 = dbContext.Companies.Include("Users") .First();
            //var entry4 = (dbContext.Companies as DbSet<Company>).Include(x => x.Users) .First();


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
    }
}
