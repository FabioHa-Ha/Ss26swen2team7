using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Model;

namespace tourplannerBackend.Services
{
    public interface IImageService
    {
        Task<int> CreateImage(ImageCreateDto imageCreateDto);
        Task<TourImage?> GetImage(int id);
    }
}
