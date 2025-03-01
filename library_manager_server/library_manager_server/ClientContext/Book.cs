namespace library_manager_server.ClientContext
{
    public class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Authour { get; set; }
        public string Publisher { get; set; }
        public string ImgUrl { get; set; }

        public Book(ServerContext.Book book){
            Isbn = book.Isbn;
            Title = book.Title;
            Authour = book.Authour.Name;
            Publisher = book.Publisher.Name;
            ImgUrl = book.ImgUrl;
        }    

        public Book(){}
    }
}
