using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Numerics;
using System.Text.Json;
using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public class WeatherService : IWeatherService
    {
        private readonly IConfiguration _configuration;
        
        private const string SECTION_NAME = "WeatherAPI";
        private const string URL_NAME = "BaseURL";
        private const string KEY_NAME = "ApiKey";

        public WeatherService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<ActionResult<WeatherResponseDto>> GetCurrentWeather(float lat, float lon)
        {
            IConfigurationSection section = _configuration.GetSection(SECTION_NAME);
            var customCulture = (CultureInfo)CultureInfo.InvariantCulture.Clone();
            customCulture.NumberFormat.NumberDecimalSeparator = ".";
            string url = section.GetValue<string>(URL_NAME) + "key=" + section.GetValue<string>(KEY_NAME) + "&q=" + lat.ToString(customCulture) + "," + lon.ToString(customCulture);
            HttpClient client = new HttpClient();
            HttpResponseMessage response = await client.GetAsync(url);
            if (response.IsSuccessStatusCode)
            {
                using JsonDocument doc = await JsonDocument.ParseAsync(await response.Content.ReadAsStreamAsync());
                JsonElement root = doc.RootElement;
                JsonElement location = root.GetProperty("location");
                JsonElement current = root.GetProperty("current");
                string locationName = location.GetProperty("name").GetString() ?? "";
                WeatherResponseDto newWeatherResponseDto = new WeatherResponseDto
                {
                    place = locationName,
                    temperature = current.GetProperty("temp_c").GetDouble(),
                    feelsLikeTemperature = current.GetProperty("temp_c").GetDouble(),
                    humidity = current.GetProperty("humidity").GetDouble(),
                    windSpeed = current.GetProperty("wind_kph").GetDouble(),
                    uvIndex = current.GetProperty("uv").GetDouble(),
                    chanceOfRain = current.GetProperty("chance_of_rain").GetDouble()
                };
                return newWeatherResponseDto;
            }
            else
            {
                throw new InvalidOperationException("An unexpected error occurred while fetching weather data");
            }
        }
    }
}
