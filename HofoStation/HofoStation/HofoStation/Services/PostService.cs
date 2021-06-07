using HofoStation.Models;
using HofoStation.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HofoStation.Services
{
    public static class PostService
    {
        static string BaseUrl = "";
        static HttpClient client;

        static PostService()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public static async Task<IEnumerable<Post>> GetPost(string lat, string lng)
        {
            var json = await client.GetStringAsync($"post/post_get_all_geo/?latitude={lat}&longitude={lng}");
            var results = JsonConvert.DeserializeObject<PostResponse>(json);
            return results.posts;
        }
    }
}
