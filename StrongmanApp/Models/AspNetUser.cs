using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace StrongmanApp.Models;

public partial class AspNetUser
{
    //public string Id { get; set; } = null!;
    public int Id { get; set; }

    public string? UserName { get; set; }

    public string? NormalizedUserName { get; set; }

    public string? Email { get; set; }

    public string? NormalizedEmail { get; set; }

    public bool EmailConfirmed { get; set; }

    public string? PasswordHash { get; set; }

    public string? SecurityStamp { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public string? PhoneNumber { get; set; }

    public bool PhoneNumberConfirmed { get; set; }

    public bool TwoFactorEnabled { get; set; }

    public DateTimeOffset? LockoutEnd { get; set; }

    public bool LockoutEnabled { get; set; }

    public int AccessFailedCount { get; set; }

    public virtual ICollection<AspNetUserClaim> AspNetUserClaims { get; set; } = new List<AspNetUserClaim>();

    public virtual ICollection<AspNetUserLogin> AspNetUserLogins { get; set; } = new List<AspNetUserLogin>();

    public virtual ICollection<AspNetUserToken> AspNetUserTokens { get; set; } = new List<AspNetUserToken>();

    public virtual ICollection<AspNetRole> Roles { get; set; } = new List<AspNetRole>();



    [Display(Name = "Name")]
    [Required(ErrorMessage = "The field can't be empty")]
    public string Name { get; set; } = null!;
    [Display(Name = "Birth date")]
    [DataType(DataType.Date)]
    [Column(TypeName = "Date")]
    public DateOnly? BirthDate { get; set; }

    public string? Email2 { get; set; }
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
