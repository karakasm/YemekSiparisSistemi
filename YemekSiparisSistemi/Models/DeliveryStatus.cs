using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class DeliveryStatus
{
    public int Id { get; set; }

    public string? StatusName { get; set; }

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();
}
