using System;
using System.Collections.Generic;

namespace CoffeeMachine;

public partial class Formula
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public int Price { get; set; }

    public int Water { get; set; }

    public int Coffee { get; set; }

    public int RecommendedMilk { get; set; }

    public int RecommendedSugar { get; set; }

    public virtual ICollection<Transaction> Transactions { get; set; } = new List<Transaction>();
}
