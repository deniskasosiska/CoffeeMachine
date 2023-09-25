using appCoffeeMachine.model.data;

namespace appCoffeeMachine.view;
//интерфейс классов вывода для нашего автомата
interface ICoffeeMachineInterface
{
    public string getInterfaceInfo();
    public void createUserInterfaceInfo(int bank, int countSugar, int countMilk, string selectCoffee);
    public void CreateMachineInterfaceInfo(ContextCoffeeMachine data);
    public void createInfo();
    public void createInfo(string str);
    public void switchUserHelp();
    public void switchServiceHelp();
}

