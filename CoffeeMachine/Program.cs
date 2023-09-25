using appCoffeeMachine;
using appCoffeeMachine.model;
using appCoffeeMachine.model.data;
using appCoffeeMachine.view.console;

try
{
	Console.Title = "Кофемашина";
	CoffeeMachineConsoleInterface Interface = CoffeeMachineConsoleInterface.getInstance();
	DBProviderContextCoffeeMachine provider = DBProviderContextCoffeeMachine.getInstance();
	CoffeeMachine? machine = CoffeeMachine.getBuilder().setContext(provider.DBContext).setInterface(Interface).build();
	CoffeeMachineConsoleInput coffee = CoffeeMachineConsoleInput.getInstance(machine);
	while (coffee.getPower())
	{
		Console.Write(coffee.getInterface());
		if (coffee.interaction(Console.ReadLine())) provider.saveChange();
		Console.Clear();
	}
}catch(Exception ex)
{
	Console.Write($"{TEXT.ERROR_ADMINISTRATOR}\n\n{ex.Message}\n\n{ex.TargetSite}\n\n{ex.StackTrace}");
	Console.ReadKey();
}