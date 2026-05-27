using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace tourplannerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class RouteController : ControllerBase
    {
        private readonly IConfiguration _config;
        private readonly HttpClient _httpClient;

        public RouteController(IConfiguration config, IHttpClientFactory httpClientFactory)
        {
            _config = config;
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet]
        public async Task<IActionResult> GetRoute(
            [FromQuery] string from,
            [FromQuery] string to,
            [FromQuery] int transportTypeId)
        {
            var coords = await GeocodeLocations(from, to);
            if (coords == null)
            {
                return StatusCode(503, "Geocoding service unavailable.");
            }

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
                return StatusCode((int)response.StatusCode, "ORS request failed.");
            }

            var json = await response.Content.ReadAsStringAsync();
            var data = JsonSerializer.Deserialize<JsonElement>(json);
            var summary = data.GetProperty("routes")[0].GetProperty("summary");

            return Ok(new
            {
                distanceKm = Math.Round(summary.GetProperty("distance").GetDouble() / 1000),
                durationMinutes = Math.Round(summary.GetProperty("duration").GetDouble() / 60)
            });
        }

        private async Task<double[][]?> GeocodeLocations(string from, string to)
        {
            async Task<double[]?> Geocode(string location)
            {
                var apiKey = _config["OpenRouteService:ApiKey"]?.Trim();

                var url = $"https://api.openrouteservice.org/geocode/search?text={Uri.EscapeDataString(location)}&size=1";

                var req = new HttpRequestMessage(HttpMethod.Get, url);

                req.Headers.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);
                req.Headers.Add("User-Agent", "TourPlanner/1.0");

                var res = await _httpClient.SendAsync(req);

                if (!res.IsSuccessStatusCode)
                {
                    Console.WriteLine($"ORS Geocoding failed: {res.StatusCode}");
                    return null;
                }


                var body = await res.Content.ReadAsStringAsync();

                Console.WriteLine($"ORS Geocoding response for '{location}': {body}");

                try
                {
                    var json = JsonSerializer.Deserialize<JsonElement>(body);

                    var features = json.GetProperty("features");

                    if (features.GetArrayLength() == 0)
                    {
                        return null;
                    }

                    var coordinates = features[0]
                        .GetProperty("geometry")
                        .GetProperty("coordinates");

                    return new[]
                    {
                        coordinates[0].GetDouble(), // lon
                        coordinates[1].GetDouble()  // lat
                    };
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Failed to parse ORS geocoding JSON: {ex.Message}");
                    return null;
                }
            }

            var fromCoords = await Geocode(from);
            await Task.Delay(1100);
            var toCoords = await Geocode(to);
            if (fromCoords == null || toCoords == null)
            {
                return null;
            }
            return [fromCoords, toCoords];
        }
    }
}
