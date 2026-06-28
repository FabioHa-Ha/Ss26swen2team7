using Microsoft.EntityFrameworkCore;
using tourplannerBackend.DTOs;
using tourplannerBackend.Model;
using tourPlannerBackend.Data;

namespace tourplannerBackend.Repositories
{
    public class ImageRepository : IImageRepository
    {
        private readonly ApplicationDbContext _context;

        public ImageRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<int> CreateImage(TourImage tourImage)
        {

            _context.TourImages.Add(tourImage);
            await _context.SaveChangesAsync();
            return tourImage.Id;
        }

        public async Task<TourImage?> GetImage(int id)
        {
            return await _context.TourImages.FindAsync(id);
        }

        public async Task<IEnumerable<TourImage>> GetByUserId(int userId)
        {
            return await _context.TourImages
                            .Include(i => i.Tour)
                            .Where(i => i.Tour.User.Id == userId)
                            .ToListAsync();
        }
    }
}
