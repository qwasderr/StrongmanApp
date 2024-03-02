using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class Result
{
    [Key]
    public int Id {  get; set; }
    public int EventId { get; set; }

    public int CompetitionId { get; set; }

    //public string UserId { get; set; } = null!;
    public int UserId {  get; set; }

    public string? Result1 { get; set; }

    public int? Place { get; set; }

    public float? Points { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual Event Event { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
