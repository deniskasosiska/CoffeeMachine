using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine;
class Machine
{
	private bool power = true;
	private List<Resource> resources;
	private List<Money> money;
	private List<Formula> formulas;
	private List<TypeMoney> typemoney;
	private List<UnitResource> unitresource;
	private int bank = 0;
	private int countSugar = 3;
	private int countMilk = 3;
	private string selectCoffee = string.Empty;
	private MachineInterface Interface;

	public Machine() {
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			resources = db.Resources.ToList();
			money = db.Money.ToList();
			formulas = db.Formulas.ToList();
			typemoney = db.TypeMoneys.ToList();
			unitresource = db.UnitResources.ToList();
		}
		Interface = new MachineInterface(resources, money, formulas, typemoney, unitresource, bank, countSugar, countMilk, selectCoffee);
	}
	public bool getPower()
	{
		return power;
	}
	
	public string getInterface()
	{
		return Interface.getInterfaceInfo();
	}
	public void setInterfaseHelp(string str)
	{
		Interface.info = str;
	}
	public void switchHelp()
	{
		Interface.helpVisibility = !Interface.helpVisibility;
	}
	public void switchServiseHelp()
	{
		Interface.serviceVisibility = !Interface.serviceVisibility;
	}
	public void switchOff()
	{
		power = !power;
	}
	public void select(String name)
	{
		Formula? formula = formulas.FirstOrDefault(f => f.Name.ToLower() == name);
		if (formula != null)
		{
			selectCoffee = formula.Name;
			countSugar = formula.RecommendedSugar/5;
			countMilk = formula.RecommendedMilk/25;
		}
		CUI();
	}
	public void enter() { 
		string resp = string.Empty;
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			Formula? formula = db.Formulas.FirstOrDefault(f => f.Name.ToLower()==selectCoffee);
			if (formula != null) {

				Resource? cup = db.Resources.FirstOrDefault(r => r.Name.ToLower() == "стаканчики");
				Resource? stick = db.Resources.FirstOrDefault(r => r.Name.ToLower() == "палочки");
				Resource? water = db.Resources.FirstOrDefault(r => r.Name.ToLower() == "вода");
				Resource? milk = db.Resources.FirstOrDefault(r => r.Name.ToLower() == "молоко");
				Resource? sugar = db.Resources.FirstOrDefault(r => r.Name.ToLower() == "сахар");
				Resource? coffee = db.Resources.FirstOrDefault(r => r.Name.ToLower() == "кофе");

				if (cup != null && cup.Count > 0) 
				{
					if (stick != null && stick.Count > 0)
					{
						if (water != null && water.Count >= formula.Water)
						{
							if (milk != null && milk.Count >= countMilk*25) 
							{ 
								if (sugar != null && sugar.Count >= countSugar * 5)
								{
									if (coffee != null && coffee.Count >= formula.Coffee)
									{
										if (bank >= formula.Price)
										{
											cup.Count--;
											stick.Count--;
											water.Count -= formula.Water;
											milk.Count -= countMilk * 25;
											sugar.Count -= countSugar * 5;
											coffee.Count -= formula.Coffee;
											bank -= formula.Price;
											selectCoffee = string.Empty;

											Transaction transaction = new Transaction() {DateTime = DateTime.Now, Formulas = formula.Id, CountMilk = countMilk * 25, CountSugar = countSugar * 5 };
											db.Transactions.Add(transaction);
											db.SaveChanges();
											resp =  $" Ваш {formula.Name} готов!";

											resources = db.Resources.ToList();
										}
										else resp = "не достаточно средств!";
									}
									else resp = "в автомате не достаточно кофе!";
								}
								else resp = "в автомате не достаточно сахара!";
							}
							else resp = "в автомате не достаточно молока!";
						}
						else resp = "в автомате не достаточно воды!";
					}
					else resp = "в автомате закончились палочки!";
				}
				else resp = "в автомате закончились стаканчики!";
			}
			else resp = "для заказа выберите кофе!";
		}
		Interface.info = resp;
		CI();
	}
	public void editSugar(int count)
	{
		countSugar = count < 0 ? 0 : count>5 ? 5 : count;
		CUI();
	}
	public void editMilk(int count)
	{
		countMilk = count < 0 ? 0 : count > 5 ? 5 : count;
		CUI();
	}
	public void depositMoney(String type, int nominal, int count)
	{
		count = count < 0 ? 0 : count;
		Money? money;
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			money = db.Money.Where(m => m.TypeNavigation.Type.ToLower() == type).FirstOrDefault(m => m.Nominal == nominal);
			if (money != null)
			{
				money.Count += count;
				bank += count * nominal;
				db.SaveChanges();
			}
			this.money = db.Money.ToList();
		}
		CI();
	}
	public void change()
	{
		if (bank > 0)
		{
			using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
			{
				string resp = "cдача выдана ";
				List<Money> moneys = db.Money.Where(m => m.TypeNavigation.Type.ToLower() == "монета").ToList();
				Money? money10 = moneys.FirstOrDefault(m => m.Nominal == 10);
				Money? money5 = moneys.FirstOrDefault(m => m.Nominal == 5);
				Money? money2 = moneys.FirstOrDefault(m => m.Nominal == 2);
				Money? money1 = moneys.FirstOrDefault(m => m.Nominal == 1);
				if(changeCalc(changeCalc(changeCalc(changeCalc(bank, money10, ref resp), money5, ref resp), money2, ref resp), money1, ref resp) == 0)
				{
					bank = 0;
					db.SaveChanges();
					money = db.Money.ToList();
				}
				else
				{
					resp = "нет мелочи не можем выдать сдачу!";
				}
				Interface.info = resp;
			}
			
			CI();
		}
	}
	private int changeCalc(int bank, Money? money, ref string resp)
	{
		if (money != null && money.Count>0 && bank>0)
		{
			if (bank / money.Nominal > money.Count)
			{
				resp += $"монета {money.Nominal} - {money.Count}шт. ";
				bank -= money.Nominal * money.Count;
				money.Count = 0;
			}
			else
			{
				resp += $"монета {money.Nominal} - {bank / money.Nominal}шт. ";
				money.Count -= bank / money.Nominal;
				bank = bank % money.Nominal;
			}
		}
		return bank;
	}
	public void encashment() 
	{
		int encashmentCount = 0;
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			List<Money> moneys = db.Money.ToList();
			foreach (Money money in moneys)
			{
				encashmentCount += money.Count * money.Nominal;
				money.Count = 0;
			}
			db.SaveChanges();
			this.money = db.Money.ToList();
		}
		CAI();
		Interface.info = $"из автомата изъято {encashmentCount}р.";
	}
	public void fillMoney()
	{
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			List<Money> moneys = db.Money.Where(m => m.TypeNavigation.Type.ToLower()=="монета").ToList();
			foreach (Money money in moneys)
			{
				money.Count = money.Count<50 ? 50 : money.Count;
			}
			db.SaveChanges();
			this.money = db.Money.ToList();
		}
		CAI();
	}
	public void fillAllResources()
	{
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			List<Resource> res = db.Resources.ToList();
			foreach(Resource resource in res)
			{
				resource.Count = resource.MaxCount;
			}
			resources = res;
			db.SaveChanges();
		}
		CAI();
	}
	public void fillResource(string name, int count)
	{
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			Resource? res = db.Resources.Where(r => r.Name.ToLower() == name).FirstOrDefault();
			if (res != null)
			{
				res.Count += count<0 ? 0 : count;
				res.Count = res.Count>res.MaxCount? res.MaxCount: res.Count;
				resources = db.Resources.ToList();
				db.SaveChanges();
			}
		}
		CAI();
	}
	public void history(int count)
	{
		using (CoffeeMachineDbContext db = new CoffeeMachineDbContext())
		{
			string resp = string.Empty;
			List<Transaction> transactions = db.Transactions.Skip(Math.Max(0,db.Transactions.Count()-count)).ToList();
			resp = "\n";
			foreach (Transaction transaction in transactions)
			{
				Formula? formula = formulas.FirstOrDefault(f => f.Id == transaction.Formulas);
				if (formula!=null) resp += $"{transaction.Id}. {transaction.DateTime} {formula.Name} {formula.Price}р. Молоко-{transaction.CountMilk}мл. Сахар-{transaction.CountSugar}гр.\n";
			}
			Interface.info=resp;
		}
	}
	public void help()
	{
		Interface.info = "\nВведите: ?  -чтобы показать/скрыть список команд пользователя\nВведите: s? -чтобы показать/скрыть список сервесных команд";
	}
	private void CI()
	{
		CUI();
		CAI();
	}
	private void CUI()
	{
		Interface.createUserInterfaceInfo(bank, countSugar, countMilk, selectCoffee);
	}
	private void CAI() 
	{
		Interface.createAutomatInterfaceInfo(resources, money, formulas, typemoney, unitresource);
	}
}

