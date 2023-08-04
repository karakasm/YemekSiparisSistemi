using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Product
{
    public int Id { get; set; }

    public int? CategoryId { get; set; }

    public int? CompanyId { get; set; }

    public string ProductName { get; set; } = null!;

    public decimal UnitPrice { get; set; }

    public byte[] ProductImage { get; set; } = null!;

    public string? Content { get; set; }

    public virtual Category? Category { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<MenuProduct> MenuProducts { get; set; } = new List<MenuProduct>();

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();
}
