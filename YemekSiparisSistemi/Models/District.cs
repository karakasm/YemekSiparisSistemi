using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace YemekSiparisSistemi.Models;

public partial class District
{
    public int Id { get; set; }

    [ForeignKey("fk_district-province")]
    public int? ProvinceId { get; set; }

    public string DistrictName { get; set; } = null!;

    public virtual Province? Province { get; set; }

    public virtual ICollection<Address> Addresses { get; set; } = new List<Address>();
}
