namespace tourplannerBackend.DTOs
{
    public class WeatherResponseDto
    {
        public required string place { get; set; }
        public double temperature { get; set; }
        public double feelsLikeTemperature { get; set; }
        public double windSpeed { get; set; }
        public double humidity { get; set; }
        public double uvIndex { get; set; }
        public double chanceOfRain { get; set; }
    }
}
