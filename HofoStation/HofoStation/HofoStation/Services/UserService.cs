using HofoStation.Models;
using HofoStation.Responses;
using HofoStation.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(UserService))]
namespace HofoStation.Services
{
    public class UserService : IUserService
    {
        string BaseUrl = "https://hofostation.azurewebsites.net";
        HttpClient client;

        void Init()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<User> LoginUser(string email, string password)
        {
            Init();

            User _user = new User
            {
                user_email = email,
                user_password = hashed(password)
            };

            var temp = JsonConvert.SerializeObject(_user);
            var request = new StringContent(temp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("user/user_login", request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserResponse>(json);

            if (result.success)
                if (string.IsNullOrEmpty(result.user.user_email))
                {
                    _user = null;
                    return _user;
                }
                else
                {
                    return result.user;
                }
            else
                _user = null;
            return _user;
        }

        public async Task<bool> RegisterUser(User _user)
        {
            Init();

            _user.user_password = hashed(_user.user_password);

            var temp = JsonConvert.SerializeObject(_user);
            var request = new StringContent(temp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("user/user_register", request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse>(json);

            if (result.success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<bool> UpdateUser(User _user)
        {
            Init();

            _user.user_password = hashed(_user.user_password);

            var temp = JsonConvert.SerializeObject(_user);
            var request = new StringContent(temp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("user/user_update", request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse>(json);

            if (result.success)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public async Task<User> GetUser(string id)
        {
            Init();
            var json = await client.GetStringAsync($"user/user_get/?requestID={id}");
            var results = JsonConvert.DeserializeObject<UserResponse>(json);

            if (results.success)
            {
                return results.user;
            }
            else
            {
                User _user = null;
                return _user;
            }
        }

        public async Task<IEnumerable<User>> GetAllUser()
        {
            Init();
            var json = await client.GetStringAsync($"user/user_get_all");
            var results = JsonConvert.DeserializeObject<IEnumerable<User>>(json);
            return results;
        }

        public string hashed(string raw)
        {
            Init();

            using (SHA256 sha256 = SHA256.Create())
            {
                byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(raw));

                StringBuilder builder = new StringBuilder();
                for (int i = 0; i < bytes.Length; i++)
                {
                    builder.Append(bytes[i].ToString("x2"));
                }

                return builder.ToString();
            }
        }
    }
}
