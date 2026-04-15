using tourplannerBackend.DTOs;
using tourplannerBackend.Model;

namespace tourplannerBackend.Repositories
{
    public interface IImageRepository
    {
        Task<int> CreateImage(TourImage tourImage);
        Task<TourImage?> GetImage(int id);
    }
}
