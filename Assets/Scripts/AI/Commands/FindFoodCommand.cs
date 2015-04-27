using UnityEngine;

public class FindFoodCommand : ICommand
{
    private readonly Ant _ant;
    private readonly PheromoneMap _map;

    private StateMachine _behaviors;

    private Vector3 _target;

    private PheromoneReading[] _leftReadings;
    private PheromoneReading[] _rightReadings;

    private FoodSource[] _food;

    public FindFoodCommand(
        Ant ant,
        PheromoneMap map)
    {
        _ant = ant;
        _map = map;

        // boo!
        _food = Object.FindObjectsOfType<FoodSource>();

        _leftReadings = PheromoneMap.Readings(_map.NumPheromones);
        _rightReadings = PheromoneMap.Readings(_map.NumPheromones);
    }

    public void Initialize(StateMachine behaviors)
    {
        _behaviors = behaviors;
    }

    public void Enter()
    {
        _ant.ActivatePheromone(Pheromones.Scouting);
    }

    public void Update(float dt)
    {
        // did we hit food?
        if (FoundFood())
        {
            Debug.Log("Found food!");

            _ant.Agent.Command(new ReturnWithFoodCommand(_ant, _map));

            return;
        }

        // update antennae data
        _map.Pheromones(
            _ant.LeftAntenna.Transform.position,
            _ant.LeftAntenna.SampleRadius,
            ref _leftReadings);
        _map.Pheromones(
            _ant.RightAntenna.Transform.position,
            _ant.RightAntenna.SampleRadius,
            ref _rightReadings);

        var moveTo = _behaviors.Current as MoveToBehavior;

        // analyze readings
        var maxReading = _leftReadings[Pheromones.Food].Max > _leftReadings[Pheromones.Food].Max
            ? _leftReadings[Pheromones.Food]
            : _rightReadings[Pheromones.Food];
        if (maxReading.Detected)
        {
            // follow your nose
            if (null == moveTo)
            {
                moveTo = new MoveToBehavior(_ant, _ant.transform.position);
                _behaviors.ChangeState(moveTo);
            }

            moveTo.UpdateTarget(_ant.transform.position + maxReading.MaxDirection);

            return;
        }


        // no food, continue wandering
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
        _ant.DeactivatePheromone(Pheromones.Scouting);
    }

    private bool FoundFood()
    {
        for (int i = 0, len = _food.Length; i < len; i++)
        {
            var food = _food[i];

            if (food.Value < Mathf.Epsilon)
            {
                continue;
            }

            if ((food.transform.position - _ant.transform.position).magnitude < food.Radius)
            {
                food.Value = Mathf.Max(0f, food.Value - 1f);

                return true;
            }
        }

        return false;
    }

    private Vector3 PickDestination()
    {
        return new Vector3(
            Random.Range(_map.Bounds.xMin, _map.Bounds.xMax),
            0f,
            Random.Range(_map.Bounds.yMin, _map.Bounds.yMax));
    }
}