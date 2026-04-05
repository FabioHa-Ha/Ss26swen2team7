using Microsoft.EntityFrameworkCore;
using tourPlannerBackend.Data;
using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public class TourLogRepository : ITourLogRepository
    {
        private readonly ApplicationDbContext _context;

        public TourLogRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TourLog>> GetAllAsync()
        {
            return await _context.TourLogs
                .Include(l => l.Tour)
                .Include(l => l.User)
                .Include(l => l.Difficulty)
                .ToListAsync();
        }

        public async Task<IEnumerable<TourLog>> GetByTourIdAsync(int tourId)
        {
            return await _context.TourLogs
                .Include(l => l.Tour)
                .Include(l => l.User)
                .Include(l => l.Difficulty)
                .Where(l => l.Tour.Id == tourId)
                .ToListAsync();
        }

        public async Task<IEnumerable<TourLog>> GetByUserIdAsync(int userId)
        {
            return await _context.TourLogs
                .Include(l => l.Tour)
                .Include(l => l.User)
                .Include(l => l.Difficulty)
                .Where(l => l.User.Id == userId)
                .ToListAsync();
        }

        public async Task<TourLog?> GetByIdAsync(int id)
        {
            return await _context.TourLogs
                .Include(l => l.Tour)
                .Include(l => l.User)
                .Include(l => l.Difficulty)
                .FirstOrDefaultAsync(l => l.Id == id);
        }

        public async Task<TourLog> CreateAsync(TourLog tourLog)
        {
            _context.TourLogs.Add(tourLog);
            await _context.SaveChangesAsync();
            return tourLog;
        }

        public async Task<TourLog?> UpdateAsync(TourLog tourLog)
        {
            _context.TourLogs.Update(tourLog);
            await _context.SaveChangesAsync();
            return tourLog;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var log = await _context.TourLogs.FindAsync(id);
            if (log == null) return false;
            _context.TourLogs.Remove(log);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
