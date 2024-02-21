using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class News
{
    public int Id { get; set; }

    public int? VideoId { get; set; }

    public string NewsContent { get; set; } = null!;

    public DateOnly DateModified { get; set; }

    public virtual Video? Video { get; set; }
}
