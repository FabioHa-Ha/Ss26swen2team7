using tourplannerBackend.DTOs;
using tourplannerBackend.Exceptions;
using tourplannerBackend.Model;
using tourplannerBackend.Repositories;

namespace tourplannerBackend.Services
{
    public class TourLogService : ITourLogService
    {
        private readonly ITourLogRepository _tourLogRepository;
        private readonly ITourRepository _tourRepository;
        private readonly IUserRepository _userRepository;
        private readonly IDifficultyRepository _difficultyRepository;

        public TourLogService(
            ITourLogRepository tourLogRepository,
            ITourRepository tourRepository,
            IUserRepository userRepository,
            IDifficultyRepository difficultyRepository)
        {
            _tourLogRepository = tourLogRepository;
            _tourRepository = tourRepository;
            _userRepository = userRepository;
            _difficultyRepository = difficultyRepository;
        }

        public async Task<IEnumerable<TourLogResponseDto>> GetAllAsync()
        {
            var logs = await _tourLogRepository.GetAllAsync();
            return logs.Select(MapToDto);
        }

        public async Task<IEnumerable<TourLogResponseDto>> GetByTourIdAsync(int tourId)
        {
            var logs = await _tourLogRepository.GetByTourIdAsync(tourId);
            return logs.Select(MapToDto);
        }

        public async Task<IEnumerable<TourLogResponseDto>> GetByUserIdAsync(int userId)
        {
            var logs = await _tourLogRepository.GetByUserIdAsync(userId);
            return logs.Select(MapToDto);
        }

        public async Task<TourLogResponseDto?> GetByIdAsync(int id)
        {
            var log = await _tourLogRepository.GetByIdAsync(id);
            return log == null ? null : MapToDto(log);
        }

        public async Task<TourLogResponseDto> CreateAsync(int userId, TourLogCreateDto dto)
        {
            var user = await _userRepository.GetByIdAsync(userId)
                ?? throw new NotFoundException(nameof(User), userId);

            var tour = await _tourRepository.GetByIdAsync(dto.TourId)
                ?? throw new NotFoundException(nameof(Tour), dto.TourId);

            var difficulty = await _difficultyRepository.GetByIdAsync(dto.DifficultyId)
                ?? throw new NotFoundException(nameof(Difficulty), dto.DifficultyId);

            if (dto.TotalDistance <= 0)
                throw new BusinessRuleException("Total distance must be greater than 0.", nameof(dto.TotalDistance));

            if (dto.TotalTime <= 0)
                throw new BusinessRuleException("Total time must be greater than 0.", nameof(dto.TotalTime));

            if (dto.Rating < 1 || dto.Rating > 5)
                throw new BusinessRuleException("Rating must be between 1 and 5.", nameof(dto.Rating));

            var log = new TourLog
            {
                Tour = tour,
                User = user,
                Date = dto.Date,
                Comment = dto.Comment,
                Difficulty = difficulty,
                TotalDistance = dto.TotalDistance,
                TotalTime = dto.TotalTime,
                Rating = dto.Rating
            };

            var created = await _tourLogRepository.CreateAsync(log);
            return MapToDto(created);
        }

        public async Task<TourLogResponseDto?> UpdateAsync(int id, TourLogUpdateDto dto)
        {
            var log = await _tourLogRepository.GetByIdAsync(id);
            if (log == null) return null;

            if (dto.TotalDistance.HasValue && dto.TotalDistance.Value <= 0)
                throw new BusinessRuleException("Total distance must be greater than 0.", nameof(dto.TotalDistance));

            if (dto.TotalTime.HasValue && dto.TotalTime.Value <= 0)
                throw new BusinessRuleException("Total time must be greater than 0.", nameof(dto.TotalTime));

            if (dto.Rating.HasValue && (dto.Rating.Value < 1 || dto.Rating.Value > 5))
                throw new BusinessRuleException("Rating must be between 1 and 5.", nameof(dto.Rating));

            if (dto.Date.HasValue) log.Date = dto.Date.Value;
            if (dto.Comment != null) log.Comment = dto.Comment;
            if (dto.TotalDistance.HasValue) log.TotalDistance = dto.TotalDistance.Value;
            if (dto.TotalTime.HasValue) log.TotalTime = dto.TotalTime.Value;
            if (dto.Rating.HasValue) log.Rating = dto.Rating.Value;

            if (dto.DifficultyId.HasValue)
            {
                var difficulty = await _difficultyRepository.GetByIdAsync(dto.DifficultyId.Value)
                    ?? throw new NotFoundException(nameof(Difficulty), dto.DifficultyId.Value);
                log.Difficulty = difficulty;
            }

            var updated = await _tourLogRepository.UpdateAsync(log);
            return updated == null ? null : MapToDto(updated);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _tourLogRepository.DeleteAsync(id);
        }

        private static TourLogResponseDto MapToDto(TourLog log) => new()
        {
            Id = log.Id,
            TourId = log.Tour.Id,
            UserId = log.User.Id,
            Date = log.Date,
            Comment = log.Comment,
            DifficultyId = log.Difficulty.Id,
            DifficultyName = log.Difficulty.Name,
            TotalDistance = log.TotalDistance,
            TotalTime = log.TotalTime,
            Rating = log.Rating
        };
    }
}
