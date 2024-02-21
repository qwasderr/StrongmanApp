using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class Video
{
    public int Id { get; set; }

    public string Url { get; set; } = null!;

    public string? Details { get; set; }

    public virtual ICollection<Competition> Competitions { get; set; } = new List<Competition>();

    public virtual ICollection<EventVideo> EventVideos { get; set; } = new List<EventVideo>();

    public virtual ICollection<News> News { get; set; } = new List<News>();
}
