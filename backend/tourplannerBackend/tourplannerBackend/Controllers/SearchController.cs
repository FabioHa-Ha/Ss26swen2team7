using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using tourplannerBackend.Services;

namespace tourplannerBackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize]
    public class SearchController : ControllerBase
    {
        private readonly ITourService _tourService;
        private readonly ITourLogService _tourLogService;

        public SearchController(ITourService tourService, ITourLogService tourLogService)
        {
            _tourService = tourService;
            _tourLogService = tourLogService;
        }

        [HttpGet]
        public async Task<IActionResult> Search([FromQuery] string query)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            if (string.IsNullOrWhiteSpace(query))
                return Ok(new { tours = new List<object>(), logs = new List<object>() });

            var term = query.ToLower();
            var tours = (await _tourService.GetByUserIdAsync(userId)).ToList();
            var logs  = (await _tourLogService.GetByUserIdAsync(userId)).ToList();

            var matchedTours = tours
                .Select(t =>
                {
                    var matched = new List<string>();

                    if (t.Name.ToLower().Contains(term))                          matched.Add("name");
                    if (t.Description?.ToLower().Contains(term) == true)          matched.Add("description");
                    if (t.FromLocation.ToLower().Contains(term))                  matched.Add("from");
                    if (t.ToLocation.ToLower().Contains(term))                    matched.Add("to");
                    if (t.TransportTypeName?.ToLower().Contains(term) == true)    matched.Add("transportType");
                    if (t.RouteInformation?.ToLower().Contains(term) == true)     matched.Add("routeInformation");
                    if (t.Popularity.ToString().Contains(term))                   matched.Add("popularity");
                    if (t.ChildFriendliness.ToString().Contains(term))            matched.Add("childFriendliness");

                    return new { tour = t, matched };
                })
                .Where(x => x.matched.Count > 0)
                .Select(x => new
                {
                    x.tour.Id,
                    x.tour.Name,
                    x.tour.FromLocation,
                    x.tour.ToLocation,
                    x.tour.TransportTypeName,
                    x.tour.Distance,
                    x.tour.EstimatedTime,
                    x.tour.Description,
                    x.tour.Popularity,
                    x.tour.ChildFriendliness,
                    x.tour.AverageRating,
                    x.tour.TotalLogs,
                    matchedFields = x.matched
                }).ToList();

            var matchedLogs = logs
                .Where(l =>
                {
                    var tourName = tours.FirstOrDefault(t => t.Id == l.TourId)?.Name;
                    return l.Comment?.ToLower().Contains(term) == true
                        || l.Rating.ToString().Contains(term)
                        || l.DifficultyName?.ToLower().Contains(term) == true
                        || tourName?.ToLower().Contains(term) == true;
                })
                .Select(l => new
                {
                    l.Id,
                    l.TourId,
                    TourName = tours.FirstOrDefault(t => t.Id == l.TourId)?.Name,
                    l.Date,
                    l.Comment,
                    l.TotalDistance,
                    l.TotalTime,
                    l.Rating,
                    l.DifficultyName,
                    matchedFields = new List<string> { "log" }
                }).ToList();

            return Ok(new { tours = matchedTours, logs = matchedLogs });
        }
    }
}
