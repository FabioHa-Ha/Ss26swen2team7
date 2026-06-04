using tourplannerBackend.DTOs;
using tourplannerBackend.Exceptions;
using tourplannerBackend.Model;
using tourplannerBackend.Repositories;

namespace tourplannerBackend.Services
{
    public class TourService : ITourService
    {
        private readonly ITourRepository _tourRepository;
        private readonly IUserRepository _userRepository;
        private readonly ITransportTypeRepository _transportTypeRepository;

        public TourService(
            ITourRepository tourRepository,
            IUserRepository userRepository,
            ITransportTypeRepository transportTypeRepository)
        {
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _transportTypeRepository = transportTypeRepository;
        }

        public async Task<IEnumerable<TourResponseDto>> GetAllAsync()
        {
            var tours = await _tourRepository.GetAllAsync();
            return tours.Select(MapToDto);
        }

        public async Task<IEnumerable<TourResponseDto>> GetByUserIdAsync(int userId)
        {
            var tours = await _tourRepository.GetByUserIdAsync(userId);
            return tours.Select(MapToDto);
        }

        public async Task<TourResponseDto?> GetByIdAsync(int id)
        {
            var tour = await _tourRepository.GetByIdAsync(id);
            return tour == null ? null : MapToDto(tour);
        }

        public async Task<TourResponseDto> CreateAsync(int userId, TourCreateDto dto)
        {
            // Layer-based error propagation:
            // Repository returns null → Service translates to NotFoundException (domain language)
            // TourController's DomainExceptionFilter then maps it to HTTP 404 ProblemDetails
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException(nameof(User), userId);

            var transportType = await _transportTypeRepository.GetByIdAsync(dto.TransportTypeId)
                ?? throw new NotFoundException(nameof(TransportType), dto.TransportTypeId);

            if (dto.Distance.HasValue && dto.Distance.Value <= 0)
                throw new BusinessRuleException("Distance must be greater than 0.", nameof(dto.Distance));

            if (dto.EstimatedTime.HasValue && dto.EstimatedTime.Value <= 0)
                throw new BusinessRuleException("Estimated time must be greater than 0.", nameof(dto.EstimatedTime));

            var tour = new Tour
            {
                User              = user,
                Name              = dto.Name,
                Description       = dto.Description,
                FromLocation      = dto.FromLocation,
                ToLocation        = dto.ToLocation,
                TransportType     = transportType,
                Distance          = dto.Distance,
                EstimatedTime     = dto.EstimatedTime,
                RouteInformation  = dto.RouteInformation
            };

            var created = await _tourRepository.CreateAsync(tour);
            return MapToDto(created);
        }

        public async Task<TourResponseDto?> UpdateAsync(int id, TourUpdateDto dto)
        {
            var tour = await _tourRepository.GetByIdAsync(id);
            if (tour == null) return null;

            if (dto.Distance.HasValue && dto.Distance.Value <= 0)
                throw new BusinessRuleException("Distance must be greater than 0.", nameof(dto.Distance));

            if (dto.EstimatedTime.HasValue && dto.EstimatedTime.Value <= 0)
                throw new BusinessRuleException("Estimated time must be greater than 0.", nameof(dto.EstimatedTime));

            if (dto.Name        != null) tour.Name             = dto.Name;
            if (dto.Description != null) tour.Description      = dto.Description;
            if (dto.FromLocation != null) tour.FromLocation    = dto.FromLocation;
            if (dto.ToLocation  != null) tour.ToLocation       = dto.ToLocation;
            if (dto.Distance.HasValue)   tour.Distance         = dto.Distance;
            if (dto.EstimatedTime.HasValue) tour.EstimatedTime = dto.EstimatedTime;
            if (dto.RouteInformation != null) tour.RouteInformation = dto.RouteInformation;

            if (dto.TransportTypeId.HasValue)
            {
                tour.TransportType = await _transportTypeRepository.GetByIdAsync(dto.TransportTypeId.Value)
                    ?? throw new NotFoundException(nameof(TransportType), dto.TransportTypeId.Value);
            }

            var updated = await _tourRepository.UpdateAsync(tour);
            return updated == null ? null : MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _tourRepository.DeleteAsync(id);
        }

        private static TourResponseDto MapToDto(Tour tour) => new()
        {
            Id               = tour.Id,
            UserId           = tour.User.Id,
            Name             = tour.Name,
            Description      = tour.Description,
            FromLocation     = tour.FromLocation,
            ToLocation       = tour.ToLocation,
            TransportTypeId  = tour.TransportType.Id,
            TransportTypeName = tour.TransportType.Name,
            Distance         = tour.Distance,
            EstimatedTime    = tour.EstimatedTime,
            RouteInformation = tour.RouteInformation,
            ImageIds         = tour.Images.Select(i => i.Id).ToList()
        };
    }
}
