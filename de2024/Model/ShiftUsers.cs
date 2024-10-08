using System;
using System.Collections.Generic;

namespace de2024.Model;

public class ShiftUsers
{
    public int Shiftid { get; set; }

    public DateTime? Datestart { get; set; }

    public DateTime? Dateend { get; set; }

    public virtual List<User> Users { get; set; } = new List<User>();
}