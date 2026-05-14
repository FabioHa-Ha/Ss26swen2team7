using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    /// <summary>
    /// Error-handling technique demonstrated here: EXPLICIT TRY/CATCH with exception filtering.
    ///
    /// The weather endpoint depends on an external HTTP API that throws HttpRequestException.
    /// That exception is not a domain exception, so the GlobalExceptionHandler would produce a
    /// generic 500. Instead we handle it explicitly here to return more meaningful HTTP codes
    /// (404 for unknown city, 502 for other upstream failures).
    ///
    /// Key C# feature shown: `catch (T ex) when (condition)` — exception filter clause.
    /// The `when` predicate is evaluated before the stack unwinds, without catching the exception
    /// if the predicate is false. This avoids swallowing exceptions you don't intend to handle.
    ///
    /// Compare with:
    ///   ContactController  — global handler only (no try/catch)
    ///   TourController     — MVC exception filter attribute
    /// </summary>
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
        [ProducesResponseType(typeof(WeatherResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
        public async Task<ActionResult<WeatherResponseDto>> GetByCity(string city)
        {
            try
            {
                var weather = await _weatherService.GetCurrentWeatherByCityAsync(city);
                return Ok(weather);
            }
            // Exception filter clause (`when`): evaluated before the stack unwinds.
            // Only catches 404s; other HTTP errors propagate to the next catch block.
            catch (HttpRequestException ex) when (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
            {
                return NotFound(new ProblemDetails
                {
                    Status   = StatusCodes.Status404NotFound,
                    Title    = "City Not Found",
                    Detail   = $"City '{city}' was not found by the weather service.",
                    Instance = HttpContext.Request.Path
                });
            }
            catch (HttpRequestException ex)
            {
                return StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
                {
                    Status   = StatusCodes.Status502BadGateway,
                    Title    = "Weather Service Unavailable",
                    Detail   = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }

        /// <summary>
        /// Returns current weather for geographic coordinates (e.g. tour start/end point).
        /// </summary>
        [HttpGet]
        [ProducesResponseType(typeof(WeatherResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ProblemDetails), StatusCodes.Status502BadGateway)]
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
                return StatusCode(StatusCodes.Status502BadGateway, new ProblemDetails
                {
                    Status   = StatusCodes.Status502BadGateway,
                    Title    = "Weather Service Unavailable",
                    Detail   = ex.Message,
                    Instance = HttpContext.Request.Path
                });
            }
        }
    }
}
