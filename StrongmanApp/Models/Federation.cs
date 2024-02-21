using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class Federation
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int NumberofContests { get; set; }

    public int? FirstYearHeld { get; set; }

    public int? LastYearHeld { get; set; }

    public bool? IsDeleted { get; set; }

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();
}
