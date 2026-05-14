using System.Net;
using System.Text;
using Microsoft.Extensions.Configuration;
using tourplannerBackend.Services;

namespace tourplannerBackend.Tests
{
    public class WeatherServiceTests
    {
        private const string FakeApiKey = "test-api-key";

        private static WeatherService CreateService(HttpMessageHandler handler)
        {
            var httpClient = new HttpClient(handler);
            var config = new ConfigurationBuilder()
                .AddInMemoryCollection(new Dictionary<string, string?> { ["OpenWeather:ApiKey"] = FakeApiKey })
                .Build();
            return new WeatherService(httpClient, config);
        }

        private static string BuildWeatherJson(
            string city = "Vienna",
            double temp = 18.5,
            double feelsLike = 17.0,
            int humidity = 65,
            string description = "clear sky",
            string icon = "01d",
            double windSpeed = 3.2) =>
            $$"""
            {
              "weather": [{"description": "{{description}}", "icon": "{{icon}}"}],
              "main": {"temp": {{temp}}, "feels_like": {{feelsLike}}, "humidity": {{humidity}}},
              "wind": {"speed": {{windSpeed}}},
              "name": "{{city}}"
            }
            """;

        // ─── GetCurrentWeatherByCityAsync ─────────────────────────────────────

        [Fact]
        public async Task GetCurrentWeatherByCityAsync_ReturnsCorrectData_OnSuccess()
        {
            var handler = new MockHttpMessageHandler(
                HttpStatusCode.OK,
                BuildWeatherJson("Graz", 22.3, 21.0, 70, "few clouds", "02d", 5.1));

            var service = CreateService(handler);
            var result = await service.GetCurrentWeatherByCityAsync("Graz");

            Assert.Equal("Graz", result.City);
            Assert.Equal(22.3, result.Temperature);
            Assert.Equal(21.0, result.FeelsLike);
            Assert.Equal(70, result.Humidity);
            Assert.Equal("few clouds", result.Description);
            Assert.Equal("02d", result.Icon);
            Assert.Equal(5.1, result.WindSpeed);
        }

        [Fact]
        public async Task GetCurrentWeatherByCityAsync_RequestContainsCityAndApiKey()
        {
            var handler = new MockHttpMessageHandler(HttpStatusCode.OK, BuildWeatherJson());
            var service = CreateService(handler);

            await service.GetCurrentWeatherByCityAsync("Salzburg");

            var requestUri = handler.LastRequestUri!.ToString();
            Assert.Contains("q=Salzburg", requestUri);
            Assert.Contains($"appid={FakeApiKey}", requestUri);
            Assert.Contains("units=metric", requestUri);
        }

        [Fact]
        public async Task GetCurrentWeatherByCityAsync_EscapesCityName()
        {
            var handler = new MockHttpMessageHandler(HttpStatusCode.OK, BuildWeatherJson("Baden bei Wien"));
            var service = CreateService(handler);

            await service.GetCurrentWeatherByCityAsync("Baden bei Wien");

            var requestUri = handler.LastRequestUri!.ToString();
            Assert.Contains("q=Baden%20bei%20Wien", requestUri);
        }

        [Fact]
        public async Task GetCurrentWeatherByCityAsync_ThrowsHttpRequestException_OnNotFound()
        {
            var handler = new MockHttpMessageHandler(
                HttpStatusCode.NotFound,
                """{"cod":"404","message":"city not found"}""");

            var service = CreateService(handler);

            var ex = await Assert.ThrowsAsync<HttpRequestException>(
                () => service.GetCurrentWeatherByCityAsync("UnknownCity123"));

            Assert.Equal(HttpStatusCode.NotFound, ex.StatusCode);
        }

        [Fact]
        public async Task GetCurrentWeatherByCityAsync_ThrowsHttpRequestException_OnUnauthorized()
        {
            var handler = new MockHttpMessageHandler(
                HttpStatusCode.Unauthorized,
                """{"cod":401,"message":"Invalid API key."}""");

            var service = CreateService(handler);

            var ex = await Assert.ThrowsAsync<HttpRequestException>(
                () => service.GetCurrentWeatherByCityAsync("Vienna"));

            Assert.Equal(HttpStatusCode.Unauthorized, ex.StatusCode);
        }

        // ─── GetCurrentWeatherByCoordinatesAsync ──────────────────────────────

        [Fact]
        public async Task GetCurrentWeatherByCoordinatesAsync_ReturnsCorrectData_OnSuccess()
        {
            var handler = new MockHttpMessageHandler(
                HttpStatusCode.OK,
                BuildWeatherJson("Vienna", 15.0, 14.0, 80, "light rain", "10d", 2.5));

            var service = CreateService(handler);
            var result = await service.GetCurrentWeatherByCoordinatesAsync(48.2082, 16.3738);

            Assert.Equal("Vienna", result.City);
            Assert.Equal(15.0, result.Temperature);
            Assert.Equal(80, result.Humidity);
            Assert.Equal("light rain", result.Description);
        }

        [Fact]
        public async Task GetCurrentWeatherByCoordinatesAsync_RequestContainsLatLonAndApiKey()
        {
            var handler = new MockHttpMessageHandler(HttpStatusCode.OK, BuildWeatherJson());
            var service = CreateService(handler);

            await service.GetCurrentWeatherByCoordinatesAsync(48.2082, 16.3738);

            var requestUri = handler.LastRequestUri!.ToString();
            Assert.Contains("lat=48.2082", requestUri);
            Assert.Contains("lon=16.3738", requestUri);
            Assert.Contains($"appid={FakeApiKey}", requestUri);
            Assert.Contains("units=metric", requestUri);
        }

        [Fact]
        public async Task GetCurrentWeatherByCoordinatesAsync_ThrowsHttpRequestException_OnServerError()
        {
            var handler = new MockHttpMessageHandler(
                HttpStatusCode.InternalServerError,
                "Internal Server Error");

            var service = CreateService(handler);

            var ex = await Assert.ThrowsAsync<HttpRequestException>(
                () => service.GetCurrentWeatherByCoordinatesAsync(0, 0));

            Assert.Equal(HttpStatusCode.InternalServerError, ex.StatusCode);
        }

        // ─── Constructor ──────────────────────────────────────────────────────

        [Fact]
        public void Constructor_ThrowsInvalidOperationException_WhenApiKeyMissing()
        {
            var httpClient = new HttpClient(new MockHttpMessageHandler(HttpStatusCode.OK, "{}"));
            var config = new ConfigurationBuilder().Build(); // no key configured

            Assert.Throws<InvalidOperationException>(
                () => new WeatherService(httpClient, config));
        }
    }

    /// <summary>
    /// Test double for HttpMessageHandler that returns a preset response
    /// and records the last request URI for assertion.
    /// </summary>
    internal sealed class MockHttpMessageHandler(HttpStatusCode statusCode, string content)
        : HttpMessageHandler
    {
        public Uri? LastRequestUri { get; private set; }

        protected override Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            LastRequestUri = request.RequestUri;
            var response = new HttpResponseMessage(statusCode)
            {
                Content = new StringContent(content, Encoding.UTF8, "application/json")
            };
            return Task.FromResult(response);
        }
    }
}
