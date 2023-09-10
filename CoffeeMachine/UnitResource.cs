using System;
using System.Collections.Generic;

namespace appCoffeeMachine;

public partial class UnitResource
{
    public int Id { get; set; }

    public string Unit { get; set; } = null!;

    public virtual ICollection<Resource> Resources { get; set; } = new List<Resource>();
}
