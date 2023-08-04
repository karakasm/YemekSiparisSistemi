using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class MenuProduct
{
    public int MenuId { get; set; }

    public int ProductId { get; set; }

    public byte Quantity { get; set; }

    public decimal UnitPrice { get; set; }

    public virtual Menu Menu { get; set; } = null!;

    public virtual Product Product { get; set; } = null!;
}
