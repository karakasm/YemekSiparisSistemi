using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Address
{
    public int Id { get; set; }

    public int? ProvinceId { get; set; }

    public int? DistrictId { get; set; }

    public string? Neighbourhood { get; set; }

    public string? Street { get; set; }

    public string? ApartmentName { get; set; }

    public string PostalCode { get; set; } = null!;

    public byte? ApartmentNumber { get; set; }

    public string? Floor { get; set; }

    public string? Description { get; set; }

    public string? Tag { get; set; }

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual District? District { get; set; }

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual Province? Province { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
