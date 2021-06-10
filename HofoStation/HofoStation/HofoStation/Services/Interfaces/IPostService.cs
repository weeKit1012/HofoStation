using HofoStation.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HofoStation.Services
{
    public interface IPostService
    {
        Task<bool> CreatePost(Post _post, Stream imageStream);
        Task<IEnumerable<Post>> GetPost(string lat, string lng);
        Task<string> uploadToBlobAsync(Stream stream);
    }
}