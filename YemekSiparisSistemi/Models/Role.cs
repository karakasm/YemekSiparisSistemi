using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Role
{
    public int Id { get; set; }

    public string? RoleName { get; set; }

    public virtual ICollection<User> Users { get; set; } = new List<User>();

    public virtual ICollection<Company> Companies { get; set; } = new List<Company>();
}
