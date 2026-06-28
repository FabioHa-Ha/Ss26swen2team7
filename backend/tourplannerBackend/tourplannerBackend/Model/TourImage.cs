namespace tourplannerBackend.Model
{
    public class TourImage
    {
        public int Id { get; set; }
        public required Tour Tour {  get; set; }
        public required byte[] Image {  get; set; }
        public required string FileName { get; set; }
        public required string ContentType { get; set; }
    }
}
