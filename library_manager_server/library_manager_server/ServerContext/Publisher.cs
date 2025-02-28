using System;
using System.Collections.Generic;

namespace library_manager_server.ServerContext;

public partial class Publisher
{
    public long PublisherId { get; set; }

    public string? Name { get; set; }

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
