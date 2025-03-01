using System;
using System.Collections.Generic;
using NpgsqlTypes;

namespace library_manager_server.ServerContext;

public partial class Book
{
    public string Isbn { get; set; } = null!;

    public string Title { get; set; } = null!;

    public string ImgUrl { get; set; } = null!;

    public long AuthorId { get; set; }

    public long PublisherId { get; set; }

    public NpgsqlTsVector? TextSearch { get; set; }

    public virtual Author Author { get; set; } = null!;

    public virtual ICollection<Loan> Loans { get; set; } = new List<Loan>();

    public virtual Publisher Publisher { get; set; } = null!;
}
