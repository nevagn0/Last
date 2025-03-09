using System;
using System.Collections.Generic;

namespace Last.Models;

public partial class User
{
    public int Id { get; set; }

    public string? Firstname { get; set; }

    public string? Secondname { get; set; }

    public string? Password { get; set; }

    public virtual ICollection<Animal> Animals { get; set; } = new List<Animal>();

    public virtual ICollection<Comm> Comms { get; set; } = new List<Comm>();

    public virtual ICollection<Record> Records { get; set; } = new List<Record>();
}
