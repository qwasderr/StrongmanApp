using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class CompetitionEvent
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    public int EventId { get; set; }

    public string? Details { get; set; }
    [Display(Name = "Competition Name")]
    public virtual Competition Competition { get; set; } = null!;
    [Display(Name = "Event Name")]
    public virtual Event Event { get; set; } = null!;
}
