using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.IO.Compression;
using System.Text.Json;
using tourplannerBackend.Exceptions;
using tourplannerBackend.Model;
using tourplannerBackend.Repositories;

namespace tourplannerBackend.Services
{
    public class ImportExportService : IImportExportService
    {
        private readonly IImageRepository _imageRepository;
        private readonly ITourRepository _tourRepository;
        private readonly ITourLogRepository _tourLogRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransportTypeRepository _transportTypeRepository;
        private readonly IDifficultyRepository _difficultyRepository;

        public ImportExportService(IImageRepository imageRepository, 
            ITourRepository tourRepository, 
            ITourLogRepository tourLogRepository,
            IUserRepository userRepository,
            ITransportTypeRepository transportTypeRepository,
            IDifficultyRepository difficultyRepository)
        {
            _imageRepository = imageRepository;
            _tourRepository = tourRepository;
            _tourLogRepository = tourLogRepository;
            _userRepository = userRepository;
            _transportTypeRepository = transportTypeRepository;
            _difficultyRepository = difficultyRepository;
        }

        public async Task<byte[]?> ExportDatabaseForUser(int userId)
        {
            MemoryStream zipStream = new MemoryStream();

            using (ZipArchive archive = new ZipArchive(zipStream, ZipArchiveMode.Create, leaveOpen: true))
            {
                IEnumerable<Tour> tours = await _tourRepository.GetByUserIdAsync(userId);
                if (tours.Count() > 0)
                {
                    await AddTableToZip(archive, "tours.json", tours);
                }

                IEnumerable<TourLog> tourLogs = await _tourLogRepository.GetByUserIdAsync(userId);
                if (tourLogs.Count() > 0)
                {
                    await AddTableToZip(archive, "tourLogs.json", tourLogs);
                }

                IEnumerable<TourImage> tourImages = await _imageRepository.GetByUserId(userId);
                if (tourImages.Count() > 0)
                {
                    await AddTableToZip(archive, "images.json", tourImages);
                }
            }

            zipStream.Position = 0;
            return zipStream.ToArray();
        }

        private async Task AddTableToZip(ZipArchive archive, string fileName, object data)
        {
            if (data == null) return;

            var entry = archive.CreateEntry(fileName);
            await using var entryStream = entry.Open();
            await JsonSerializer.SerializeAsync(entryStream, data, new JsonSerializerOptions { WriteIndented = true });
        }

        public async Task<bool> ImportDatabaseForUser(IFormFile zipFile, int userId)
        {
            using Stream stream = zipFile.OpenReadStream();
            using ZipArchive archive = new ZipArchive(stream, ZipArchiveMode.Read);

            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException(nameof(User), userId);

            Dictionary<int, int> newToursMapping = new Dictionary<int, int>();

            ZipArchiveEntry? toursEntry = archive.GetEntry("tours.json");
            if (toursEntry != null)
            {
                using Stream entryStream = toursEntry.Open();
                IEnumerable<Tour>? importedTours = await JsonSerializer.DeserializeAsync<IEnumerable<Tour>>(entryStream);

                if (importedTours != null)
                {
                    foreach (Tour tour in importedTours)
                    {
                        var transportType = await _transportTypeRepository.GetByIdAsync(tour.TransportType.Id)
                            ?? throw new NotFoundException(nameof(TransportType), tour.TransportType.Id);

                        Tour newTour = new Tour
                        {
                            User = user,
                            Name = tour.Name,
                            Description = tour.Description,
                            FromLocation = tour.FromLocation,
                            ToLocation = tour.ToLocation,
                            TransportType = transportType,
                            Distance = tour.Distance,
                            EstimatedTime = tour.EstimatedTime,
                            RouteInformation = tour.RouteInformation
                        };
                        int createdTourId = _tourRepository.CreateAsync(newTour).GetAwaiter().GetResult().Id;
                        newToursMapping.Add(tour.Id, createdTourId);
                    }
                }
            }

            ZipArchiveEntry? tourLogsEntry = archive.GetEntry("tourLogs.json");
            if (tourLogsEntry != null)
            {
                using Stream entryStream = tourLogsEntry.Open();
                IEnumerable<TourLog>? importedTourLogs = await JsonSerializer.DeserializeAsync<IEnumerable<TourLog>>(entryStream);

                if (importedTourLogs != null)
                {
                    foreach (TourLog tourLog in importedTourLogs)
                    {
                        int createdTourId = newToursMapping[tourLog.Tour.Id];
                        var tour = await _tourRepository.GetByIdAsync(createdTourId)
                            ?? throw new NotFoundException(nameof(Tour), createdTourId);
                        var difficulty = await _difficultyRepository.GetByIdAsync(tourLog.Difficulty.Id)
                            ?? throw new NotFoundException(nameof(Difficulty), tourLog.Difficulty.Id);

                        TourLog newTourLog = new TourLog
                        {
                            User = user,
                            Tour = tour,
                            Date = tourLog.Date,
                            Comment = tourLog.Comment,
                            Difficulty = difficulty,
                            TotalDistance = tourLog.TotalDistance,
                            TotalTime = tourLog.TotalTime,
                            Rating = tourLog.Rating
                        };
                        await _tourLogRepository.CreateAsync(newTourLog);
                    }
                }
            }

            ZipArchiveEntry? imagesEntry = archive.GetEntry("images.json");
            if (imagesEntry != null)
            {
                using Stream entryStream = imagesEntry.Open();
                IEnumerable<TourImage>? importedImages = await JsonSerializer.DeserializeAsync<IEnumerable<TourImage>>(entryStream);

                if (importedImages != null)
                {
                    foreach (TourImage tourImage in importedImages)
                    {
                        int createdTourId = newToursMapping[tourImage.Tour.Id];
                        var tour = await _tourRepository.GetByIdAsync(createdTourId)
                            ?? throw new NotFoundException(nameof(Tour), createdTourId);

                        TourImage newTourImage = new TourImage
                        {
                            Tour = tour,
                            Image = tourImage.Image,
                            FileName = tourImage.FileName,
                            ContentType = tourImage.ContentType
                        };

                        await _imageRepository.CreateImage(newTourImage);
                    }
                }
            }
            return true;
        }
    }
}
