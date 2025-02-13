using System;
using System.Collections.Generic;

namespace library_manager_server;

public partial class User
{
    public long UserId { get; set; }

    public string Email { get; set; } = null!;

    public string Username { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();
}
