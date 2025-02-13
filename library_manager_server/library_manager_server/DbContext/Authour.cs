using System;
using System.Collections.Generic;

namespace library_manager_server;

public partial class Authour
{
    public long AuthourId { get; set; }

    public string? Authour1 { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
