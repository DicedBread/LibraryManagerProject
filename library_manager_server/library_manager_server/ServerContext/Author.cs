using System;
using System.Collections.Generic;

namespace library_manager_server.ServerContext;

public partial class Author
{
    public long AuthorId { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Book> Books { get; set; } = new List<Book>();
}
