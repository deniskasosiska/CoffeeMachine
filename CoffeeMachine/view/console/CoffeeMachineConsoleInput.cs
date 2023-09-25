using appCoffeeMachine.model;

namespace appCoffeeMachine.view.console;
//Класс обработчик консольного ввода команд, принимает команду и дёргает соответствующий метод автомата
class CoffeeMachineConsoleInput
{
    private CoffeeMachine? machine;
    private static CoffeeMachineConsoleInput? instance;
    private CoffeeMachineConsoleInput(CoffeeMachine? machine)
    {
        this.machine = machine;
    }
    public static CoffeeMachineConsoleInput getInstance(CoffeeMachine? machine)
    {
        if (instance == null)
            instance = new CoffeeMachineConsoleInput(machine);
        return instance;
    }
    public bool getPower() { return machine.getPower(); }
    public string getInterface() { return machine.getInterface(); }
    public bool interaction(string? interString)
    {
        bool changingContext = false;
        if (interString != null)
        {
            machine.setInterfaceInfo(string.Empty);
            string[] inter = interString.ToLower().Split();

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
                    changingContext = machine.enter(); break;
                case "сахар":
                    if (inter.Length == 2 && int.TryParse(inter[1], out count)) machine.editSugar(count); break;
                case "молоко":
                    if (inter.Length == 2 && int.TryParse(inter[1], out count)) machine.editMilk(count); break;
                case "внести":
                    if (inter.Length == 4 && int.TryParse(inter[2], out int nominal) && int.TryParse(inter[3], out count))
                        changingContext = machine.depositMoney(inter[1], nominal, count); break;
                case "сдача":
                    changingContext = machine.change(); break;
                case "инкассация":
                    changingContext = machine.encashment(); break;
                case "пополнить":
                    if (inter.Length > 1)
                        switch (inter[1])
                        {
                            case "сдачу":
                                changingContext = machine.fillMoney(); break;
                            case "всё":
                                changingContext = machine.fillAllResources(); break;
                            default:
                                if (inter.Length == 3 && int.TryParse(inter[2], out count)) changingContext = machine.fillResource(inter[1], count); break;
                        }
                    break;
                case "история":
                    if (inter.Length == 2 && int.TryParse(inter[1], out count)) machine.history(count); break;
                case "выкл":
                    machine.switchOff(); break;
                default: machine.help(); break;
            }
        }
        return changingContext;
    }
}
