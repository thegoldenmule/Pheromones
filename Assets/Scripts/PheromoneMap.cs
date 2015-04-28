using System;
using System.Collections.Generic;   

using UnityEngine;

/// <summary>
/// A reading for a single pheromone.
/// </summary>
[Serializable]
public class PheromoneReading
{
    /// <summary>
    /// False if none of the pheromone is detected.
    /// </summary>
    public bool Detected = false;

    /// <summary>
    /// Maximum value of the scent.
    /// </summary>
    public float Max;

    /// <summary>
    /// Direction in which scent is strongest.
    /// </summary>
    public Vector3 MaxDirection;

    /// <summary>
    /// Minimum value of scent.
    /// </summary>
    public float Min;

    /// <summary>
    /// Direction in which scent is weakest.
    /// </summary>
    public Vector3 MinDirection;
}

/// <summary>
/// Keeps track of pheromones + a decay model.
/// </summary>
public class PheromoneMap : MonoBehaviour
{
    /// <summary>
    /// Number of pheromones.
    /// </summary>
    public int NumPheromones = 3;

    /// <summary>
    /// Size, in world space, of the map.
    /// </summary>
    public int Size = 64;

    /// <summary>
    /// Resolution of the map.
    /// </summary>
    public int Resolution = 256;

    /// <summary>
    /// Amount of decay per second.
    /// </summary>
    public float DecayPerSecond = 0.001f;

    /// <summary>
    /// Controls how the pheromones disperse.
    /// </summary>
    public float Dispersion = 0.01f;

    /// <summary>
    /// Map of pheromones.
    /// </summary>
    public float[,][] Pheromones;

    /// <summary>
    /// Bounds of the map.
    /// </summary>
    public Rect Bounds
    {
        get
        {
            return new Rect(
                -Size / 2f,
                -Size / 2f,
                Size,
                Size);
        }
    }

    /// <summary>
    /// Reads pheromone values at a point.
    /// </summary>
    public void Read(
        Vector3 point,
        float radius,
        ref PheromoneReading[] readings)
    {
        float u = Mathf.Clamp((point.x + Size / 2f) / Size, 0f, 1f);
        float v = Mathf.Clamp((point.z + Size / 2f) / Size, 0f, 1f);

        var discretizedRadius = Mathf.CeilToInt(radius);

        // get bounding box
        var x_dc = Mathf.FloorToInt(u * (Resolution - 1));
        var y_dc = Mathf.FloorToInt(v * (Resolution - 1));

        var x_min = Mathf.Max(0, x_dc - discretizedRadius);
        var y_min = Mathf.Max(0, y_dc - discretizedRadius);

        var x_max = Mathf.Min(Resolution - 1, x_dc + discretizedRadius);
        var y_max = Mathf.Min(Resolution - 1, y_dc + discretizedRadius);

        // normalized center of reading
        var x_c = u * (Resolution - 1);
        var y_c = v * (Resolution - 1);

        // smell for each pheromone
        for (int p = 0; p < NumPheromones; p++)
        {
            var reading = readings[p];
            reading.Detected = false;

            var minDirection = Vector3.zero;
            var maxDirection = Vector3.zero;

            var min = float.MaxValue;
            var max = float.MinValue;

            // smell in bounding box
            for (int x = x_min; x <= x_max; x++)
            {
                for (int y = y_min; y <= y_max; y++)
                {
                    var value = Pheromones[x, y][p];
                    
                    if (value > Mathf.Epsilon)
                    {
                        if (value > max)
                        {
                            max = value;
                            reading.Detected = true;

                            maxDirection = new Vector3(
                                x - x_c,
                                0f,
                                y - y_c);
                        }
                        else if (value < min)
                        {
                            min = value;
                            reading.Detected = true;

                            minDirection = new Vector3(
                                x - x_c,
                                0f,
                                y - y_c);
                        }
                    }
                }
            }

            if (reading.Detected)
            {
                reading.Min = min;
                reading.Max = max;
                reading.MinDirection = minDirection.normalized;
                reading.MaxDirection = maxDirection.normalized;
            }
        }
    }

