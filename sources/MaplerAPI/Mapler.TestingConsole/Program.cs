using DataPersistance.Facade;
using DataPersistence.Facade;
using Mapler.DataAccess.RepositoryDataFiltering;
using Mapler.DataAccess.RepositoryDataFiltering.Proxies;
using Mapler.DataPersistance.Models;
using Mapler.DataPersistence.EntityFramework.EFContext;
using Mapler.DataPersistence.MockData;
using Mapler.Security;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Mapler.TestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new MaplerContext("MaplerDB");
            
            IPersistentRepository<MapItem> _mapItemRepo = new PersistentRepository<MapItem>(dbContext);
            IPersistentRepository<User> _userRepo = new PersistentRepository<User>(dbContext);
            IPersistentRepository<Company> _copmanyRepo = new PersistentRepository<Company>(dbContext);

            Authenticate("j_doe", _userRepo, _copmanyRepo, true);

            IRepoBusinessProxy<MapItem> _mapItemProxy = new MapItemRepoProxy(_mapItemRepo);
            (_mapItemProxy as RepoProxyBase<MapItem>).UserRepository = _userRepo;
            (_mapItemProxy as RepoProxyBase<MapItem>).CompanyRepository = _copmanyRepo;

            //_mapItemProxy.Delete(Guid.Parse("68BEA510-D9FA-4256-9E56-4B55580D23ED"));
            var item = _mapItemProxy.GetAll().First(x => x.Name == "TEST_NEW");

            //_mapItemProxy.Update(item);
            //_mapItemProxy.Delete(item.Id);
            (dbContext as IDbContext).Delete<MapItem>(item.Id);

            dbContext.SaveChanges();
            dbContext.Dispose();
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
