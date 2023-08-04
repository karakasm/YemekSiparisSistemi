using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Category
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public string? CategoryName { get; set; }

    public virtual Company? Company { get; set; }

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
