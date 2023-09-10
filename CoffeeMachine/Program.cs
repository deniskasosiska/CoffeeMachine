using appCoffeeMachine;

try
{
	CoffeeMachineConsoleInterface Interface = new CoffeeMachineConsoleInterface();
	DBProviderContextCoffeeMachine provider = new DBProviderContextCoffeeMachine();
	CoffeeMachine machine = new CoffeeMachine(provider.DBContext, Interface);
	CoffeeMachineConsoleInput coffee = new CoffeeMachineConsoleInput(machine);
	while (coffee.getPower())
	{
		Console.Write(coffee.getInterface());
		if (coffee.interaction(Console.ReadLine())) provider.saveChange();
		Console.Clear();
	}
}catch(Exception ex)
{
	Console.Write($"{TEXT.ERROR_ADMINISTRATOR}\n\n{ex.Message}\n\n{ex.TargetSite}\n\n{ex.StackTrace}");
}