using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class Country
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string? Details { get; set; }
    [Display(Name = "Flag")]
    public string? FlagUrl { get; set; }

    public virtual ICollection<Town> Towns { get; set; } = new List<Town>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
