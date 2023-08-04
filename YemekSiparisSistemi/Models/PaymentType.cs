using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class PaymentType
{
    public int Id { get; set; }

    public string? PaymentName { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
