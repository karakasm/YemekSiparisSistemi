using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Order
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int CompanyId { get; set; }

    public int PaymentType { get; set; }

    public int AddressId { get; set; }

    public decimal TotalPrice { get; set; }

    public byte[] CreatedAt { get; set; } = null!;

    public virtual Address Address { get; set; } = null!;

    public virtual Company Company { get; set; } = null!;

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<OrderProduct> OrderProducts { get; set; } = new List<OrderProduct>();

    public virtual Payment? Payment { get; set; }

    public virtual PaymentType PaymentTypeNavigation { get; set; } = null!;

    public virtual User User { get; set; } = null!;
}
