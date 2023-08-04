using System;
using System.Collections.Generic;

namespace YemekSiparisSistemi.Models;

public partial class Comment
{
    public int Id { get; set; }

    public int? UserId { get; set; }

    public int? CompanyId { get; set; }

    public string? Comment1 { get; set; }

    public DateTime? Date { get; set; }

    public virtual Company? Company { get; set; }

    public virtual User? User { get; set; }
}
