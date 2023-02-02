using NewEnvisioBackend.Data.DTOs.UserDTOs;
using System.Threading.Tasks;

namespace NewEnvisioBackend.BusinessLogic
{
    public interface IAuthentication
    {
        Task<UserResponseDTO> Login(UserRequest userRequest);
        Task<UserResponseDTO> Register(RegistrationRequest registrationRequest);
        Task<string> ForgotPasswordAsync(ForgotPassword forgotPassword);
        Task<string> ResetPasswordAsync(ResetPasswordRequest resetPassword);
    }
}