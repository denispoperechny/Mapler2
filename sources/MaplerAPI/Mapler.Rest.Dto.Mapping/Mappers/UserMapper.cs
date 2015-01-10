using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataPersistance.Facade;
using Mapler.DataPersistance.Models;
using Mapler.Rest.Dto.Mapping.Interfaces;

namespace Mapler.Rest.Dto.Mapping.Mappers
{
    public class UserMapper : IDtoMapper<UserDto, User>
    {
        public UserDto Map(User item)
        {
            return new UserDto
            {
                Id = item.Id,
                Login = item.Login,
                Created = item.Created,
                FirstName = item.FirstName,
                LastName = item.LastName
            };
        }

        public User MapBack(UserDto item)
        {
            if (item == null) throw new ArgumentNullException("item");

            var result = new User
            {
                Id = item.Id == Guid.Empty ? Guid.NewGuid() : item.Id,
                Created = DateTime.Now,
                FirstName = item.FirstName,
                LastName = item.LastName,
                Login = item.Login
            };

            return result;
        }


        public void UpdateBack(UserDto dtoItem, User persistItem)
        {
            if (dtoItem == null) throw new ArgumentNullException("dtoItem");
            if (persistItem == null) throw new ArgumentNullException("persistItem");

            //persistItem.Id = dtoItem.Id;
            persistItem.FirstName = dtoItem.FirstName;
            persistItem.LastName = dtoItem.LastName;
            //persistItem.Created
            //persistItem.Login
        }
    }
}
