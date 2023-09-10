using System;
using System.Collections.Generic;

namespace appCoffeeMachine;

public partial class Transaction
{
    public int Id { get; set; }

    public DateTime DateTime { get; set; }

    public int Formulas { get; set; }

    public int CountMilk { get; set; }

    public int CountSugar { get; set; }

    public virtual Formula FormulasNavigation { get; set; } = null!;
}
