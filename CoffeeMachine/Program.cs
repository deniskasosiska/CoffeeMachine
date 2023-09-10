using CoffeeMachine;

MachineConsoleInput coffee = new MachineConsoleInput();
while (coffee.getPower())
{
	Console.Write(coffee.getInterface());
	coffee.interaction(Console.ReadLine());
	Console.Clear();
}