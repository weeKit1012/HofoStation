using HofoStation.Models;
using HofoStation.Responses;
using HofoStation.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(VoteService))]
namespace HofoStation.Services
{
    public class VoteService : IVoteService
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

        public async Task<int> GetVoteCount(string id)
        {
            Init();
            var json = await client.GetStringAsync($"vote/vote_get_count/?requestID={id}");
            var results = JsonConvert.DeserializeObject<int>(json);
            return results;
        }

        public async Task<bool> CreateVote(Vote _vote)
        {
            Init();

            var temp = JsonConvert.SerializeObject(_vote);
            var request = new StringContent(temp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("vote/vote_create", request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse>(json);

            return result.success;
        }

        public async Task<int> CheckVoteExist(Vote _vote)
        {
            Init();

            var temp = JsonConvert.SerializeObject(_vote);
            var request = new StringContent(temp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("vote/vote_check_exist", request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<int>(json);

            return result;
        }
    }
}
