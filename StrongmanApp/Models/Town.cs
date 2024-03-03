using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class Town
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;
    
    public int CountryId { get; set; }

    public string? Details { get; set; }
    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();
    [Display(Name = "Country Name")]
    public virtual Country Country { get; set; } = null!;
}
