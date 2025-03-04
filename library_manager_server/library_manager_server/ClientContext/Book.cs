namespace library_manager_server.ClientContext
{
    public class Book
    {
        public string Isbn { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Publisher { get; set; }
        public long NumAvailable { get; set; }
        public string ImgUrl { get; set; }

        public Book(ServerContext.Book book){
            Isbn = book.Isbn;
            Title = book.Title;
            Author = book.Author.Name;
            Publisher = book.Publisher.Name;
            NumAvailable = book.NumAvailable;
            ImgUrl = book.ImgUrl;
        }    

        public Book(){}
    }
}
