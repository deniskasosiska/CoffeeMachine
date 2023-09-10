using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace appCoffeeMachine;
public interface CoffeeMachineInterface
{
	public string getInterfaceInfo();
	public void createUserInterfaceInfo(int bank, int countSugar, int countMilk, string selectCoffee);
	public void createMachineInterfaceInfo(ContextCoffeeMachine data);
	public void createInfo();
	public void createInfo(String str);
	public void switchUserHelp();
	public void switchServiceHelp();
}

