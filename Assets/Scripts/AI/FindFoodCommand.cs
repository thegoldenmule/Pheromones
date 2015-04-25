public class FindFoodCommand : IState
{
    private readonly Ant _ant;
    private readonly PheromoneMap _map;

    public FindFoodCommand(
        Ant ant,
        PheromoneMap map)
    {
        _ant = ant;
        _map = map;
    }

    public void Enter()
    {

    }

    public void Update(float dt)
    {
        // smell any food?


        // no food, continue wandering
    }

    public void Exit()
    {

    }
}