using System;
using System.Collections.Generic;

namespace library_manager_server.ServerContext;

public partial class Loan
{
    public long LoanId { get; set; }

    public long UserId { get; set; }

    public string Isbn { get; set; } = null!;

    public DateOnly Date { get; set; }

    public virtual Book IsbnNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
