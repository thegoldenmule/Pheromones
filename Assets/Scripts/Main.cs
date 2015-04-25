using UnityEngine;
using System;

using Random = UnityEngine.Random;

public class Main : MonoBehaviour
{
    public PheromoneMap Map;
    public Ant Ant;

    public int NumAnts = 1;

    public Rect Spawn;
    
    public float TimeMultiplier = 1f;

    private void Start()
    {
        if (null == Ant || null == Map)
        {
            Debug.LogWarning("Ant and Map required.");

            return;
        }

        for (var i = 0; i < NumAnts; i++)
        {
            var ant = Instantiate(Ant);
            ant.transform.position = new Vector3(
                Random.Range(Spawn.xMin, Spawn.xMax),
                0f,
                Random.Range(Spawn.yMin, Spawn.yMax));
            ant.Initialize(Map);
        }
    }

    private void Update()
    {
        Time.timeScale = TimeMultiplier;
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