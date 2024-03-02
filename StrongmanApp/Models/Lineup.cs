using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class Lineup
{
    public int Id { get; set; }

    public int CompetitionId { get; set; }

    //public string UserId { get; set; } = null!;
    public int UserId {  get; set; }
    public string? Details { get; set; }

    public int IsConfirmed { get; set; }
    [Display(Name = "Registration date")]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateOnly? RegistrationDate { get; set; }

    public int? Place { get; set; }

    public float? Points { get; set; }

    public virtual Competition Competition { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
