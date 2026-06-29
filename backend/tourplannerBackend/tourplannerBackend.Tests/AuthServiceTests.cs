using Microsoft.Extensions.Configuration;
using tourplannerBackend.DTOs;
using tourplannerBackend.Exceptions;
using tourplannerBackend.Services;
using tourplannerBackend.Tests.TestDoubles;

namespace tourplannerBackend.Tests
{
    /// <summary>
    /// Unit tests for the BL-layer AuthService against an in-memory fake user repository.
    /// Covers registration (incl. duplicate-username domain exception), login success/failure
    /// and the PBKDF2 password hashing round-trip.
    /// </summary>
    public class AuthServiceTests
    {
        private readonly FakeUserRepository _users = new();

        private static IConfiguration BuildConfig() =>
            new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["Jwt:Key"]      = "unit-test-signing-key-at-least-32-chars!!",
                    ["Jwt:Issuer"]   = "tourplanner-test",
                    ["Jwt:Audience"] = "tourplanner-test"
                })
                .Build();

        private AuthService CreateService() => new(_users, BuildConfig());

        // ─── RegisterAsync ────────────────────────────────────────────────────

        [Fact]
        public async Task RegisterAsync_CreatesUser_AndReturnsToken()
        {
            var dto = new UserRegisterDto { Username = "alice", Password = "secret123", Email = "a@b.c" };

            var result = await CreateService().RegisterAsync(dto);

            Assert.Equal("alice", result.Username);
            Assert.True(result.UserId > 0);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
            Assert.Single(_users.Users);
        }

        [Fact]
        public async Task RegisterAsync_DoesNotStorePasswordInPlaintext()
        {
            var dto = new UserRegisterDto { Username = "bob", Password = "secret123" };

            await CreateService().RegisterAsync(dto);

            Assert.NotEqual("secret123", _users.Users[0].Password);
            Assert.Contains(":", _users.Users[0].Password); // salt:hash format
        }

        [Fact]
        public async Task RegisterAsync_ThrowsConflict_WhenUsernameTaken()
        {
            _users.Users.Add(new() { Id = 1, Username = "taken", Password = "x" });
            var dto = new UserRegisterDto { Username = "taken", Password = "secret123" };

            await Assert.ThrowsAsync<ConflictException>(
                () => CreateService().RegisterAsync(dto));
        }

        // ─── LoginAsync ───────────────────────────────────────────────────────

        [Fact]
        public async Task LoginAsync_ReturnsToken_OnCorrectCredentials()
        {
            var service = CreateService();
            await service.RegisterAsync(new UserRegisterDto { Username = "carol", Password = "pw12345678" });

            var result = await service.LoginAsync(new UserLoginDto { Username = "carol", Password = "pw12345678" });

            Assert.NotNull(result);
            Assert.Equal("carol", result!.Username);
            Assert.False(string.IsNullOrWhiteSpace(result.Token));
        }

        [Fact]
        public async Task LoginAsync_ReturnsNull_OnWrongPassword()
        {
            var service = CreateService();
            await service.RegisterAsync(new UserRegisterDto { Username = "dave", Password = "correct-pw" });

            var result = await service.LoginAsync(new UserLoginDto { Username = "dave", Password = "wrong-pw" });

            Assert.Null(result);
        }
    }
}
