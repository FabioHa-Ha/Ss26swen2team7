using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using tourplannerBackend.DTOs;
using tourplannerBackend.Exceptions;

namespace tourplannerBackend.Services
{
    /// <summary>
    /// Business-Logic layer for tour routing.
    ///
    /// Layering note (NFR "layers only call the immediate layer below"):
    /// All OpenRouteService HTTP communication, geocoding and JSON parsing live here in the
    /// BL layer. The RouteController (presentation layer) only calls this service and no longer
    /// talks to HttpClient directly. Upstream failures are translated into the BL's own
    /// ExternalServiceException so no implementation-specific exception leaks to the controller.
    /// </summary>
    public class RouteService : IRouteService
    {
        private const string ServiceName = "OpenRouteService";

        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        private readonly ILogger<RouteService> _logger;

        public RouteService(HttpClient httpClient, IConfiguration config, ILogger<RouteService> logger)
        {
            _httpClient = httpClient;
            _config = config;
            _logger = logger;
        }

        public async Task<RouteResultDto> GetRouteAsync(string from, string to, int transportTypeId)
        {
            var coords = await GeocodeLocationsAsync(from, to)
                ?? throw new ExternalServiceException(ServiceName,
                    "Geocoding failed — one or both locations could not be resolved.");

            var profile = transportTypeId switch
            {
                1 => "cycling-regular",
                2 => "foot-hiking",
                3 => "foot-walking",
                _ => "driving-car"
            };

            var apiKey = _config["OpenRouteService:ApiKey"]?.Trim();
            var body = JsonSerializer.Serialize(new { coordinates = coords });
            var request = new HttpRequestMessage(HttpMethod.Post,
                $"https://api.openrouteservice.org/v2/directions/{profile}")
            {
                Content = new StringContent(body, Encoding.UTF8, "application/json")
            };
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

            var response = await _httpClient.SendAsync(request);
            if (!response.IsSuccessStatusCode)
            {
                _logger.LogWarning("ORS directions request failed: {StatusCode}", response.StatusCode);
                throw new ExternalServiceException(ServiceName,
                    $"Directions request failed with status {(int)response.StatusCode}.");
            }

            var json = await response.Content.ReadAsStringAsync();
            try
            {
                var data = JsonSerializer.Deserialize<JsonElement>(json);
                var summary = data.GetProperty("routes")[0].GetProperty("summary");

                return new RouteResultDto
                {
                    DistanceKm      = Math.Round(summary.GetProperty("distance").GetDouble() / 1000),
                    DurationMinutes = Math.Round(summary.GetProperty("duration").GetDouble() / 60)
                };
            }
            catch (Exception ex) when (ex is KeyNotFoundException or InvalidOperationException or JsonException)
            {
                _logger.LogError(ex, "Failed to parse ORS directions response.");
                throw new ExternalServiceException(ServiceName, "Could not parse the directions response.");
            }
        }

        private async Task<double[][]?> GeocodeLocationsAsync(string from, string to)
        {
            var fromCoords = await GeocodeAsync(from);
            await Task.Delay(1100); // ORS free tier rate limit
            var toCoords = await GeocodeAsync(to);

            if (fromCoords == null || toCoords == null)
                return null;

            return [fromCoords, toCoords];
        }

        private async Task<double[]?> GeocodeAsync(string location)
        {
            var apiKey = _config["OpenRouteService:ApiKey"]?.Trim();
            var url = $"https://api.openrouteservice.org/geocode/search?text={Uri.EscapeDataString(location)}&size=1";

            var req = new HttpRequestMessage(HttpMethod.Get, url);
            req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
            req.Headers.Add("User-Agent", "TourPlanner/1.0");

            var res = await _httpClient.SendAsync(req);
            if (!res.IsSuccessStatusCode)
            {
                _logger.LogWarning("ORS geocoding failed for '{Location}': {StatusCode}", location, res.StatusCode);
                return null;
            }

            var body = await res.Content.ReadAsStringAsync();
            try
            {
                var json = JsonSerializer.Deserialize<JsonElement>(body);
                var features = json.GetProperty("features");
                if (features.GetArrayLength() == 0)
                    return null;

                var coordinates = features[0].GetProperty("geometry").GetProperty("coordinates");
                return new[]
                {
                    coordinates[0].GetDouble(), // lon
                    coordinates[1].GetDouble()  // lat
                };
            }
            catch (Exception ex) when (ex is KeyNotFoundException or InvalidOperationException or JsonException)
            {
                _logger.LogError(ex, "Failed to parse ORS geocoding response for '{Location}'.", location);
                return null;
            }
        }
    }
}
