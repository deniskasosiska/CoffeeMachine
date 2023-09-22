using appCoffeeMachine.EF;
using appCoffeeMachine.model.data;
using appCoffeeMachine.view;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace appCoffeeMachine.model;

//Класс представляешь логику работы автомата
class CoffeeMachine
{
    private ContextCoffeeMachine context;
    private bool power = true;
    private int bank = 0;
    private int countSugar = 3;
    private int countMilk = 3;
    private string selectCoffee = string.Empty;
    private ICoffeeMachineInterface Interface;
    //инициализация
    public CoffeeMachine(ContextCoffeeMachine context, ICoffeeMachineInterface Interface)
    {
        this.context = context;
        this.Interface = Interface;
        CI();
    }
    public static BuilderCoffeeMachine getBuilder()
    {
        return new BuilderCoffeeMachine();
    }
    //возвращает состояние питания автомата
    public bool getPower()
    {
        return power;
    }
    //возвращает интерфейс автомата
    public string getInterface()
    {
        return Interface.getInterfaceInfo();
    }
    //Бередает текст информационного собщения в интерфейс
    public void setInterfaceInfo(string str)
    {
        Interface.createInfo(str);
    }
    //Включает выключает вывод подсказки с командами пользователя
    public void switchHelp()
    {
        Interface.switchUserHelp();
    }
    //Включает выключает вывод подсказки с сервисными командами
    public void switchServiseHelp()
    {
        Interface.switchServiceHelp();
    }
    //Выключает автомат
    public void switchOff()
    {
        power = false;
    }
    //Выбор напитка и установка уровня сахара и молока на рекомендованые значения по рецепту
    public void select(string name)
    {
        Formula? formula = context.formulas?.FirstOrDefault(f => f.Name.ToLower() == name);
        if (formula != null)
        {
            selectCoffee = formula.Name;
            countSugar = formula.RecommendedSugar;
            countMilk = formula.RecommendedMilk;
        }
        CUI();
    }
    //Запуск производства напитка
    public bool enter()
    {
        string resp = string.Empty;

        bool success = false;

        Formula? formula = context.formulas.FirstOrDefault(f => f.Name.ToLower() == selectCoffee.ToLower());
        if (formula != null)
        {

            Resource? cup = context.resources.FirstOrDefault(r => r.Name.ToLower() == "стаканчики");
            Resource? stick = context.resources.FirstOrDefault(r => r.Name.ToLower() == "палочки");
            Resource? water = context.resources.FirstOrDefault(r => r.Name.ToLower() == "вода");
            Resource? milk = context.resources.FirstOrDefault(r => r.Name.ToLower() == "молоко");
            Resource? sugar = context.resources.FirstOrDefault(r => r.Name.ToLower() == "сахар");
            Resource? coffee = context.resources.FirstOrDefault(r => r.Name.ToLower() == "кофе");

            if (cup != null && cup.Count > 0)
            {
                if (stick != null && stick.Count > 0)
                {
                    if (water != null && water.Count >= formula.Water)
                    {
                        if (milk != null && milk.Count >= countMilk * 25)
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

                                        Transaction transaction = new Transaction() { DateTime = DateTime.Now, Formulas = formula.Id, CountMilk = countMilk * 25, CountSugar = countSugar * 5 };
                                        context.transactions.Add(transaction);
                                        resp = $" Ваш {formula.Name} готов!";
                                        success = true;
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
        setInterfaceInfo(resp);
        CI();
        return success;
    }
    //регулировка кол-ва сахара
    public void editSugar(int count)
    {
        countSugar = count < 0 ? 0 : count > 5 ? 25 : count * 5;
        CUI();
    }
    //регулировка кол-ва молока
    public void editMilk(int count)
    {
        countMilk = count < 0 ? 0 : count > 5 ? 125 : count * 25;
        CUI();
    }
    //пополнение денег
    public bool depositMoney(string type, int nominal, int count)
    {
        bool success = false;
        count = count < 0 ? 0 : count;
        Money? money;

        money = context.moneys.Where(m => m.Type == context.typeMoneys.FirstOrDefault(t => t.Type.ToLower() == type)?.Id).FirstOrDefault(m => m.Nominal == nominal);
        if (money != null)
        {
            if (count * nominal + bank <= 999999)
            {
                money.Count += count;
                bank += count * nominal;
                success = true;
            }
            else
            {
                setInterfaceInfo("Нельзя чтобы пополнить баланс больше 999999р.");
            }
        }
        CI();
        return success;
    }
    //выдача сдачи
    public bool change()
    {
        bool success = false;
        if (bank > 0)
        {
            string resp = "cдача выдана ";
            List<Money> moneys = context.moneys.Where(m => m.TypeNavigation.Type.ToLower() == "монета").ToList();
            Money? money10 = moneys.FirstOrDefault(m => m.Nominal == 10);
            Money? money5 = moneys.FirstOrDefault(m => m.Nominal == 5);
            Money? money2 = moneys.FirstOrDefault(m => m.Nominal == 2);
            Money? money1 = moneys.FirstOrDefault(m => m.Nominal == 1);
            if (changeCalc(changeCalc(changeCalc(changeCalc(bank, money10, ref resp), money5, ref resp), money2, ref resp), money1, ref resp) == 0)
            {
                bank = 0;
                success = true;
            }
            else
            {
                resp = "нет мелочи чтобы выдать всю сдачу сдачу!";
            }
            setInterfaceInfo(resp);
            CI();
        }
        return success;
    }
    //метод расчёта сдачи
    private int changeCalc(int bank, Money? money, ref string resp)
    {
        if (money != null && money.Count > 0 && bank > 0)
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
    //произведение инкассации (изъятия всех денег из автомата)
    public bool encashment()
    {
        int encashmentCount = 0;
        List<Money> moneys = context.moneys;
        foreach (Money money in moneys)
        {
            encashmentCount += money.Count * money.Nominal;
            money.Count = 0;
        }
        setInterfaceInfo($"из автомата изъято {encashmentCount}р.");
        CMI();
        return true;
    }
    //пополнение автомата мелочью для выдачи сдачи
    public bool fillMoney()
    {
        List<Money> moneys = context.moneys.Where(m => m.TypeNavigation.Type.ToLower() == "монета").ToList();
        foreach (Money money in moneys)
        {
            money.Count = money.Count < 50 ? 50 : money.Count;
        }
        CMI();
        return true;
    }
    //пополнение всех ресурсов на максимальное количество
    public bool fillAllResources()
    {

        List<Resource> res = context.resources;
        foreach (Resource resource in res)
        {
            resource.Count = resource.MaxCount;
        }
        CMI();
        return true;
    }
    //пополнение указаного ресурса на указаное количество
    public bool fillResource(string name, int count)
    {
        bool success = false;
        Resource? res = context.resources.Where(r => r.Name.ToLower() == name).FirstOrDefault();
        if (res != null)
        {
            res.Count += count < 0 ? 0 : count;
            res.Count = res.Count > res.MaxCount ? res.MaxCount : res.Count;
            success = true;
        }
        CMI();
        return success;
    }
    //вывод истории покупок
    public void history(int count)
    {
        string resp = string.Empty;
        List<Transaction> transactions = context.transactions.Skip(Math.Max(0, context.transactions.Count() - count)).ToList();
        resp = "\n";
        foreach (Transaction transaction in transactions)
        {
            Formula? formula = context.formulas.FirstOrDefault(f => f.Id == transaction.Formulas);
            if (formula != null) resp += $"{transaction.Id}. {transaction.DateTime} {formula.Name} {formula.Price}р. Молоко-{transaction.CountMilk}мл. Сахар-{transaction.CountSugar}гр.\n";
        }
        setInterfaceInfo(resp);
    }
    //вывод вспомогательной информации
    public void help()
    {
        Interface.createInfo();
    }
    //перерисовка пользовательского интерфейса и интерфейса автомата
    private void CI()
    {
        CUI();
        CMI();
    }
    //перерисовка интерфейса пользователя
    private void CUI()
    {
        Interface.createUserInterfaceInfo(bank, countSugar, countMilk, selectCoffee);
    }
    //перерисовка интерфейса автомата
    private void CMI()
    {
        Interface.CreateMachineInterfaceInfo(context);
    }
}
