using NewEnvisioBackend.Data.DTOs.UserDTOs;
using System.Threading.Tasks;

namespace NewEnvisioBackend.BusinessLogic
{
    public interface IUserService
    {
        Task<bool> DeleteUser(string userId);
        Task<UserResponseDTO> GetUser(string userId);
        // Task<bool> UpdatePassword(string userId, UpdatePasswordRequest updatePassword);
        
    }
}