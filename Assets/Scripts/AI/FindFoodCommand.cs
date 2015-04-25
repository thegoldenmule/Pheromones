using UnityEngine;

public class FindFoodCommand : ICommand
{
    private readonly Ant _ant;
    private readonly PheromoneMap _map;

    private StateMachine _behaviors;

    private Vector3 _target;

    public FindFoodCommand(
        Ant ant,
        PheromoneMap map)
    {
        _ant = ant;
        _map = map;
    }

    public void Initialize(StateMachine behaviors)
    {
        _behaviors = behaviors;
    }

    public void Enter()
    {

    }

    public void Update(float dt)
    {
        // smell
        //var whiff = _map.Pheromones();

        // no food, continue wandering
        var moveTo = _behaviors.Current as MoveToBehavior;
        if (null == moveTo
            || moveTo.HasArrived)
        {
            _behaviors.ChangeState(new MoveToBehavior(
                _ant,
                PickDestination()));
        }

        _map.Emit(
            _ant.transform.position,
            3f,
            0,
            0.03f);
    }

    public void Exit()
    {

    }

    private Vector3 PickDestination()
    {
        return new Vector3(
            Random.Range(_map.Bounds.xMin, _map.Bounds.xMax),
            0f,
            Random.Range(_map.Bounds.yMin, _map.Bounds.yMax));
    }
}