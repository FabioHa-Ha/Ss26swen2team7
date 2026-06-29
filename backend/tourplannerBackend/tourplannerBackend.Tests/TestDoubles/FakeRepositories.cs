using tourplannerBackend.Model;
using tourplannerBackend.Repositories;

namespace tourplannerBackend.Tests.TestDoubles
{
    /// <summary>
    /// Lightweight, list-backed test doubles for the DAL interfaces.
    ///
    /// We hand-roll fakes instead of pulling in a mocking library: the repository
    /// contracts are small, and explicit fakes keep the BL-layer tests dependency-free
    /// and easy to read. Each fake exposes its backing list so tests can seed data and
    /// assert on what the service persisted.
    /// </summary>
    public sealed class FakeUserRepository : IUserRepository
    {
        public List<User> Users { get; } = new();
        private int _nextId = 1;

        public Task<User?> GetByIdAsync(int id) =>
            Task.FromResult(Users.FirstOrDefault(u => u.Id == id));

        public Task<User?> GetByUsernameAsync(string username) =>
            Task.FromResult(Users.FirstOrDefault(u => u.Username == username));

        public Task<User?> GetByEmailAsync(string email) =>
            Task.FromResult(Users.FirstOrDefault(u => u.Email == email));

        public Task<User> CreateAsync(User user)
        {
            user.Id = _nextId++;
            Users.Add(user);
            return Task.FromResult(user);
        }

        public Task<bool> ExistsAsync(string username) =>
            Task.FromResult(Users.Any(u => u.Username == username));
    }

    public sealed class FakeTransportTypeRepository : ITransportTypeRepository
    {
        public List<TransportType> TransportTypes { get; } = new();

        public Task<TransportType?> GetByIdAsync(int id) =>
            Task.FromResult(TransportTypes.FirstOrDefault(t => t.Id == id));

        public Task<IEnumerable<TransportType>> GetAllAsync() =>
            Task.FromResult<IEnumerable<TransportType>>(TransportTypes);
    }

    public sealed class FakeDifficultyRepository : IDifficultyRepository
    {
        public List<Difficulty> Difficulties { get; } = new();

        public Task<Difficulty?> GetByIdAsync(int id) =>
            Task.FromResult(Difficulties.FirstOrDefault(d => d.Id == id));

        public Task<IEnumerable<Difficulty>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Difficulty>>(Difficulties);
    }

    public sealed class FakeTourRepository : ITourRepository
    {
        public List<Tour> Tours { get; } = new();
        private int _nextId = 1;

        public Task<IEnumerable<Tour>> GetAllAsync() =>
            Task.FromResult<IEnumerable<Tour>>(Tours);

        public Task<IEnumerable<Tour>> GetByUserIdAsync(int userId) =>
            Task.FromResult<IEnumerable<Tour>>(Tours.Where(t => t.User.Id == userId).ToList());

        public Task<Tour?> GetByIdAsync(int id) =>
            Task.FromResult(Tours.FirstOrDefault(t => t.Id == id));

        public Task<Tour> CreateAsync(Tour tour)
        {
            tour.Id = _nextId++;
            Tours.Add(tour);
            return Task.FromResult(tour);
        }

        public Task<Tour?> UpdateAsync(Tour tour)
        {
            var existing = Tours.FirstOrDefault(t => t.Id == tour.Id);
            return Task.FromResult(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var existing = Tours.FirstOrDefault(t => t.Id == id);
            if (existing == null) return Task.FromResult(false);
            Tours.Remove(existing);
            return Task.FromResult(true);
        }
    }

    public sealed class FakeTourLogRepository : ITourLogRepository
    {
        public List<TourLog> Logs { get; } = new();
        private int _nextId = 1;

        public Task<IEnumerable<TourLog>> GetAllAsync() =>
            Task.FromResult<IEnumerable<TourLog>>(Logs);

        public Task<IEnumerable<TourLog>> GetByTourIdAsync(int tourId) =>
            Task.FromResult<IEnumerable<TourLog>>(Logs.Where(l => l.Tour.Id == tourId).ToList());

        public Task<IEnumerable<TourLog>> GetByUserIdAsync(int userId) =>
            Task.FromResult<IEnumerable<TourLog>>(Logs.Where(l => l.User.Id == userId).ToList());

        public Task<TourLog?> GetByIdAsync(int id) =>
            Task.FromResult(Logs.FirstOrDefault(l => l.Id == id));

        public Task<TourLog> CreateAsync(TourLog tourLog)
        {
            tourLog.Id = _nextId++;
            Logs.Add(tourLog);
            return Task.FromResult(tourLog);
        }

        public Task<TourLog?> UpdateAsync(TourLog tourLog)
        {
            var existing = Logs.FirstOrDefault(l => l.Id == tourLog.Id);
            return Task.FromResult(existing);
        }

        public Task<bool> DeleteAsync(int id)
        {
            var existing = Logs.FirstOrDefault(l => l.Id == id);
            if (existing == null) return Task.FromResult(false);
            Logs.Remove(existing);
            return Task.FromResult(true);
        }
    }
}
