using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using tourplannerBackend.DTOs;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RouteController : ControllerBase
    {
        private readonly IRouteService _routeService;

        public RouteController(IRouteService routeService)
        {
            _routeService = routeService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(RouteResultDto), StatusCodes.Status200OK)]
        public async Task<ActionResult<RouteResultDto>> GetRoute(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] int transportTypeId)
        {
            // Presentation layer only delegates to the BL layer. External API access,
            // geocoding and parsing happen in RouteService. Failures surface as
            // ExternalServiceException and are mapped to 502 by the GlobalExceptionHandler.
            var route = await _routeService.GetRouteAsync(from, to, transportTypeId);
            return Ok(route);
        }
    }
}
