using tourplannerBackend.DTOs;
using tourplannerBackend.Exceptions;
using tourplannerBackend.Model;
using tourplannerBackend.Services;
using tourplannerBackend.Tests.TestDoubles;

namespace tourplannerBackend.Tests
{
    /// <summary>
    /// Unit tests for the BL-layer TourService, exercised against in-memory fake
    /// repositories. Covers happy paths, domain-exception translation and the
    /// computed-attribute logic (average rating, child-friendliness).
    /// </summary>
    public class TourServiceTests
    {
        private readonly FakeTourRepository _tours = new();
        private readonly FakeUserRepository _users = new();
        private readonly FakeTransportTypeRepository _transportTypes = new();
        private readonly FakeTourLogRepository _logs = new();

        private TourService CreateService() =>
            new(_tours, _users, _transportTypes, _logs);

        private User SeedUser(int id = 1)
        {
            var user = new User { Id = id, Username = $"user{id}", Password = "x" };
            _users.Users.Add(user);
            return user;
        }

        private TransportType SeedTransportType(int id = 1)
        {
            var tt = new TransportType { Id = id, Name = "Hike" };
            _transportTypes.TransportTypes.Add(tt);
            return tt;
        }

        private static TourCreateDto ValidCreateDto(int transportTypeId = 1) => new()
        {
            Name = "Test Tour",
            FromLocation = "Vienna",
            ToLocation = "Graz",
            TransportTypeId = transportTypeId,
            Distance = 200,
            EstimatedTime = 120
        };

        private Tour SeedTour(User user, TransportType tt, int id = 1)
        {
            var tour = new Tour
            {
                Id = id,
                User = user,
                Name = "Existing",
                FromLocation = "A",
                ToLocation = "B",
                TransportType = tt
            };
            _tours.Tours.Add(tour);
            return tour;
        }

        private void AddLog(Tour tour, User user, int difficultyId, int rating)
        {
            _logs.Logs.Add(new TourLog
            {
                Tour = tour,
                User = user,
                Date = new DateTime(2026, 6, 1),
                Difficulty = new Difficulty { Id = difficultyId, Name = "D" },
                TotalDistance = 1,
                TotalTime = 1,
                Rating = rating
            });
        }

        // ─── CreateAsync ──────────────────────────────────────────────────────

        [Fact]
        public async Task CreateAsync_ReturnsMappedDto_AndPersists_OnSuccess()
        {
            SeedUser();
            SeedTransportType();

            var result = await CreateService().CreateAsync(1, ValidCreateDto());

            Assert.Equal("Test Tour", result.Name);
            Assert.Equal(1, result.UserId);
            Assert.Single(_tours.Tours);
        }

        [Fact]
        public async Task CreateAsync_ThrowsNotFound_WhenUserMissing()
        {
            SeedTransportType();
            await Assert.ThrowsAsync<NotFoundException>(
                () => CreateService().CreateAsync(99, ValidCreateDto()));
        }

        [Fact]
        public async Task CreateAsync_ThrowsNotFound_WhenTransportTypeMissing()
        {
            SeedUser();
            await Assert.ThrowsAsync<NotFoundException>(
                () => CreateService().CreateAsync(1, ValidCreateDto(transportTypeId: 42)));
        }

        [Fact]
        public async Task CreateAsync_ThrowsBusinessRule_WhenDistanceNotPositive()
        {
            SeedUser();
            SeedTransportType();
            var dto = ValidCreateDto();
            dto.Distance = 0;

            var ex = await Assert.ThrowsAsync<BusinessRuleException>(
                () => CreateService().CreateAsync(1, dto));
            Assert.Equal(nameof(dto.Distance), ex.Field);
        }

        // ─── GetByIdAsync & computed attributes ───────────────────────────────

        [Fact]
        public async Task GetByIdAsync_ReturnsNull_WhenTourMissing()
        {
            Assert.Null(await CreateService().GetByIdAsync(123));
        }

        [Fact]
        public async Task GetByIdAsync_ComputesAverageRating_RoundedToOneDecimal()
        {
            var user = SeedUser();
            var tour = SeedTour(user, SeedTransportType());
            AddLog(tour, user, difficultyId: 3, rating: 5);
            AddLog(tour, user, difficultyId: 3, rating: 4);
            AddLog(tour, user, difficultyId: 3, rating: 4);

            var result = await CreateService().GetByIdAsync(tour.Id);

            Assert.Equal(3, result!.TotalLogs);
            Assert.Equal(4.3, result.AverageRating); // (5+4+4)/3 = 4.333 → 4.3
        }

        [Fact]
        public async Task GetByIdAsync_ChildFriendlinessIsHigh_ForEasyLogs()
        {
            var user = SeedUser();
            var tour = SeedTour(user, SeedTransportType());
            AddLog(tour, user, difficultyId: 1, rating: 5); // easiest difficulty

            var result = await CreateService().GetByIdAsync(tour.Id);

            Assert.Equal(5, result!.ChildFriendliness);
        }

        // ─── UpdateAsync / DeleteAsync ────────────────────────────────────────

        [Fact]
        public async Task UpdateAsync_ReturnsNull_WhenTourMissing()
        {
            Assert.Null(await CreateService().UpdateAsync(404, new TourUpdateDto { Name = "x" }));
        }

        [Fact]
        public async Task DeleteAsync_ReturnsTrue_WhenTourExists_AndFalse_WhenMissing()
        {
            var user = SeedUser();
            var tour = SeedTour(user, SeedTransportType());

            Assert.True(await CreateService().DeleteAsync(tour.Id));
            Assert.False(await CreateService().DeleteAsync(tour.Id));
        }
    }
}
