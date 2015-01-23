using DataPersistance.Facade;
using Mapler.DataPersistance.Models;
using Mapler.DataPersistence.EntityFramework.EFContext;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mapler.TestingConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            var dbContext = new MaplerContext("MaplerTestDB");

            dbContext.SaveChanges();
            //var users = dbContext.Users.ToList();

            //IPersistentRepository<User> _userRepo = new PersistentRepository<User>(dbContext);

            //object res = null;

            //Task.Factory.StartNew(() => _userRepo.GetAll(x => x.Login.Equals("", StringComparison.InvariantCultureIgnoreCase) && x.IsActive)
            //            .FirstOrDefault());

            //var user =
            //        _userRepo.GetAll(x => x.Login.Equals("", StringComparison.InvariantCultureIgnoreCase) && x.IsActive)
            //            .FirstOrDefault();


        }
    }
}
