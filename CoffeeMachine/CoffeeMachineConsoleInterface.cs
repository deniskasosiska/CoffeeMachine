using System;
using System.Collections.Generic;
using System.Linq;
using System.Resources;
using System.Text;
using System.Threading.Tasks;

namespace appCoffeeMachine;
class CoffeeMachineConsoleInterface : CoffeeMachineInterface
{
	private string automatInterfaceInfo = string.Empty;
	private string userInterfaceInfo = string.Empty;
	private bool helpVisibility = false;
	private bool serviceVisibility = false;
	private string info = TEXT.INFO;
	private string help = TEXT.HELP;
	private string serviceHelp = TEXT.SERVICE_HELP;

	public string getInterfaceInfo()
	{
		String resp = automatInterfaceInfo + userInterfaceInfo;
		resp += helpVisibility ? help + tabulation.underLine("", 90) + "\n" : "";
		resp += serviceVisibility ? serviceHelp + tabulation.underLine("", 90) + "\n" : "";
		resp += info != string.Empty ? "Инфо: " + info + "\n" + tabulation.underLine("", 90) + "\n" : "";
		return resp;
	}


	public void createUserInterfaceInfo(int bank, int countSugar, int countMilk, string selectCoffee)
	{
		userInterfaceInfo = "Сахар: ";
		for (int i = 0; i < 5; i++)
		{
			userInterfaceInfo += i < countSugar/5 ? "■" : "_";
		}
		userInterfaceInfo += "  Молоко:";
		for (int i = 0; i < 5; i++)
		{
			userInterfaceInfo += i < countMilk/25 ? "■" : "_";
		}
		userInterfaceInfo += $"  Баланс: {bank}р.  Выбранный кофе:{selectCoffee}\n";
		userInterfaceInfo += tabulation.underLine("", 90) + "\n";
	}
	public void createMachineInterfaceInfo(ContextCoffeeMachine data)
	{
		List<String> strings = new List<String>();
		strings.Add(tabulation.whiteSpace("", 40) + "Кофе машина");
		strings.Add(tabulation.underLine("", 90));
		strings.Add(tabulation.whiteSpace(tabulation.whiteSpace("Напитки:", 30) + "|Деньги в автомате:", 60) + "|Расходники:");
		strings.Add(tabulation.underLine("", 90));
		int? maxCountString = data.resources?.Count >= data.moneys?.Count && data.resources?.Count >= data.formulas?.Count
		? data.resources.Count
		: data.moneys?.Count >= data.formulas?.Count
								? data.moneys.Count
								: data.formulas?.Count;
		if (maxCountString != null)
			for (int i = 0; i < maxCountString; i++)
			{
				String tmp = String.Empty;

				if (i < data.formulas?.Count)
				{
					Formula f = data.formulas[i];
					tmp += $"{f.Id}.{f.Name} - {f.Price}p.";
				}

				tmp = tabulation.whiteSpace(tmp, 30) + "|"; ;

				if (i < data.moneys?.Count)
				{
					Money m = data.moneys[i];
					tmp += $"{m.Id}.{data.typeMoneys?.Where(t => t.Id == m.Type).Select(t => t.Type).First()} {m.Nominal} - {m.Count}";
				}

				tmp = tabulation.whiteSpace(tmp, 60) + "|"; ;

				if (i < data.resources?.Count)
				{
					Resource r = data.resources[i];
					tmp += $"{r.Id}.{r.Name} - {r.Count} {data.unitResources?.Where(u => u.Id == r.Unit).Select(u => u.Unit).First()}.";
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
	public void createInfo()
	{
		info = TEXT.INFO;
	}
	public void createInfo(String str)
	{
		info = str;
	}
	public void switchUserHelp()
	{
		helpVisibility = !helpVisibility;
	}
	public void switchServiceHelp()
	{
		serviceVisibility = !serviceVisibility;
	}
}

class tabulation
{
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