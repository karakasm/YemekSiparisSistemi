using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Delivery
{
    public int Id { get; set; }

    public int? CompanyId { get; set; }

    public int? OrderId { get; set; }

    public int? CourierId { get; set; }

    public int? AddressId { get; set; }

    public int? StatusId { get; set; }

    public DateTime? ReleaseDate { get; set; }

    public DateTime? ArrivalDate { get; set; }

    public virtual Address? Address { get; set; }

    public virtual Company? Company { get; set; }

    public virtual Courier? Courier { get; set; }

    public virtual Order? Order { get; set; }

    public virtual DeliveryStatus? Status { get; set; }
}
