﻿namespace library_manager_server.model;

public class Loan
{
    public required double LoanId { get; set; }
    public required double UserId { get; set; }
    public required Book Book { get; set; }
    public required DateTime Date { get; set; }
}