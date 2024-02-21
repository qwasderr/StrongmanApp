using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class CompetitionEvent
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    public int EventId { get; set; }

    public string? Details { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;
}
