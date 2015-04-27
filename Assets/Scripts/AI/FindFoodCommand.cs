using UnityEngine;

public class FindFoodCommand : ICommand
{
    private readonly Ant _ant;
    private readonly PheromoneMap _map;

    private StateMachine _behaviors;

    private Vector3 _target;

    private PheromoneReading[] _leftReadings;
    private PheromoneReading[] _rightReadings;

    public FindFoodCommand(
        Ant ant,
        PheromoneMap map)
    {
        _ant = ant;
        _map = map;

        _leftReadings = new PheromoneReading[_map.NumPheromones];
        _rightReadings = new PheromoneReading[_map.NumPheromones];
        for (int i = 0; i < _map.NumPheromones; i++)
        {
            _leftReadings[i] = new PheromoneReading();
            _rightReadings[i] = new PheromoneReading();
        }
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
        _map.Pheromones(
            _ant.LeftAntenna.Transform.position,
            _ant.LeftAntenna.SampleRadius,
            ref _leftReadings);
        _map.Pheromones(
            _ant.RightAntenna.Transform.position,
            _ant.RightAntenna.SampleRadius,
            ref _rightReadings);

        // analyze readings


        // no food, continue wandering
        var moveTo = _behaviors.Current as MoveToBehavior;
        if (null == moveTo
            || moveTo.HasArrived)
        {
            _behaviors.ChangeState(new MoveToBehavior(
                _ant,
                PickDestination()));
        }
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