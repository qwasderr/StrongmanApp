using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class Federation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    [Display(Name = "Number of Contests")]
    public int NumberofContests { get; set; }
    [Display(Name = "First Year Held")]
    public int? FirstYearHeld { get; set; }
    [Display(Name = "Last Year Held")]
    public int? LastYearHeld { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();
}
