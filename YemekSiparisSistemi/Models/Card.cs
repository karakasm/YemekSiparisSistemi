using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Card
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public string? CardName { get; set; }

    public string? CardNumber { get; set; }

    public DateTime? ExpirationDate { get; set; }

    public virtual User? User { get; set; }
}
