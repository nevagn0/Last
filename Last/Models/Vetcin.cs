using System;
using System.Collections.Generic;

namespace Last.Models;

public partial class Vetcin
{
    public int Id { get; set; }

    public string Adress { get; set; } = null!;

    public string? Phone { get; set; }

    public virtual ICollection<Comm> Comms { get; set; } = new List<Comm>();

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
