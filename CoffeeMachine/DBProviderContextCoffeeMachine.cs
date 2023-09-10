﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appCoffeeMachine;
class DBProviderContextCoffeeMachine
{
	public ContextCoffeeMachine DBContext {get;}
	public DBProviderContextCoffeeMachine()
	{
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			List<Resource> resources = db.Resources.ToList();
			List<Money> moneys = db.Money.ToList();
			List<Formula> formulas = db.Formulas.ToList();
			List<UnitResource> unitResources = db.UnitResources.ToList();
			List<TypeMoney> typeMoneys = db.TypeMoneys.ToList();
			List<Transaction> transactions = db.Transactions.ToList();

			DBContext = new ContextCoffeeMachine((resources, moneys, formulas, unitResources, typeMoneys, transactions));
		}
	}
	public async void saveChange()
	{
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			if (DBContext.resources != null) foreach (Resource r in DBContext.resources) db.Resources.Update(r);
			if (DBContext.moneys != null) foreach (Money m in DBContext.moneys) db.Money.Update(m);
			if (DBContext.formulas != null) foreach (Formula f in DBContext.formulas) db.Formulas.Update(f);
			if (DBContext.unitResources != null) foreach (UnitResource u in DBContext.unitResources) db.UnitResources.Update(u);
			if (DBContext.typeMoneys != null) foreach (TypeMoney t in DBContext.typeMoneys) db.TypeMoneys.Update(t);
			if (DBContext.transactions != null) foreach (Transaction t in DBContext.transactions) db.Transactions.Update(t);

			await db.SaveChangesAsync();
		}
	}
}
