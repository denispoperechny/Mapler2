﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade.MockData;
using Mapler.DataPersistance.Models;

namespace Mapler.DataPersistence.MockData
{
    public static class MockDataFactory
    {

        public static MockDataContext GetSomeMockData()
        {
            var result = new MockDataContext();
            
            result.AddDataCollection(Companies);
            result.AddDataCollection(Users);
            result.AddDataCollection(Tags);
            result.AddDataCollection(MapItems);
            result.AddDataCollection(Locations);
            result.AddDataCollection(UserPasswords);
            result.AddDataCollection(Attachments);

            return result;
        }

        private static List<Company> _companies;
        public static List<Company> Companies
        {
            get { return _companies ?? (_companies = GenerateCompanies().ToList()); }
        }

        private static List<User> _users;
        public static List<User> Users
        {
            get { return _users ?? (_users = GenerateUsers().ToList()); }
        }

        //private static List<Attachment> _attachments;
        public static List<Attachment> Attachments
        {
            get { return new List<Attachment>();}
        }

        private static List<Tag> _tags;
        public static List<Tag> Tags
        {
            get { return _tags ?? (_tags = GenerateTags().ToList()); }
        }

        private static List<MapItem> _mapItems;
        public static List<MapItem> MapItems
        {
            get { return _mapItems ?? (_mapItems = GenerateMapItems().ToList()); }
        }

        private static List<Location> _locations;
        public static List<Location> Locations
        {
            get { return _locations ?? (_locations = GenerateLocations().ToList()); }
        }

        private static List<UserPass> _userPasswords;
        public static List<UserPass> UserPasswords
        {
            get { return _userPasswords ?? (_userPasswords = GenerateUserPasswords().ToList()); }
        }

        private static List<MapItemComment> _mapItemComments;
        public static List<MapItemComment> MapItemComments
        {
            get { return _mapItemComments ?? (_mapItemComments = GenerateMapItemComments().ToList()); }
        }

        private static IEnumerable<MapItemComment> GenerateMapItemComments()
        {
            return Enumerable.Empty<MapItemComment>();
        }

        private static IEnumerable<Company> GenerateCompanies()
        {
            var users = Users;
            var defaultLocation = Locations.First(x => x.IsSystemDefault);

            yield return new Company
            {
                Id = Guid.Parse("092F35ED-E31F-417D-8DAD-8B8ACC1E04FA"),
                Administrator = users.First(),
                Created = DateTime.Parse("2008-05-01"),
                DefaultLocation = defaultLocation,
                Name = "Corporation One",
                Users = users,
                IsActive = true
            };

            yield return new Company
            {
                Id = Guid.Parse("5283DECB-CE3F-40A2-BBD4-B11F21DC7F13"),
                Administrator = users[1],
                Created = DateTime.Parse("2008-05-01"),
                DefaultLocation = defaultLocation,
                Name = "Corporation Two",
                Users = users.GetRange(1, 2),
                IsActive = true
            };
        }

        private static IEnumerable<Location> GenerateLocations()
        {
            yield return new Location
            {
                Id = Guid.Parse("A70970A4-D1F4-480A-9A90-267AA3A0797C"),
                Latitude = 12,
                Longitude = 23,
                Zoom = 8,
                IsSystemDefault = true
            };

            yield return new Location
            {
                Id = Guid.Parse("CA31282B-24DF-46E4-97D5-F71DD298EA3D"),
                Latitude = 45,
                Longitude = 56,
                Zoom = 6,
                IsSystemDefault = false
            };
        }

