using System;
using System.Collections.Generic;

namespace WebApi.Model;

public partial class Shift
{
    public int Shiftid { get; set; }

    public DateTime? Datestart { get; set; }

    public DateTime? Dateend { get; set; }

    public virtual ICollection<Userlist> Userlists { get; set; } = new List<Userlist>();
}
