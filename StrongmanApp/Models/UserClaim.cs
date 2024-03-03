using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class UserClaim
{
    public int Id { get; set; }

    //public string UserId { get; set; } = null!;
    public int UserId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }

    // public virtual AspNetUser User { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
