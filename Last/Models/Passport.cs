using System;
using System.Collections.Generic;

namespace Last.Models;

public partial class Passport
{
    public int Id { get; set; }

    public string? Seria { get; set; }

    public string? Number { get; set; }

    public virtual Animal IdNavigation { get; set; } = null!;

    public virtual ICollection<Vacin> Vacins { get; set; } = new List<Vacin>();
}
