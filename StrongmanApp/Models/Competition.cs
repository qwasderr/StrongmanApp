using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class Competition
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Division { get; set; } = null!;

    public string CompScale { get; set; } = null!;
    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateOnly Date { get; set; }

    public int TownId { get; set; }

    public int? FederationId { get; set; }

    public string? VideoUrl { get; set; }

    public virtual ICollection<CompetitionEvent> CompetitionEvents { get; set; } = new List<CompetitionEvent>();

    public virtual Federation? Federation { get; set; }

    public virtual ICollection<Lineup> Lineups { get; set; } = new List<Lineup>();

    public virtual Town Town { get; set; } = null!;
}
