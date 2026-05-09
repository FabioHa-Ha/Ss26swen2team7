using Microsoft.EntityFrameworkCore;
using tourPlannerBackend.Data;
using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public class TourRepository : ITourRepository
    {
        private readonly ApplicationDbContext _context;

        public TourRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Tour>> GetAllAsync()
        {
            return await _context.Tours
                .Include(t => t.User)
                .Include(t => t.TransportType)
                .ToListAsync();
        }

        public async Task<IEnumerable<Tour>> GetByUserIdAsync(int userId)
        {
            return await _context.Tours
                .Include(t => t.User)
                .Include(t => t.TransportType)
                .Where(t => t.User.Id == userId)
                .ToListAsync();
        }

        public async Task<Tour?> GetByIdAsync(int id)
        {
            return await _context.Tours
                .Include(t => t.User)
                .Include(t => t.TransportType)
                .Include(t => t.Images)
                .FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task<Tour> CreateAsync(Tour tour)
        {
            _context.Tours.Add(tour);
            await _context.SaveChangesAsync();
            return tour;
        }

        public async Task<Tour?> UpdateAsync(Tour tour)
        {
            _context.Tours.Update(tour);
            await _context.SaveChangesAsync();
            return tour;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tour = await _context.Tours.FindAsync(id);
            if (tour == null) return false;
            _context.Tours.Remove(tour);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
