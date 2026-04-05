using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public interface ITourLogRepository
    {
        Task<IEnumerable<TourLog>> GetAllAsync();
        Task<IEnumerable<TourLog>> GetByTourIdAsync(int tourId);
        Task<IEnumerable<TourLog>> GetByUserIdAsync(int userId);
        Task<TourLog?> GetByIdAsync(int id);
        Task<TourLog> CreateAsync(TourLog tourLog);
        Task<TourLog?> UpdateAsync(TourLog tourLog);
        Task<bool> DeleteAsync(int id);
    }
}
