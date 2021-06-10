using HofoStation.Models;
using System.Threading.Tasks;

namespace HofoStation.Services
{
    public interface IUserService
    {
        string hashed(string raw);
        Task<User> LoginUser(string email, string password);
        Task<bool> RegisterUser(User _user);
        Task<bool> UpdateUser(User _user);
    }
}