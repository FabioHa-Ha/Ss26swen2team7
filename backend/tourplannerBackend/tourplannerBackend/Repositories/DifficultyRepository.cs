using Microsoft.EntityFrameworkCore;
using tourPlannerBackend.Data;
using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public class DifficultyRepository : IDifficultyRepository
    {
        private readonly ApplicationDbContext _context;

        public DifficultyRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Difficulty?> GetByIdAsync(int id)
        {
            return await _context.Difficulties.FindAsync(id);
        }

        public async Task<IEnumerable<Difficulty>> GetAllAsync()
        {
            return await _context.Difficulties.ToListAsync();
        }
    }
}
