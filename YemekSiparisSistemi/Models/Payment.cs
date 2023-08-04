using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Payment
{
    public int OrderId { get; set; }

    public int? PaymentType { get; set; }

    public int UserId { get; set; }

    public decimal? TotalAmount { get; set; }

    public byte[] Date { get; set; } = null!;

    public virtual Order Order { get; set; } = null!;

    public virtual PaymentType? PaymentTypeNavigation { get; set; }

    public virtual User User { get; set; } = null!;
}
