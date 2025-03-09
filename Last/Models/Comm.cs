using System;
using System.Collections.Generic;

namespace Last.Models;

public partial class Comm
{
    public int Id { get; set; }

    public string? Descrip { get; set; }

    public int? Userid { get; set; }

    public int? Vetclinid { get; set; }

    public virtual User? User { get; set; }

    public virtual Vetcin? Vetclin { get; set; }
}
