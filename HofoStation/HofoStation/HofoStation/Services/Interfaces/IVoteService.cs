using HofoStation.Models;
using System.Threading.Tasks;

namespace HofoStation.Services
{
    public interface IVoteService
    {
        Task<bool> CreateVote(Vote _vote);
        Task<int> GetVoteCount(string id);
        Task<int> CheckVoteExist(Vote _vote);
    }
}