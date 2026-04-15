using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Model;
using tourplannerBackend.Repositories;

namespace tourplannerBackend.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;

        public ImageService(IImageRepository imageRepository)
        {
            _imageRepository = imageRepository;
        }

        public async Task<int> CreateImage(ImageCreateDto imageCreateDto)
        {
            byte[] test = { };

            using var memoryStream = new MemoryStream();
            await imageCreateDto.Image.CopyToAsync(memoryStream);

            TourImage tourImage = new TourImage
            {
                TourLog = imageCreateDto.TourLogId,
                Image = memoryStream.ToArray(),
                FileName = imageCreateDto.Image.FileName,
                ContentType = imageCreateDto.Image.ContentType,
            };
            return await _imageRepository.CreateImage(tourImage);
        }

        public Task<TourImage?> GetImage(int id)
        {
            return _imageRepository.GetImage(id);
        }
    }
}
