using System;
using System.Collections.Generic;

namespace WebApi.Model;

public partial class Userlist
{
    public int Userlistid { get; set; }

    public int Userid { get; set; }

    public int Shiftid { get; set; }

    public virtual Shift Shift { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
