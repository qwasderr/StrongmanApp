using System;
using System.Collections.Generic;

namespace StrongmanApp.Models;

public partial class UserToken
{
    //public string UserId { get; set; } = null!;
    public int UserId { get; set; }

    public string LoginProvider { get; set; } = null!;

    public string Name { get; set; } = null!;

    public string? Value { get; set; }

    //public virtual AspNetUser User { get; set; } = null!;
    public virtual User User { get; set; } = null!;
}

