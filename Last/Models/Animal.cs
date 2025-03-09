using System;
using System.Collections.Generic;

namespace Last.Models;

public partial class Animal
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public string Age { get; set; } = null!;

    public string Type { get; set; } = null!;

    public int? Userid { get; set; }

    public virtual Passport? Passport { get; set; }

    public virtual User? User { get; set; }
}
