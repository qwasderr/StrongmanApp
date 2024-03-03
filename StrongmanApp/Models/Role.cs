using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
namespace StrongmanApp.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string? NormalizedName { get; set; }

    public string? ConcurrencyStamp { get; set; }

    public virtual ICollection<RoleClaim> AspNetRoleClaims { get; set; } = new List<RoleClaim>();

    //public virtual ICollection<AspNetUser> Users { get; set; } = new List<AspNetUser>();
    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