    /// <summary>
    /// Emits a pheromone at a specific spot.
    /// </summary>
    public void Emit(
        Vector3 point,
        float radius,
        int pheromone,
        float value)
    {
        float u = Mathf.Clamp((point.x + Size / 2f) / Size, 0f, 1f);
        float v = Mathf.Clamp((point.z + Size / 2f) / Size, 0f, 1f);

        var discretizedRadius = Mathf.CeilToInt(radius);

        // get bounding box
        var x_dc = Mathf.FloorToInt(u * (Resolution - 1));
        var y_dc = Mathf.FloorToInt(v * (Resolution - 1));

        var x_min = Mathf.Max(0, x_dc - discretizedRadius);
        var y_min = Mathf.Max(0, y_dc - discretizedRadius);

        var x_max = Mathf.Min(Resolution - 1, x_dc + discretizedRadius);
        var y_max = Mathf.Min(Resolution - 1, y_dc + discretizedRadius);

        // set values
        var x_c = u * (Resolution - 1);
        var y_c = v * (Resolution - 1);

        radius = radius * Resolution / Size;

        for (int x = x_min; x <= x_max; x++)
        {
            for (int y = y_min; y <= y_max; y++)
            {
                var distance = Mathf.Sqrt((x - x_c) * (x - x_c) + (y - y_c) * (y - y_c));
                var ratio = Mathf.Clamp(1 - distance / radius, 0, 1);

                Pheromones[x, y][pheromone] = Pheromones[x, y][pheromone] + ratio * value;
            }
        }
    }

    /// <summary>
    /// Called to initialize the map.
    /// </summary>
    private void Awake()
    {
        Resolution = Mathf.Max(1, Resolution);

        PrepPheromoneMap();
    }

    /// <summary>
    /// Called every frame. This applies blur and decay to the pheromones so
    /// that they wis away over time.
    /// </summary>
    private void Update()
    {
        var dt = Time.deltaTime;

        // apply convolution matrix
        // TODO: skip convolution, 2x speedup with just grabbing adjacents
        var dispersion = Dispersion * dt;
        var center = 1f - dispersion * 4;
        var convolution = new float[,]
        {
            {   0,      dispersion,    0   },
            {   dispersion,    center,    dispersion },
            {   0,      dispersion,    0   }
        };

        var convolutionSize = convolution.GetLength(0);
        var halfConvolutionSize = convolutionSize / 2;

        var decay = DecayPerSecond * dt;

        var results = new float[NumPheromones];

        for (int x = 0, xlen = Pheromones.GetLength(0); x < xlen; x++)
        for (int y = 0, ylen = Pheromones.GetLength(1); y < ylen; y++)
        {
            for (int p = 0; p < NumPheromones; p++)
            {
                results[p] = 0;
            }

            for (int i = 0; i < convolutionSize; i++)
            for (int j = 0; j < convolutionSize; j++)
            {
                int imageX = (x - halfConvolutionSize + i + xlen) % xlen;
                int imageY = (y - halfConvolutionSize + j + ylen) % ylen;

                for (int p = 0; p < NumPheromones; p++)
                {
                    results[p] += Pheromones[imageX, imageY][p] * convolution[i, j];
                }
            }

            for (int p = 0; p < NumPheromones; p++)
            {
                Pheromones[x, y][p] = Mathf.Max(
                    results[p] - decay,
                    0f);
            }
        }
    }

    /// <summary>
    /// Prepares the data structure that will hold the pheromones.
    /// </summary>
    private void PrepPheromoneMap()
    {
        Pheromones = new float[
            Resolution,
            Resolution][];
        for (int x = 0, xlen = Pheromones.GetLength(0); x < xlen; x++)
        {
            for (int y = 0, ylen = Pheromones.GetLength(1); y < ylen; y++)
            {
                Pheromones[x, y] = new float[NumPheromones];
            }
        }
    }

    /// <summary>
    /// Retrieves array of blank readings.
    /// </summary>
    /// <param name="numPheromones"></param>
    /// <returns></returns>
    public static PheromoneReading[] Readings(int numPheromones)
    {
        var readings = new PheromoneReading[numPheromones];
        
        for (int i = 0; i < readings.Length; i++)
        {
            readings[i] = new PheromoneReading();
        }

        return readings;
    }
}