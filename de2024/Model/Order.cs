using System;
using System.Collections.Generic;

namespace de2024.Model;

public class Order
{
    public int Orderid { get; set; }

    public DateTime Datecreation { get; set; }

    public string Orderstatus { get; set; } = null!;

    public string Nameconference { get; set; } = null!;

    public int Amountguests { get; set; }

    public string Equipment { get; set; } = null!;

    public string Paymentstatus { get; set; } = null!;
}