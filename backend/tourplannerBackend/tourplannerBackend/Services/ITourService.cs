using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public interface ITourService
    {
        Task<IEnumerable<TourResponseDto>> GetAllAsync();
        Task<IEnumerable<TourResponseDto>> GetByUserIdAsync(int userId);
        Task<TourResponseDto?> GetByIdAsync(int id);
        Task<TourResponseDto> CreateAsync(int userId, TourCreateDto dto);
        Task<TourResponseDto?> UpdateAsync(int id, TourUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
