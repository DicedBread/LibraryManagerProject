using library_manager_server.Model;
using Microsoft.AspNetCore.Identity;

namespace library_manager_server;

public interface ILibraryManager
{
    /// <summary>
    /// get book within range limit with offset 
    /// </summary>
    /// <param name="limit">number of books to retrive</param>
    /// <param name="offset">offset to start counting limit</param>
    /// <returns></returns>
    List<Model.Book> GetBooks(int limit, int offset);

    /// <summary>
    /// get book with isbn number 
    /// </summary>
    /// <param name="isbn">isbn of book to retrieve</param>
    /// <returns>book with isbn number or null if not book assosiated</returns>
    Model.Book? GetBook(string isbn);

    /// <summary>
    /// check if user has entered valid password
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <returns>pass or failed</returns>
    PasswordVerificationResult AuthenticateUser(string email, string password);

    /// <summary>
    /// inserts new user into the db
    /// </summary>
    /// <param name="email"></param>
    /// <param name="password"></param>
    /// <param name="username"></param>
    /// <returns>true if added successfully false if not</returns>
    bool AddUser(string email, string password, string username);

    double? GetUserId(string email);
    List<Model.Loan> GetLoans(double userId);
    Model.Loan? GetLoan(double loanId);

    /// <summary>
    /// create loan 
    /// </summary>
    /// <param name="isbn">loaned book</param>
    /// <param name="userId">user id</param>
    /// <param name="date"></param>
    /// <returns>true if succsessful otherwise false</returns>
    // bool CreateLoan(string isbn, double userId, DateTime date);
    
    /// <summary>
    /// create loan 
    /// </summary>
    /// <param name="isbn">loaned book</param>
    /// <param name="userId">user id</param>
    /// <param name="date">date of creation</param>
    /// <returns>loan if succsessful otherwise null</returns>
    Model.Loan? CreateLoan(string isbn, double userId, DateTime date);

    /// <summary>
    /// 
    /// </summary>
    /// <param name="loanId"></param>
    /// <returns></returns>
    bool DeleteLoan(double loanId);

    bool OwnsLoan(double loanId, double userId);

    /// <summary>
    /// is the book with isbn already loaned out
    /// </summary>
    /// <param name="isbn"></param>
    /// <returns></returns>
    bool HasActiveLoan(string isbn);

    /// <summary>
    /// search books 
    /// </summary>
    /// <param name="search"></param>
    /// <param name="limit"></param>
    /// <param name="offset"></param>
    /// <returns></returns>
    List<Model.Book> SearchBooks(string search, int limit, int offset);
}