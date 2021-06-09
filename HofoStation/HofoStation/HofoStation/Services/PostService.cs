using Azure.Storage.Blobs;
using HofoStation.Models;
using HofoStation.Responses;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace HofoStation.Services
{
    public static class PostService
    {
        static string BaseUrl = "https://hofostation.azurewebsites.net";
        static HttpClient client;

        static string blobCnnStr = "DefaultEndpointsProtocol=https;AccountName=hofostation;AccountKey=f0VNFGfFRCyJNvN8LhmZoV/MJjCj0WVB1/kVH85MMSsX7dINdq5ipuqkYqkAph8WxO+m3xXeCoJzOTVLweevjQ==;EndpointSuffix=core.windows.net";

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

        public static async Task<bool> CreatePost(Post _post, Stream imageStream)
        {
            string imagePath = await uploadToBlobAsync(imageStream);
            _post.post_image_url = imagePath;

            var temp = JsonConvert.SerializeObject(_post);
            var request = new StringContent(temp, Encoding.UTF8, "application/json");
            var response = await client.PostAsync("post/post_create", request);
            var json = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<OperationResponse>(json);

            return result.success;
        }

        public static async Task<string> uploadToBlobAsync(Stream stream)
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

    }
}
