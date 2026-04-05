using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public interface ITourLogService
    {
        Task<IEnumerable<TourLogResponseDto>> GetAllAsync();
        Task<IEnumerable<TourLogResponseDto>> GetByTourIdAsync(int tourId);
        Task<IEnumerable<TourLogResponseDto>> GetByUserIdAsync(int userId);
        Task<TourLogResponseDto?> GetByIdAsync(int id);
        Task<TourLogResponseDto> CreateAsync(int userId, TourLogCreateDto dto);
        Task<TourLogResponseDto?> UpdateAsync(int id, TourLogUpdateDto dto);
        Task<bool> DeleteAsync(int id);
    }
}
