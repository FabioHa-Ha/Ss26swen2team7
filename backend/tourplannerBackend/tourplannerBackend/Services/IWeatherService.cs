using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public interface IWeatherService
    {
        Task<ActionResult<WeatherResponseDto>> GetCurrentWeather(float lat, float lon);
    }
}
