using UnityEngine;

public class Ant : MonoBehaviour
{
    private readonly Agent _agent = new Agent();
    private PheromoneMap _pheromones;

    public float Speed = 0.1f;

    public void Initialize(PheromoneMap pheromones)
    {
        _pheromones = pheromones;

        _agent.Command(new FindFoodCommand(this, pheromones));
    }

    private void Update()
    {
        if (null == _pheromones)
        {
            return;
        }

        _agent.Update(Time.deltaTime);
    }
}