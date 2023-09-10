using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine;
class MachineInterface
{
	private string automatInterfaceInfo = string.Empty;
	private string userInterfaceInfo = string.Empty;
	public bool helpVisibility = false;
	public bool serviceVisibility = false;
	public string info = "\nВведите: ?  -чтобы показать/скрыть список команд пользователя\nВведите: s? -чтобы показать/скрыть список сервесных команд";
	private string help =	$"Выбрать *название кофе*                    - выбрает напиток к заказу\n" +
							$"Заказать                                   - автомат производит кофе\n" +
							$"Сахар *0-5*                                - указывает кол-во сахара от 0-5\n" +
							$"Молоко *0-5*                               - указывает кол-во молока от 0-5\n" +
							$"Внести *монета/купюра* *номинал* *кол-во*  - пополнение счёта\n" +
							$"Сдача                                      - автомат выдаст сдачу\n";
	private string serviceHelp =$"Инкассация                                 - изъятие всех денежных средств из автомата\n" +
								$"Пополнить сдачу                            - пополнение автомата мелочью для выдачи сдачи\n" +
								$"Пополнить всё                              - пополняет все расходники на максимальное кол-во\n" +
								$"Пополнить *название расходника* *кол-во*   - пополняет указаный расходник на указаное кол-во\n" +
								$"История *n*                                - выведет последние n-заказов\n" +
								$"Выкл                                       - выключает автомат\n";

	public MachineInterface(List<Resource> resources, List<Money> money, List<Formula> formulas, List<TypeMoney> typemoney, List<UnitResource> unitresource,
	int bank, int countSugar, int countMilk, string selectCoffee)
	{
		createAutomatInterfaceInfo(resources, money, formulas, typemoney, unitresource);
		createUserInterfaceInfo(bank, countSugar, countMilk, selectCoffee);
	}

	public string getInterfaceInfo()
	{
		String resp = automatInterfaceInfo + userInterfaceInfo;
		resp += helpVisibility ? help + tabulation.underLine("",90) + "\n" : "";
		resp += serviceVisibility ? serviceHelp + tabulation.underLine("",90) + "\n" : "";
		resp += info!=string.Empty ? "Инфо: " + info + "\n" + tabulation.underLine("", 90) + "\n" : "";
		return resp;
	}


	public void createUserInterfaceInfo(int bank, int countSugar, int countMilk, string selectCoffee)
	{
		userInterfaceInfo = "Сахар: ";
		for (int i = 0; i < 5; i++)
		{
			userInterfaceInfo += i < countSugar ? "■" : "_";
		}
		userInterfaceInfo += "  Молоко:";
		for (int i = 0; i < 5; i++)
		{
			userInterfaceInfo += i < countMilk ? "■" : "_";
		}
		userInterfaceInfo += $"  Баланс: {bank}р.  Выбранный кофе:{selectCoffee}\n";
		userInterfaceInfo += tabulation.underLine("", 90)+"\n";
	}
	public void createAutomatInterfaceInfo(List<Resource> resources, List<Money> money, List<Formula> formulas, List<TypeMoney> typemoney, List<UnitResource> unitresource)
	{
		List<String> strings = new List<String>();
		strings.Add(tabulation.whiteSpace("",40)+"Кофе машина");
		strings.Add(tabulation.underLine("", 90));
		strings.Add(tabulation.whiteSpace(tabulation.whiteSpace("Напитки:", 30) + "|Деньги в автомате:", 60) + "|Расходники:");
		strings.Add(tabulation.underLine("", 90));
		int maxCountString = resources.Count >= money.Count && resources.Count >= formulas.Count
		? resources.Count
		: money.Count >= formulas.Count
								? money.Count
								: formulas.Count;

		for (int i = 0; i < maxCountString; i++)
		{
			String tmp = String.Empty;

			if (i < formulas.Count)
			{
				Formula f = formulas[i];
				tmp += $"{f.Id}.{f.Name} - {f.Price}p.";
			}

			tmp = tabulation.whiteSpace(tmp, 30) + "|"; ;

			if (i < money.Count)
			{
				Money m = money[i];
				tmp += $"{m.Id}.{typemoney.Where(t => t.Id == m.Type).Select(t => t.Type).First()} {m.Nominal} - {m.Count}";
			}

			tmp = tabulation.whiteSpace(tmp, 60) + "|"; ;

			if (i < resources.Count)
			{
				Resource r = resources[i];
				tmp += $"{r.Id}.{r.Name} - {r.Count} {unitresource.Where(u => u.Id == r.Unit).Select(u => u.Unit).First()}.";
			}

			strings.Add(tmp);
		}
		strings.Add(tabulation.underLine("", 90));

		automatInterfaceInfo = String.Empty;

		foreach (String s in strings)
		{
			automatInterfaceInfo += s + $"\n";
		}
	}
}

class tabulation{
	public static String whiteSpace(String str, int count)
	{
		return addTabulation(str, count, ' ');
	}
	public static String underLine(String str, int count)
	{
		return addTabulation(str, count, '_');
	}
	public static String addTabulation(String str, int count, char tab)
	{
		for (int i = 0 + str.Length; i < count; i++)
		{
			str += tab;
		}
		return str;
	}
}