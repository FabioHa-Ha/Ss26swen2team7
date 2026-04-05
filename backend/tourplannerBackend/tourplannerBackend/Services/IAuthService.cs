using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public interface IAuthService
    {
        Task<AuthResponseDto> RegisterAsync(UserRegisterDto dto);
        Task<AuthResponseDto?> LoginAsync(UserLoginDto dto);
    }
}
