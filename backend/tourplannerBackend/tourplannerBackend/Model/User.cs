namespace tourplannerBackend.Model
{
    public class User
    {
        public required int Id { get; set; }
        public required string Username { get; set; }
        public required string Password { get; set; }
        public string? Email { get; set; }
    }
}
