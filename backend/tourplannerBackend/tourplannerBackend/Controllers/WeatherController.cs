using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _weatherService;

        public WeatherController(IWeatherService weatherService)
        {
            _weatherService = weatherService;
        }

        /// <summary>
        /// Returns current weather for a given city name.
        /// </summary>
        [HttpGet("{city}")]
        public async Task<ActionResult<WeatherResponseDto>> GetByCity(string city)
        {
            try
            {
                var weather = await _weatherService.GetCurrentWeatherByCityAsync(city);
                return Ok(weather);
            }
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound($"City '{city}' not found.");
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, $"Weather service error: {ex.Message}");
            }
        }

        /// <summary>
        /// Returns current weather for geographic coordinates (e.g. tour start/end point).
        /// </summary>
        [HttpGet]
        public async Task<ActionResult<WeatherResponseDto>> GetByCoordinates(
            [FromQuery] double lat,
            [FromQuery] double lon)
        {
            try
            {
                var weather = await _weatherService.GetCurrentWeatherByCoordinatesAsync(lat, lon);
                return Ok(weather);
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(502, $"Weather service error: {ex.Message}");
            }
        }
    }
}
