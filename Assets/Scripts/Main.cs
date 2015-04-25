using UnityEngine;

public class Main : MonoBehaviour
{
    public PheromoneMap Map;
    public Ant Ant;

    public int NumAnts = 1;

    public Rect Spawn;

    private void Start()
    {
        if (null == Ant || null == Map)
        {
            Debug.LogWarning("Ant and Map required.");

            return;
        }

        for (var i = 0; i < NumAnts; i++)
        {
            var ant = Object.Instantiate(Ant);
            ant.transform.position = new Vector3(
                Random.Range(Spawn.xMin, Spawn.xMax),
                0f,
                Random.Range(Spawn.yMin, Spawn.yMax));
            ant.Initialize(Map);
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        Gizmos.DrawWireCube(
            new Vector3(
                Spawn.center.x,
                0f,
                Spawn.center.y),
            new Vector3(
                Spawn.width,
                0f,
                Spawn.height));

        Gizmos.color = Color.white;
        Gizmos.DrawWireCube(
            Vector3.zero,
            new Vector3(
                Map.Size,
                0f,
                Map.Size));
    }
}