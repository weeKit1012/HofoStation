using Azure.Storage.Blobs;
using HofoStation.Models;
using HofoStation.Responses;
using HofoStation.Services;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

[assembly: Dependency(typeof(PostService))]
namespace HofoStation.Services
{
    public class PostService : IPostService
    {
        string BaseUrl = "https://hofostation.azurewebsites.net";
        HttpClient client;

        string blobCnnStr = "DefaultEndpointsProtocol=https;AccountName=hofostation;AccountKey=f0VNFGfFRCyJNvN8LhmZoV/MJjCj0WVB1/kVH85MMSsX7dINdq5ipuqkYqkAph8WxO+m3xXeCoJzOTVLweevjQ==;EndpointSuffix=core.windows.net";

        void Init()
        {
            client = new HttpClient
            {
                BaseAddress = new Uri(BaseUrl)
            };
        }

        public async Task<IEnumerable<Post>> GetAllPostGeo(string lat, string lng)
        {
            Init();
            var json = await client.GetStringAsync($"post/post_get_all_geo/?latitude={lat}&longitude={lng}");
            var results = JsonConvert.DeserializeObject<PostResponse>(json);
            return results.posts;
        }

        public async Task<IEnumerable<Post>> GetAllPost()
        {
            Init();
            var json = await client.GetStringAsync($"post/post_get_all");
            var results = JsonConvert.DeserializeObject<PostResponse>(json);
            return results.posts;
        }

        public async Task<bool> CreatePost(Post _post)
        {
            Init();

            var temp = JsonConvert.SerializeObject(_post);
            var request = new StringContent(temp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("post/post_create", request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse>(json);

            return result.success;
        }

        public async Task<string> uploadToBlobAsync(Stream stream)
        {
            // Create a BlobServiceClient object which will be used to create a container client
            BlobServiceClient blobServiceClient = new BlobServiceClient(blobCnnStr);

            //Create a unique name for the container
            string containerName = "hofogallery";

            // Create the container and return a container client object
            //BlobContainerClient containerClient = await blobServiceClient.CreateBlobContainerAsync(containerName);
            BlobContainerClient containerClient = blobServiceClient.GetBlobContainerClient(containerName);

            // Get a reference to a blob
            var blobName = Guid.NewGuid().ToString();
            BlobClient blobClient = containerClient.GetBlobClient($"{blobName}.png");

            //Uploading

            await blobClient.UploadAsync(stream, true);

            //Get 
            //var url = blobClient.Uri.OriginalString;

            //SAS
            DateTime newTime = new DateTime(2025, 01, 01);
            DateTimeOffset expireDate = new DateTimeOffset(newTime,
                                     TimeZoneInfo.Local.GetUtcOffset(newTime));
            var url = blobClient.GenerateSasUri(Azure.Storage.Sas.BlobSasPermissions.Read, expireDate);

            return url.AbsoluteUri;
        }

        public async Task<Post> GetPostDetail(string id)
        {
            Init();
            var json = await client.GetStringAsync($"post/post_get/?requestID={id}");
            var results = JsonConvert.DeserializeObject<PostResponse>(json);
            var temp = new Post();
            foreach (var item in results.posts)
            {
                temp = item;
            }

            return temp;
        }

        public async Task<IEnumerable<Post>> GetUserPost(string id)
        {
            Init();
            var json = await client.GetStringAsync($"post/post_get_user/?requestID={id}");
            var results = JsonConvert.DeserializeObject<PostResponse>(json);

            if (results.success)
            {
                return results.posts;
            }
            else
            {
                return new List<Post>();
            }
        }

        public async Task<bool> UpdatePost(Post _post)
        {
            Init();

            var temp = JsonConvert.SerializeObject(_post);
            var request = new StringContent(temp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("post/post_update", request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse>(json);

            return result.success;
        }

        public async Task<bool> DeletePost(Post _post)
        {
            Init();

            var temp = JsonConvert.SerializeObject(_post);
            var request = new StringContent(temp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("post/post_delete", request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse>(json);

            return result.success;
        }
    }
}
