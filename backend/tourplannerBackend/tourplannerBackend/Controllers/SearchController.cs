using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Security.Claims;
using tourplannerBackend.Model;
using tourplannerBackend.Repositories;

namespace tourplannerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly ITourRepository _tourRepository;
        private readonly ITourLogRepository _tourLogRepository;

        public SearchController(ITourRepository tourRepository, ITourLogRepository tourLogRepository)
        {
            _tourRepository = tourRepository;
            _tourLogRepository = tourLogRepository;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (string.IsNullOrWhiteSpace(query))
            {
                return Ok(new { tours = new List<object>(), logs = new List<object>() });
            }

            var term = query.ToLower();
            var tours = await _tourRepository.GetByUserIdAsync(userId);
            var logs = await _tourLogRepository.GetByUserIdAsync(userId);

            var logsByTour = logs.GroupBy(l => l.Tour.Id).ToDictionary(g => g.Key, g => g.ToList());

            var matchedTours = tours
                .Select(t =>
                {
                    var tourLogs = logsByTour.GetValueOrDefault(t.Id, []);
                    var popularity = tourLogs.Count;
                    var childFriendliness = ComputeChildFriendliness(tourLogs);

                    var matched = new List<string>();

                    if (t.Name.ToLower().Contains(term))
                    {
                        matched.Add("name");
                    }

                    if (t.Description?.ToLower().Contains(term) == true)
                    {
                        matched.Add("description");
                    }

                    if (t.FromLocation.ToLower().Contains(term))
                    {
                        matched.Add("from");
                    }

                    if (t.ToLocation.ToLower().Contains(term))
                    {
                        matched.Add("to");
                    }

                    if (t.TransportType.Name.ToLower().Contains(term))
                    {
                        matched.Add("transportType");
                    }

                    if (t.RouteInformation?.ToLower().Contains(term) == true)
                    {
                        matched.Add("routeInformation");
                    }

                    if (popularity.ToString().Contains(term))
                    {
                        matched.Add("popularity");
                    }

                    if (childFriendliness.ToString().Contains(term))
                    {
                        matched.Add("childFriendliness");
                    }

                    return new { tour = t, matched, popularity, childFriendliness };
                })
                .Where(x => x.matched.Count > 0)
                .Select(x => new
                {
                    x.tour.Id,
                    x.tour.Name,
                    x.tour.FromLocation,
                    x.tour.ToLocation,
                    TransportTypeName = x.tour.TransportType.Name,
                    x.tour.Distance,
                    x.tour.EstimatedTime,
                    x.tour.Description,
                    x.popularity,
                    x.childFriendliness,
                    matchedFields = x.matched
                }).ToList();

            var matchedLogs = logs
                .Where(l =>
                {
                    var tour = tours.FirstOrDefault(t => t.Id == l.Tour.Id);
                    return l.Comment?.ToLower().Contains(term) == true
                        || l.Rating.ToString().Contains(term)
                        || l.Difficulty.Name.ToLower().Contains(term)
                        || tour?.Name.ToLower().Contains(term) == true;
                })
                .Select(l => new
                {
                    l.Id,
                    l.Tour,
                    TourName = tours.FirstOrDefault(t => t.Id == l.Tour.Id)?.Name,
                    l.Date,
                    l.Comment,
                    l.TotalDistance,
                    l.TotalTime,
                    l.Rating,
                    l.Difficulty,
                    matchedFields = new List<string> { "log" }
                }).ToList();

            return Ok(new { tours = matchedTours, logs = matchedLogs });
        }

        private static int ComputeChildFriendliness(List<TourLog> logs)
        {
            if (!logs.Any())
            {
                return 0;
            }

            var avgDifficulty = logs.Average(l => l.Difficulty.Id);
            var avgDistance = logs.Average(l => l.TotalDistance);

            var score = 5 - (int)Math.Round((avgDifficulty - 1) / 4 * 4);
            return Math.Clamp(score, 1, 5);
        }
    }
}
