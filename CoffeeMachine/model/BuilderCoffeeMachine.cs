using appCoffeeMachine.model.data;
using appCoffeeMachine.view;

namespace appCoffeeMachine.model;

class BuilderCoffeeMachine
{
    private ContextCoffeeMachine? context = null;
    private ICoffeeMachineInterface? Interface = null;

    public BuilderCoffeeMachine setContext(ContextCoffeeMachine? context)
    {
        this.context = context;
        return this;
    }
    public BuilderCoffeeMachine setInterface(ICoffeeMachineInterface? Interface)
    {
        this.Interface = Interface;
        return this;
    }
    public CoffeeMachine? build()
    {
        if (context is not null && Interface is not null)
            return new CoffeeMachine(context, Interface);
        return null;
    }

}
