using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Menu
{
    public int Id { get; set; }

    public int CompanyId { get; set; }

    public string? MenuName { get; set; }

    public byte[]? Image { get; set; }

    public decimal TotalPrice { get; set; }

    public string? Detail { get; set; }

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<MenuProduct> MenuProducts { get; set; } = new List<MenuProduct>();
}
