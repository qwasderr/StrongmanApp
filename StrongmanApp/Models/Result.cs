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
    [Display(Name = "Result")]
    public string? Result1 { get; set; }

    public int? Place { get; set; }

    public float? Points { get; set; }
    [Display(Name = "Competition Name")]
    public virtual Competition Competition { get; set; } = null!;
    [Display(Name = "Event Name")]
    public virtual Event Event { get; set; } = null!;
    [Display(Name = "User Name")]
    public virtual User User { get; set; } = null!;
}
