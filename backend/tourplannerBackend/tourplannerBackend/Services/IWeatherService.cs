using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public interface IWeatherService
    {
        Task<WeatherResponseDto> GetCurrentWeatherByCityAsync(string city);
        Task<WeatherResponseDto> GetCurrentWeatherByCoordinatesAsync(double lat, double lon);
    }
}
