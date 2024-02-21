using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class EventVideo
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int VideoId { get; set; }

    public string? Description { get; set; }

    public virtual Event Event { get; set; } = null!;

    public virtual Video Video { get; set; } = null!;
}
