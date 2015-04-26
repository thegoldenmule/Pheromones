using UnityEngine;

/// <summary>
/// Contains data for an antenna.
/// </summary>
[System.Serializable]
public class AntennaData
{
    public Transform Transform;
    public float SampleRadius = 5f;
}

public class Ant : MonoBehaviour
{
    private readonly Agent _agent = new Agent();
    private PheromoneMap _pheromones;
    private Vector3 _lastFrame;
    private float _rollOver = 0f;

    public float Speed = 0.1f;
    public float DropDistance = 0.1f;
    public float DropIntensity = 0.01f;
    public float DropRadius = 1f;

    public AntennaData LeftAntenna;
    public AntennaData RightAntenna;
    
    public void Initialize(PheromoneMap pheromones)
    {
        _pheromones = pheromones;

        _agent.Command(new FindFoodCommand(this, pheromones));

        _lastFrame = transform.position;
    }

    private void Update()
    {
        if (null == _pheromones)
        {
            return;
        }

        _agent.Update(Time.deltaTime);

        var position = transform.position;
        var delta = position - _lastFrame;
        var length = delta.magnitude;
        var direction = delta / length;

        var origin = position - _rollOver * direction;
        length += _rollOver;

        var sum = 0f;
        while (length > DropDistance && DropDistance > Mathf.Epsilon)
        {
            sum += DropDistance;
            length -= DropDistance;

            _pheromones.Emit(
                origin + sum * direction,
                DropRadius,
                0,
                DropIntensity);
        }

        _rollOver = length;
        _lastFrame = position;
    }
}