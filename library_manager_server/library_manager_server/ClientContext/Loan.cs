namespace library_manager_server.ClientContext;

public class Loan
{
    public Loan(ServerContext.Loan loan)
    {
        LoanId = loan.LoanId;
        UserId = loan.UserId;
        Book = new ClientContext.Book(loan.IsbnNavigation);
        Date = loan.Date;
    }

    public Loan(){}

    public long LoanId { get; set; }
    public long UserId { get; set; }
    public Book Book { get; set; }
    public DateOnly Date { get; set; }

    
}