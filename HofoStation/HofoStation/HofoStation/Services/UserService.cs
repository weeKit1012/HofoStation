using HofoStation.Models;
using HofoStation.Responses;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HofoStation.Services
{
    public static class UserService
    {
        static string BaseUrl = "";
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
                user_password = password
            };

            var json = JsonConvert.SerializeObject(_user);
            var request = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("user/user_login", request);
            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<UserResponse>(content);

            if (result.success)
                return result.user;
            else
                _user = null;
                return _user;
        }

    }
}
