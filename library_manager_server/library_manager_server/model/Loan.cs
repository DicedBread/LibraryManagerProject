namespace library_manager_server.model;

public class Loan
{
    public required double Loan_id { get; set; }
    // public required double User_id { get; set; }
    public required string Isbn { get; set; }
    public required DateTime Date { get; set; }
}