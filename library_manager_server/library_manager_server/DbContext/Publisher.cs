using System;
using System.Collections.Generic;

namespace library_manager_server;

public partial class Publisher
{
    public long PublisherId { get; set; }

    public string? Publisher1 { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
