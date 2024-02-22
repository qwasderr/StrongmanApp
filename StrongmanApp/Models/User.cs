﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace StrongmanApp.Models;

public partial class User
{
    public string Id { get; set; } = null!;
    [Display(Name = "Name")]
    [Required(ErrorMessage = "The field can't be empty")]
    public string Name { get; set; } = null!;
    [Display(Name = "Birth date")]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateOnly? BirthDate { get; set; }

    public string Email { get; set; } = null!;
    [Display(Name = "Are you planning to participate in the competition?")]
    public bool? IsContestant { get; set; }
    [Display(Name = "Age")]
    public int? Age { get; set; }
    [Display(Name = "Weight")]
    public int? Weight { get; set; }
    [Display(Name = "Height")]
    public int? Height { get; set; }

    public int? FirstCompYear { get; set; }

    public int? LastCompYear { get; set; }
    [Display(Name = "Country")]
    public int? CountryId { get; set; }
    [Display(Name = "Photo Url")]
    public string? PhotoUrl { get; set; }

    public string? IsAdmin { get; set; } = null!;
    [Display(Name = "Gender")]
    public string? Sex { get; set; } = null!;

    public string? SportCategory { get; set; }

    public DateOnly LastUpdate { get; set; }

    public bool? IsDeleted { get; set; }
    [ForeignKey("CountryId")]
    public virtual Country Country { get; set; } = null!;

    public virtual ICollection<Lineup> Lineups { get; set; } = new List<Lineup>();
}