        private static IEnumerable<MapItem> GenerateMapItems()
        {
            var companies = Companies;

            yield return new MapItem
            {
                Id = Guid.Parse("68BEA510-D9FA-4256-9E56-4B55580D23ED"),
                Longitude = 34,
                Latitude = 45,
                Color = 123,
                Created = DateTime.Parse("2008-05-01"),
                Name = "Point #1",
                Tags = Tags,
                Company = companies[0],
                Author = companies[0].Users[1],
                IsActive = true
            };

            yield return new MapItem
            {
                Id = Guid.Parse("45DFC15B-CDC4-4BC8-AABC-E83B9973BD0A"),
                Longitude = 67,
                Latitude = 34,
                Color = 13,
                Created = DateTime.Parse("2008-05-01"),
                Name = "Point #2",
                Tags = Tags,
                Company = companies[0],
                Author = companies[0].Users[1],
                IsActive = true
            };

            yield return new MapItem
            {
                Id = Guid.Parse("0CAC77F4-CF2C-4CB7-941A-A3E780B9DFBF"),
                Longitude = 4,
                Latitude = 5,
                Color = 3,
                Created = DateTime.Parse("2008-05-01"),
                Name = "Point #3",
                Tags = Tags,
                Company = companies[1],
                Author = companies[1].Users[1],
                IsActive = true
            };

            yield return new MapItem
            {
                Id = Guid.Parse("B5689F0D-E5DE-4099-974A-A2DC5D639A2C"),
                Longitude = 16,
                Latitude = 43,
                Color = 1,
                Created = DateTime.Parse("2008-05-01"),
                Name = "Point #4",
                Tags = Tags,
                Company = companies[1],
                Author = companies[1].Users[1],
                IsActive = true
            };
        }

        private static IEnumerable<Tag> GenerateTags()
        {
            yield return new Tag
            {
                Id = Guid.Parse("C60576F2-574E-4682-BF4A-157263810AD9"),
                Company = Companies.First(),
                Description = "Favorites items",
                Name = "Favorites",
                IsActive = true
            };
        }

        private static IEnumerable<User> GenerateUsers()
        {
            Guid id1 = Guid.Parse("B02E3C74-1568-44C2-B154-3F2B79355E11");
            yield return new User
            {
                Id = id1,
                Login = "j_smith",
                FirstName = "John",
                LastName = "Smith",
                Created = DateTime.Parse("2008-05-01"),
                IsActive = true,
                IsSuperUser = false,
                //Companies = Companies.Where(x => x.Users.Any(a => a.Id == id1)).ToList()
            };

            Guid id2 = Guid.Parse("20E825B3-5642-4D9E-A06C-0B854AD19E17");
            yield return new User
            {
                Id = id2,
                Login = "j_doe",
                FirstName = "John",
                LastName = "Doe",
                Created = DateTime.Parse("2008-05-01"),
                IsActive = true,
                IsSuperUser = false,
                //Companies = Companies.Where(x => x.Users.Any(a => a.Id == id2)).ToList()
            };

            Guid id3 = Guid.Parse("9795BFF5-C2DF-48EF-8C2E-C059D5A6E774");
            yield return new User
            {
                Id = id3,
                Login = "e_presley",
                FirstName = "Elvis",
                LastName = "Presley",
                Created = DateTime.Parse("2008-05-01"),
                IsActive = true,
                IsSuperUser = false,
                //Companies = Companies.Where(x => x.Users.Any(a => a.Id == id3)).ToList()
            };

            Guid id4 = Guid.Parse("9795BFF5-C2DF-48EF-8C2E-C059D5A6E785");
            yield return new User
            {
                Id = id4,
                Login = "super",
                FirstName = "super",
                LastName = "super",
                Created = DateTime.Parse("2008-05-01"),
                IsActive = true,
                IsSuperUser = true,
                //Companies = Companies.Where(x => x.Users.Any(a => a.Id == id4)).ToList()
            };
        }

        private static IEnumerable<UserPass> GenerateUserPasswords()
        {
            return Users.Select(x => new UserPass()
            {
                Id = Guid.NewGuid(), 
                UserId = x.Id, 
                PassHash = "pass", 
                IsActive = true
            });
        }
    }
}
