using System;
using System.Collections.Generic;

namespace CoffeeMachine;

public partial class Money
{
    public int Id { get; set; }

    public int Type { get; set; }

    public int Nominal { get; set; }

    public int Count { get; set; }

    public virtual TypeMoney TypeNavigation { get; set; } = null!;
}
