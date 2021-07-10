/*
 Programmer Name    :   TEY WEE KIT
 Program Name       :   HofoStation (Front End, User Interface)
 */

using HofoStation.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace HofoStation.Services
{
    public interface IUserService
    {
        string hashed(string raw);
        Task<User> LoginUser(string email, string password);
        Task<bool> RegisterUser(User _user);
        Task<bool> UpdateUser(User _user);
        Task<User> GetUser(string id);
        Task<IEnumerable<User>> GetAllUser();
    }
}