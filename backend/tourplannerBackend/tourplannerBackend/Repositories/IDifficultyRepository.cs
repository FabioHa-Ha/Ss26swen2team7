using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public interface IDifficultyRepository
    {
        Task<Difficulty?> GetByIdAsync(int id);
        Task<IEnumerable<Difficulty>> GetAllAsync();
    }
}
