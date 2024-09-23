using System;
using System.Collections.Generic;

namespace WebApi.ModelOver;

public partial class User
{
    public int Userid { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Status { get; set; }

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Middlename { get; set; }

    public int? Userroleid { get; set; }

    public Userrole? Userrole { get; set; }
}