namespace tourplannerBackend.DTOs
{
    /// <summary>
    /// Result of a route lookup: total distance and duration of the computed tour.
    /// Returned by the BL layer (IRouteService) to the presentation layer (RouteController).
    /// </summary>
    public class RouteResultDto
    {
        public double DistanceKm { get; set; }
        public double DurationMinutes { get; set; }
    }
}
