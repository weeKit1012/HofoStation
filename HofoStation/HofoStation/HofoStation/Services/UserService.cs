using HofoStation.Models;
using HofoStation.Responses;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace HofoStation.Services
{
    public static class UserService
    {
        static string BaseUrl = "https://hofostation.azurewebsites.net";
        static HttpClient client;

        static UserService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public static async Task<User> LoginUser(string email, string password)
        {
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

        public static async Task<bool> RegisterUser(User _user)
        {
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

        private static string hashed(string raw)
        {
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
