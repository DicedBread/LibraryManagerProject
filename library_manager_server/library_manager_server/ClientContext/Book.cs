namespace library_manager_server.ClientContext
{
    public class Book
    {
        public required string Isbn { get; set; }
        public required string Title { get; set; }
        public required string Authour { get; set; }
        public required string Publisher { get; set; }
        public required string ImgUrl { get; set; }

        // public Book(ServerContext.Book book){
        //     Isbn = book.Isbn;
        //     Title = book.Title;
        //     Authour = book.Authour.Name;
        //     Publisher = book.Publisher.Name;
        //     ImgUrl = book.ImgUrl;
        // }    
    }
}
