using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class UserLogin
{
    public string LoginProvider { get; set; } = null!;

    public string ProviderKey { get; set; } = null!;

    public string? ProviderDisplayName { get; set; }

    //public string UserId { get; set; } = null!;
    public int UserId { get; set; }

    //public virtual AspNetUser User { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}
