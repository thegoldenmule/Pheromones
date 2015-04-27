using UnityEngine;
using System.Collections.Generic;


/// <summary>
/// Contains data for an antenna.
/// </summary>
[System.Serializable]
public class AntennaData
{
    public Transform Transform;
    public float SampleRadius = 5f;
}

/// <summary>
/// Describes how a pheromone is emitted.
/// </summary>
[System.Serializable]
public class EmissionData
{
    public int Pheromone;
    public float Intensity = 0.01f;
    public float Radius = 1f;
}

[RequireComponent(typeof(AntMover))]
public class Ant : MonoBehaviour
{
    private readonly Agent _agent = new Agent();
    private PheromoneMap _pheromones;
    private Vector3 _lastFrame;
    private float _rollOver = 0f;

    public EmissionData[] EmissionData;

    public AntennaData LeftAntenna;
    public AntennaData RightAntenna;
    public Transform Head;

    public float DropDistance = 0.1f;
    
    private readonly List<EmissionData> _activePheromones = new List<EmissionData>();

    public Agent Agent
    {
        get
        {
            return _agent;
        }
    }

    public AntMover Mover
    {
        get
        {
            return GetComponent<AntMover>();
        }
    }
    
    public void Initialize(PheromoneMap pheromones)
    {
        _pheromones = pheromones;

        _agent.Command(new FindFoodCommand(this, pheromones));

        _lastFrame = transform.position;
    }

    public void ActivatePheromone(int pheromone)
    {
        for (int i = 0, len = EmissionData.Length; i < len; i++)
        {
            var emission = EmissionData[i];
            if (null != emission & emission.Pheromone == pheromone)
            {
                _activePheromones.Add(emission);
                
                break;
            }
        }
    }

    public void DeactivatePheromone(int pheromone)
    {
        for (int i = 0, len = _activePheromones.Count; i < len; i++)
        {
            if (_activePheromones[i].Pheromone == pheromone)
            {
                _activePheromones.RemoveAt(i);

                break;
            }
        }
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

            for (int p = 0, plen = _activePheromones.Count; p < plen; p++)
            {
                var emissionData = _activePheromones[p];
                if (null == emissionData)
                {
                    continue;
                }

                _pheromones.Emit(
                    origin + sum * direction,
                    emissionData.Radius,
                    emissionData.Pheromone,
                    emissionData.Intensity);
            }
        }

        _rollOver = length;
        _lastFrame = position;
    }
}