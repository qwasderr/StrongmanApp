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
    [Display(Name = "Competition Scale")]
    public string CompScale { get; set; } = null!;
    [Display(Name = "Date")]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateOnly Date { get; set; }

    public int TownId { get; set; }

    public int? FederationId { get; set; }
    [Display(Name = "Video")]
    public string? VideoUrl { get; set; }

    public virtual ICollection<CompetitionEvent> CompetitionEvents { get; set; } = new List<CompetitionEvent>();
    [Display(Name = "Federation Name")]
    public virtual Federation? Federation { get; set; }

    public virtual ICollection<Lineup> Lineups { get; set; } = new List<Lineup>();
    [Display(Name = "Town Name")]
    public virtual Town Town { get; set; } = null!;
}
