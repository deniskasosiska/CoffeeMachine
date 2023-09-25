using appCoffeeMachine.EF;

namespace appCoffeeMachine.model.data;
//класс для хранения данных из модели
class ContextCoffeeMachine
{
    public List<Resource> resources;
    public List<Money> moneys;
    public List<Formula> formulas;
    public List<UnitResource> unitResources;
    public List<TypeMoney> typeMoneys;
    public List<Transaction> transactions;
    public ContextCoffeeMachine((List<Resource>?, List<Money>?, List<Formula>?, List<UnitResource>?,
        List<TypeMoney>?, List<Transaction>?) data)
    {
        resources = data.Item1 != null ? data.Item1 : new List<Resource>();
        moneys = data.Item2 != null ? data.Item2 : new List<Money>();
        formulas = data.Item3 != null ? data.Item3 : new List<Formula>();
        unitResources = data.Item4 != null ? data.Item4 : new List<UnitResource>();
        typeMoneys = data.Item5 != null ? data.Item5 : new List<TypeMoney>();
        transactions = data.Item6 != null ? data.Item6 : new List<Transaction>();
    }
}
