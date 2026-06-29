using tourplannerBackend.DTOs;

namespace tourplannerBackend.Services
{
    public interface IRouteService
    {
        /// <summary>
        /// Geocodes the two locations and queries the OpenRouteService Directions API
        /// for the route matching the given transport type.
        /// Throws ExternalServiceException when the upstream service fails or a location
        /// cannot be geocoded.
        /// </summary>
        Task<RouteResultDto> GetRouteAsync(string from, string to, int transportTypeId);
    }
}
