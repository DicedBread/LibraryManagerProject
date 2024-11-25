using library_manager_server.model;
using Microsoft.AspNetCore.Identity;

namespace library_manager_server
{
    public interface ILibraryManger
    {
        public List<Book> GetBooks(int limit, int offset);
        public Book? GetBook(string isbn);

        public PasswordVerificationResult AuthenticateUser(string username, string password);

    }
}
