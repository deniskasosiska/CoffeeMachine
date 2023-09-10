using System;
using System.Collections.Generic;

namespace CoffeeMachine;

public partial class Resource
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Count { get; set; }

    public int MaxCount { get; set; }

    public int Unit { get; set; }

    public virtual UnitResource UnitNavigation { get; set; } = null!;
}
