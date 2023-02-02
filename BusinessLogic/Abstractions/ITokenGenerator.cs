using NewEnvisioBackend.Models;
using System.Threading.Tasks;

namespace NewEnvisioBackend.BusinessLogic
{
    public interface ITokenGenerator
    {
        Task<string> GenerateToken(AppUser user);
    }
}