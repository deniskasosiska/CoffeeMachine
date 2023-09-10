using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeMachine;
class MachineConsoleInput
{
	private Machine machine;
	public MachineConsoleInput() {machine = new Machine();}
	public bool getPower() {return machine.getPower();}
	public string getInterface() {return machine.getInterface();}
	public void interaction(String interString)
	{
		if (!interString.IsNullOrEmpty())
		{
			machine.setInterfaseHelp(string.Empty);
			String[] inter = interString.ToLower().Split();

			int count = 0;
			switch (inter[0])
			{
				case "?":
					machine.switchHelp(); break;
				case "s?":
					machine.switchServiseHelp(); break;
				case "выбрать":
					if (inter.Length == 2) machine.select(inter[1]); break;
				case "заказать":
					machine.enter(); break;
				case "сахар":
					if (inter.Length == 2 && int.TryParse(inter[1], out count)) machine.editSugar(count); break;
				case "молоко":
					if (inter.Length == 2 && int.TryParse(inter[1], out count)) machine.editMilk(count); break;
				case "внести":
					if (inter.Length == 4 && int.TryParse(inter[2], out int nominal) && int.TryParse(inter[3], out count)) machine.depositMoney(inter[1], nominal, count); break;
				case "сдача":
					machine.change(); break;
				case "инкассация":
					machine.encashment(); break;
				case "пополнить":
					if (inter.Length > 1)
						switch (inter[1])
						{
							case "сдачу":
								machine.fillMoney(); break;
							case "всё":
								machine.fillAllResources(); break;
							default:
								if (inter.Length == 3 && int.TryParse(inter[2], out count)) machine.fillResource(inter[1], count); break;
						}
					break;
				case "история":
					if (inter.Length == 2 && int.TryParse(inter[1], out count)) machine.history(count); break;
				case "выкл":
					machine.switchOff(); break;
				default: machine.help(); break;
			}
		}
	}
}
