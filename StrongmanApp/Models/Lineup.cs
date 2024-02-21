using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class Lineup
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    public string UserId { get; set; } = null!;

    public string? Details { get; set; }

    public int IsConfirmed { get; set; }

    public DateOnly? RegistrationDate { get; set; }

    public int? Place { get; set; }

    public float? Points { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
