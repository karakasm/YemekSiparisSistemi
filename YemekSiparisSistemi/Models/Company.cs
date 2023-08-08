using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Company
{
    public int Id { get; set; }

    public int? RoleId { get; set; }

    public string? CompanyName { get; set; }

    public string LogoPath { get; set; } = null!;

    public int? AddressId { get; set; }

    public string Email { get; set; } = null!;

    public string Phone { get; set; } = null!;

    public string Password { get; set; } = null!;

    public virtual Address? Address { get; set; }

    public virtual Role? Role { get; set; }

    public virtual ICollection<Category> Categories { get; set; } = new List<Category>();

    public virtual ICollection<Comment> Comments { get; set; } = new List<Comment>();

    public virtual ICollection<Courier> Couriers { get; set; } = new List<Courier>();

    public virtual ICollection<Delivery> Deliveries { get; set; } = new List<Delivery>();

    public virtual ICollection<Menu> Menus { get; set; } = new List<Menu>();

    public virtual ICollection<Order> Orders { get; set; } = new List<Order>();

    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
