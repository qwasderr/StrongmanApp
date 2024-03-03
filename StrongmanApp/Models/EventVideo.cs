using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class EventVideo
{
    public int Id { get; set; }

    public int EventId { get; set; }

    public int VideoId { get; set; }

    public string? Description { get; set; }
    [Display(Name = "Event Name")]
    public virtual Event Event { get; set; } = null!;
    [Display(Name = "Video")]
    public virtual Video Video { get; set; } = null!;
}
