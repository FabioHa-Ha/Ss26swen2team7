using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    [ApiController]
    [Route("api")]
    [Authorize]
    public class WeatherController : ControllerBase
    {
        private readonly IWeatherService _newWeatherService;

        public WeatherController(IWeatherService newWeatherService)
        {
            _newWeatherService = newWeatherService;
        }

        [HttpGet("currentWeather/{lat}/{lon}")]
        public async Task<ActionResult<WeatherResponseDto>> GetCurrentWeather(float lat, float lon)
        {
            try
            {
                return await _newWeatherService.GetCurrentWeather(lat, lon);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
