using UnityEngine;

public class ReturnWithFoodCommand : ICommand
{
    private readonly Ant _ant;
    private readonly PheromoneMap _map;

    private PheromoneReading[] _leftReadings;
    private PheromoneReading[] _rightReadings;

    private StateMachine _behaviors;

    public ReturnWithFoodCommand(
        Ant ant,
        PheromoneMap map)
    {
        _ant = ant;
        _map = map;

        _leftReadings = PheromoneMap.Readings(_map.NumPheromones);
        _rightReadings = PheromoneMap.Readings(_map.NumPheromones);
    }

    public void Initialize(StateMachine behaviors)
    {
        _behaviors = behaviors;
    }

    public void Enter()
    {
        _ant.ActivatePheromone(Pheromones.Food);
    }

    public void Update(float dt)
    {
        // smell out trail
        _map.Read(
            _ant.LeftAntenna.Transform.position,
            _ant.LeftAntenna.SampleRadius,
            ref _leftReadings);
        _map.Read(
            _ant.RightAntenna.Transform.position,
            _ant.RightAntenna.SampleRadius,
            ref _rightReadings);

        // travel backward
        var max = _leftReadings[Pheromones.Scouting].Max > _rightReadings[Pheromones.Scouting].Max
            ? _leftReadings[Pheromones.Scouting]
            : _rightReadings[Pheromones.Scouting];
        if (max.Detected)
        {
            var moveTo = _behaviors.Current as MoveToBehavior;
            if (null == moveTo)
            {
                moveTo = new MoveToBehavior(_ant, _ant.transform.position);
            }

            moveTo.UpdateTarget(_ant.transform.position - max.MaxDirection);
        }

        // our trail was blown away! pick a random destination!
        else
        {
            Debug.Log("No trail found!");
        }
    }

    public void Exit()
    {
        _ant.DeactivatePheromone(Pheromones.Food);
    }
}