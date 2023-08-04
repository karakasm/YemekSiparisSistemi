using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Courier
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public string Name { get; set; } = null!;

    public string? Surname { get; set; }

    public string Phone { get; set; } = null!;

    public virtual Company? Company { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}
