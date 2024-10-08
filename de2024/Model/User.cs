using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace de2024.Model;

public class User{
    public int Userid { get; set; }

    public string Login { get; set; } = null!;

    public string Password { get; set; } = null!;

    public string? Status { get; set; }

    public string Lastname { get; set; } = null!;

    public string Firstname { get; set; } = null!;

    public string? Middlename { get; set; }

    public int? Userroleid { get; set; }
    
    public virtual Userrole? Userrole { get; set; }
    
    public virtual ICollection<Userlist> Userlists { get; set; } = new List<Userlist>();

    public override string ToString()
    {
        return $"{Firstname} {Lastname} {Middlename}";
    }
}