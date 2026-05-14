using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;
using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private const string BaseUrl = "https://api.openweathermap.org/data/2.5/weather";

        private static readonly JsonSerializerOptions JsonOptions = new()
        {
            PropertyNameCaseInsensitive = true
        };

        public WeatherService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _apiKey = configuration["OpenWeather:ApiKey"]
                ?? throw new InvalidOperationException("OpenWeather:ApiKey is not configured.");
        }

        public async Task<WeatherResponseDto> GetCurrentWeatherByCityAsync(string city)
        {
            var url = $"{BaseUrl}?q={Uri.EscapeDataString(city)}&appid={_apiKey}&units=metric";
            return await FetchWeatherAsync(url);
        }

        public async Task<WeatherResponseDto> GetCurrentWeatherByCoordinatesAsync(double lat, double lon)
        {
            var latStr = lat.ToString("G", CultureInfo.InvariantCulture);
            var lonStr = lon.ToString("G", CultureInfo.InvariantCulture);
            var url = $"{BaseUrl}?lat={latStr}&lon={lonStr}&appid={_apiKey}&units=metric";
            return await FetchWeatherAsync(url);
        }

        private async Task<WeatherResponseDto> FetchWeatherAsync(string url)
        {
            var response = await _httpClient.GetAsync(url);

            if (!response.IsSuccessStatusCode)
            {
                var errorBody = await response.Content.ReadAsStringAsync();
                throw new HttpRequestException(
                    $"OpenWeatherMap API returned {(int)response.StatusCode}: {errorBody}",
                    null,
                    response.StatusCode);
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<OpenWeatherResponse>(json, JsonOptions)
                ?? throw new InvalidOperationException("Failed to deserialize weather response.");

            return MapToDto(data);
        }

        private static WeatherResponseDto MapToDto(OpenWeatherResponse data) => new()
        {
            City = data.Name,
            Temperature = data.Main.Temp,
            FeelsLike = data.Main.FeelsLike,
            Humidity = data.Main.Humidity,
            Description = data.Weather.FirstOrDefault()?.Description ?? string.Empty,
            Icon = data.Weather.FirstOrDefault()?.Icon ?? string.Empty,
            WindSpeed = data.Wind.Speed
        };

        // Internal classes for OpenWeatherMap JSON deserialization
        private sealed class OpenWeatherResponse
        {
            public WeatherItem[] Weather { get; set; } = [];
            public MainData Main { get; set; } = new();
            public WindData Wind { get; set; } = new();
            public string Name { get; set; } = string.Empty;
        }

        private sealed class WeatherItem
        {
            public string Description { get; set; } = string.Empty;
            public string Icon { get; set; } = string.Empty;
        }

        private sealed class MainData
        {
            public double Temp { get; set; }

            [JsonPropertyName("feels_like")]
            public double FeelsLike { get; set; }

            public int Humidity { get; set; }
        }

        private sealed class WindData
        {
            public double Speed { get; set; }
        }
    }
}
