namespace library_manager_server.model
{
    public class Book
    {
        public required string Id { get; set; }
        public required string Title { get; set; }
        public required string Authour { get; set; }
        public required string Publisher { get; set; }
        public required string ImgUrl { get; set; }
    }
}
