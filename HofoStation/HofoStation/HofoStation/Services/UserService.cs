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
                user_password = Hashed(password)
            };

            var json = JsonConvert.SerializeObject(_user);
            var request = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("user/user_login", request);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserResponse>(content);

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

        private static string Hashed(string val)
        {
            byte[] salt = new byte[128 / 8];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }

            string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: val,
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA1,
                iterationCount: 10000,
                numBytesRequested: 256 / 8));

            return hashed;
        }
    }
}
