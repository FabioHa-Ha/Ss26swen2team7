using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public interface ITransportTypeRepository
    {
        Task<TransportType?> GetByIdAsync(int id);
        Task<IEnumerable<TransportType>> GetAllAsync();
    }
}
