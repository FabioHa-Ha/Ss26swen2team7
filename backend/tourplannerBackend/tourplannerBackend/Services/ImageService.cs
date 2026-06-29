using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using tourplannerBackend.DTOs;
using tourplannerBackend.Exceptions;
using tourplannerBackend.Model;
using tourplannerBackend.Repositories;

namespace tourplannerBackend.Services
{
    public class ImageService : IImageService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ITourRepository _tourRepository;

        public ImageService(IImageRepository imageRepository, ITourRepository tourRepository)
        {
            _imageRepository = imageRepository;
            _tourRepository = tourRepository;
        }

        public byte[] ResizeAndConvertToJpeg(byte[] inputBytes)
        {
            int width = 1920;
            int height = 1080;

            using (Image image = Image.Load<Rgba32>(inputBytes))
            {
                using (MemoryStream ms = new MemoryStream())
                {
                    image.Mutate(i => i.BackgroundColor(Color.White));

                    ResizeOptions options = new ResizeOptions
                    {
                        Size = new Size(width, height),
                        Mode = ResizeMode.Pad,
                        PadColor = Color.White,
                        Sampler = KnownResamplers.NearestNeighbor
                    };

                    image.Mutate(i => i.Resize(options));

                    image.Save(ms, new JpegEncoder());
                    return ms.ToArray();
                }
            }
        }

        public async Task<int> CreateImage(ImageCreateDto imageCreateDto)
        {
            using var memoryStream = new MemoryStream();
            await imageCreateDto.Image.CopyToAsync(memoryStream);

            byte[] imageBytes = memoryStream.ToArray();

            imageBytes = ResizeAndConvertToJpeg(imageBytes);

            var tour = await _tourRepository.GetByIdAsync(imageCreateDto.TourId);

            if (tour == null)
            {
                // Use the BL layer's own domain exception (→ HTTP 404) instead of
                // the framework's KeyNotFoundException.
                throw new NotFoundException(nameof(Tour), imageCreateDto.TourId);
            }

            TourImage tourImage = new TourImage
            {
                Tour = tour,
                Image = imageBytes,
                FileName = Path.GetFileNameWithoutExtension(imageCreateDto.Image.FileName) + ".jpeg",
                ContentType = "image/jpeg",
            };
            return await _imageRepository.CreateImage(tourImage);
        }

        public Task<TourImage?> GetImage(int id)
        {
            return _imageRepository.GetImage(id);
        }
    }
}
