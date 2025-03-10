using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Last.Models;

public partial class Comm
{
    public int Id { get; set; }
    [MaxLength(500)]
    public string? Descrip { get; set; }

    public int? Userid { get; set; }

    public int? Vetclinid { get; set; }

    public virtual User? User { get; set; }

    public virtual Vetcin? Vetclin { get; set; }
}
