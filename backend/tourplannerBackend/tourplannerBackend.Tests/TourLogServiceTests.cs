using tourplannerBackend.DTOs;
using tourplannerBackend.Exceptions;
using tourplannerBackend.Model;
using tourplannerBackend.Services;
using tourplannerBackend.Tests.TestDoubles;

namespace tourplannerBackend.Tests
{
    /// <summary>
    /// Unit tests for the BL-layer TourLogService against in-memory fake repositories.
    /// Covers creation, validation rules (positive distance, rating range),
    /// domain-exception translation and update/delete behaviour.
    /// </summary>
    public class TourLogServiceTests
    {
        private readonly FakeTourLogRepository _logs = new();
        private readonly FakeTourRepository _tours = new();
        private readonly FakeUserRepository _users = new();
        private readonly FakeDifficultyRepository _difficulties = new();

        private TourLogService CreateService() =>
            new(_logs, _tours, _users, _difficulties);

        private User SeedUser(int id = 1)
        {
            var user = new User { Id = id, Username = $"user{id}", Password = "x" };
            _users.Users.Add(user);
            return user;
        }

        private Difficulty SeedDifficulty(int id = 3)
        {
            var d = new Difficulty { Id = id, Name = "Medium" };
            _difficulties.Difficulties.Add(d);
            return d;
        }

        private Tour SeedTour(User user, int id = 1)
        {
            var tour = new Tour
            {
                Id = id,
                User = user,
                Name = "Tour",
                FromLocation = "A",
                ToLocation = "B",
                TransportType = new TransportType { Id = 1, Name = "Hike" }
            };
            _tours.Tours.Add(tour);
            return tour;
        }

        private static TourLogCreateDto ValidCreateDto(int tourId = 1, int difficultyId = 3) => new()
        {
            TourId = tourId,
            Date = new DateTime(2026, 6, 1),
            Comment = "Nice",
            DifficultyId = difficultyId,
            TotalDistance = 50,
            TotalTime = 90,
            Rating = 4
        };

        private TourLog SeedLog(Tour tour, User user, Difficulty diff, int id = 1)
        {
            var log = new TourLog
            {
                Id = id,
                Tour = tour,
                User = user,
                Date = new DateTime(2026, 5, 1),
                Difficulty = diff,
                TotalDistance = 10,
                TotalTime = 20,
                Rating = 3
            };
            _logs.Logs.Add(log);
            return log;
        }

        // ─── CreateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ReturnsMappedDto_AndPersists_OnSuccess()
        {
            var user = SeedUser();
            SeedTour(user);
            SeedDifficulty();

            var result = await CreateService().CreateAsync(1, ValidCreateDto());

            Assert.Equal(1, result.TourId);
            Assert.Equal("Medium", result.DifficultyName);
            Assert.Single(_logs.Logs);
        }

        [Fact]
        public async Task CreateAsync_ThrowsNotFound_WhenTourMissing()
        {
            SeedUser();
            SeedDifficulty();
            await Assert.ThrowsAsync<NotFoundException>(
                () => CreateService().CreateAsync(1, ValidCreateDto(tourId: 555)));
        }

        [Fact]
        public async Task CreateAsync_ThrowsNotFound_WhenDifficultyMissing()
        {
            var user = SeedUser();
            SeedTour(user);
            await Assert.ThrowsAsync<NotFoundException>(
                () => CreateService().CreateAsync(1, ValidCreateDto(difficultyId: 9)));
        }

        [Fact]
        public async Task CreateAsync_ThrowsBusinessRule_WhenTotalDistanceNotPositive()
        {
            var user = SeedUser();
            SeedTour(user);
            SeedDifficulty();
            var dto = ValidCreateDto();
            dto.TotalDistance = 0;

            await Assert.ThrowsAsync<BusinessRuleException>(
                () => CreateService().CreateAsync(1, dto));
        }

        [Fact]
        public async Task CreateAsync_ThrowsBusinessRule_WhenRatingOutOfRange()
        {
            var user = SeedUser();
            SeedTour(user);
            SeedDifficulty();
            var dto = ValidCreateDto();
            dto.Rating = 6;

            var ex = await Assert.ThrowsAsync<BusinessRuleException>(
                () => CreateService().CreateAsync(1, dto));
            Assert.Equal(nameof(dto.Rating), ex.Field);
        }

        // ─── UpdateAsync / DeleteAsync ────────────────────────────────────────

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenLogMissing()
        {
            Assert.Null(await CreateService().UpdateAsync(404, new TourLogUpdateDto { Rating = 2 }));
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenLogExists_AndFalse_WhenMissing()
        {
            var user = SeedUser();
            var diff = SeedDifficulty();
            var tour = SeedTour(user);
            var log = SeedLog(tour, user, diff);

            Assert.True(await CreateService().DeleteAsync(log.Id));
            Assert.False(await CreateService().DeleteAsync(log.Id));
        }
    }
}
