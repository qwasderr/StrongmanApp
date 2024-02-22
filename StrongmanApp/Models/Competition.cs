using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class Competition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string CompScale { get; set; } = null!;

    public DateOnly Date { get; set; }

    public int TownId { get; set; }

    public int? FederationId { get; set; }

    //public int? VideoId { get; set; }
    public string? VideoUrl { get; set; }

    public virtual ICollection<CompetitionEvent> CompetitionEvents { get; set; } = new List<CompetitionEvent>();

    public virtual Federation? Federation { get; set; }

    public virtual ICollection<Lineup> Lineups { get; set; } = new List<Lineup>();

    public virtual Town Town { get; set; } = null!;

    //public virtual Video? Video { get; set; }
}
