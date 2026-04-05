using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public interface ITourRepository
    {
        Task<IEnumerable<Tour>> GetAllAsync();
        Task<IEnumerable<Tour>> GetByUserIdAsync(int userId);
        Task<Tour?> GetByIdAsync(int id);
        Task<Tour> CreateAsync(Tour tour);
        Task<Tour?> UpdateAsync(Tour tour);
        Task<bool> DeleteAsync(int id);
    }
}
