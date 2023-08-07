using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Province
{
    public int Id { get; set; }

    public string ProvinceName { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();

    public virtual ICollection<District> Districts { get; set; } = new List<District>();
}
