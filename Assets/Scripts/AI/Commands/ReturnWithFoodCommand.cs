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
        PheromoneReading reading;
        Vector3 readingOrigin;

        var leftReading = _leftReadings[Pheromones.Food];
        var rightReading = _rightReadings[Pheromones.Food];

        if (leftReading.Min < rightReading.Min)
        {
            reading = leftReading;
            readingOrigin = _ant.LeftAntenna.Transform.position;
        }
        else
        {
            reading = rightReading;
            readingOrigin = _ant.RightAntenna.Transform.position;
        }

        if (reading.Detected)
        {
            var moveTo = _behaviors.Current as MoveToBehavior;
            if (null == moveTo)
            {
                moveTo = new MoveToBehavior(_ant, _ant.transform.position);
            }

            moveTo.UpdateTarget(readingOrigin + reading.MinDirection);
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