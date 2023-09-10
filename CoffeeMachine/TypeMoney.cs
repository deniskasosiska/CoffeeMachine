using System;
using System.Collections.Generic;

namespace CoffeeMachine;

public partial class TypeMoney
{
    public int Id { get; set; }

    public string Type { get; set; } = null!;

    public virtual ICollection<Money> Money { get; set; } = new List<Money>();
}
