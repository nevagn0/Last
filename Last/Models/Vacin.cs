using System;
using System.Collections.Generic;

namespace Last.Models;

public partial class Vacin
{
    public int Id { get; set; }

    public string? Type { get; set; }

    public int? Passportid { get; set; }

    public virtual Passport? Passport { get; set; }
}
