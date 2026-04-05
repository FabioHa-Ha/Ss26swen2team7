using Microsoft.EntityFrameworkCore;
using tourPlannerBackend.Data;
using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public class TransportTypeRepository : ITransportTypeRepository
    {
        private readonly ApplicationDbContext _context;

        public TransportTypeRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<TransportType?> GetByIdAsync(int id)
        {
            return await _context.TransportTypes.FindAsync(id);
        }

        public async Task<IEnumerable<TransportType>> GetAllAsync()
        {
            return await _context.TransportTypes.ToListAsync();
        }
    }
}
