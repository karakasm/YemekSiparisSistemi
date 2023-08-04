using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class District
{
    public int Id { get; set; }

    public string DistrictName { get; set; } = null!;

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
