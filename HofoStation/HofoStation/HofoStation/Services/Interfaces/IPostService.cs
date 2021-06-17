using HofoStation.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HofoStation.Services
{
    public interface IPostService
    {
        Task<bool> CreatePost(Post _post);
        Task<IEnumerable<Post>> GetAllPostGeo(string lat, string lng);
        Task<IEnumerable<Post>> GetAllPost();
        Task<string> uploadToBlobAsync(Stream stream);
        Task<Post> GetPostDetail(string id);
    }
}