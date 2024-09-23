using System;
using System.Collections.Generic;

namespace WebApi.Model;

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

    public virtual ICollection<Orderuserlist> Orderuserlists { get; set; } = new List<Orderuserlist>();

    public virtual ICollection<Userlist> Userlists { get; set; } = new List<Userlist>();

    public virtual Userrole? Userrole { get; set; }
}
