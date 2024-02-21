using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class Result
{
    public int EventId { get; set; }

    public int CompetitionId { get; set; }

    public string UserId { get; set; } = null!;

    public string? Result1 { get; set; }

    public int? Place { get; set; }

    public float? Points { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
