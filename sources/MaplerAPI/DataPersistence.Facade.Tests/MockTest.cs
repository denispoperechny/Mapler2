using System;
using System.Linq;
using DataPersistance.Facade;
using DataPersistance.Facade.MockData;
using Mapler.DataPersistance.Models;
using DataPersistence.Facade.Data;
using Mapler.DataPersistence.MockData;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace DataPersistence.Facade.Tests
{
    [TestClass]
    public class MockTest
    {
        [TestMethod]
        public void TestGettingById()
        {
            IPersistentRepository<MapItem> mapItemsRepo = GetMockRepo<MapItem>();

            IPersistentModel testEntity = mapItemsRepo.GetAll().First();
            Guid testId = testEntity.Id;

            Assert.IsTrue(mapItemsRepo.Get(testId) != null, "Cannot find by IPersistentModel.Id");
        }

        [TestMethod]
        public void TestDefaultLocation()
        {
            IPersistentRepository<Location> locationsRepo = GetMockRepo<Location>();

            Assert.IsTrue(locationsRepo.GetAll().Any(x => x.IsSystemDefault), "No system default location");
        }

        [TestMethod]
        public void TestRecordsCount()
        {
            IPersistentRepository<Company> companiesRepo = GetMockRepo<Company>();

            Assert.IsTrue(companiesRepo.GetAll().Any(), "No companies added");
            Assert.IsTrue(companiesRepo.GetAll().All(x => x.Administrator != null), "No company administrator added");
            Assert.IsTrue(companiesRepo.GetAll().All(x => x.DefaultLocation != null), "No default location added");
            Assert.IsTrue(companiesRepo.GetAll().All(x => x.Users.Any()), "No users added");

        }

        [TestMethod]
        public void TestFiltering()
        {
            var testName = "John";
            IPersistentRepository<User> userRepo = GetMockRepo<User>();

            var filteredUsers = userRepo.GetAll(x => x.FirstName == testName).ToList();
            Assert.IsTrue(filteredUsers.Any(), string.Format("Any user with name {0} found", testName));
        }

        [TestMethod]
        public void TestAdding()
        {
            IPersistentRepository<Tag> tagsRepo = GetMockRepo<Tag>();
            IPersistentRepository<Company> companyRepo = GetMockRepo<Company>();

            Company company = companyRepo.GetAll().First();

            var initialRecordsCount = tagsRepo.GetAll(x => x.Company.Id == company.Id).Count();
            tagsRepo.Add(new Tag
            {
                Id = Guid.NewGuid(),
                Company = company,
                Description = "New One",
                Name = "New"
            });

            var newRecordsCount = tagsRepo.GetAll(x => x.Company.Id == company.Id).Count();

            Assert.AreNotEqual(initialRecordsCount, newRecordsCount, "New entry has not been added");
        }

        [TestMethod]
        public void TestDeletion()
        {
            IPersistentRepository<User> userRepo = GetMockRepo<User>();
            var user = userRepo.GetAll().First();
            userRepo.Delete(user.Id);

            var remainingEntries = userRepo.GetAll(x => x.Id == user.Id).ToList();

            Assert.AreEqual(0, remainingEntries.Count(), "Entry has not been deleted");
        }

        [TestMethod]
        public void TestUpdate()
        {
            IPersistentRepository<Tag> tagsRepo = GetMockRepo<Tag>();
            var tag = tagsRepo.GetAll().First();
            var initialName = tag.Name;

            tag.Name = "Changed";
            tagsRepo.Update(tag);

            Assert.AreNotEqual(initialName, tagsRepo.Get(tag.Id).Name, "Tag name has not been updated");
        }

        public IPersistentRepository<T> GetMockRepo<T>() where T : class, IPersistentModel
        {
            return new PersistentRepository<T>(GetMockDataContext());
        }

        private MockDataContext GetMockDataContext()
        {
            return MockDataFactory.GetSomeMockData();
        }

    }
}
