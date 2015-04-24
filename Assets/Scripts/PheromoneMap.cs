using System;
using UnityEngine;

public class PheromoneMap : MonoBehaviour
{
    /// <summary>
    /// Number of pheromones to store data for.
    /// </summary>
    public int NumPheromones = 2;

    /// <summary>
    /// Material to use to present a visualization.
    /// </summary>
    public Material Material;

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
    /// Amount of blur to apply per second.
    /// </summary>
    public float BlurPerSecond = 0.001f;

    /// <summary>
    /// Map of pheromones.
    /// </summary>
    private float[,][] _pheromones;

    /// <summary>
    /// Color buffer for pheromone visualization.
    /// </summary>
    private Color[] _colors;

    /// <summary>
    /// Texture to update for visualization.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// Reads pheromone values at a point.
    /// </summary>
    public void Pheromones(
        float u,
        float v,
        ref float[] pheromones)
    {
        
    }

    /// <summary>
    /// Emits a pheromone at a specific spot.
    /// </summary>
    public void Emit(
        float u,
        float v,
        float radius,
        int pheromone,
        float value)
    {
        
    }

    /// <summary>
    /// Called to initialize the map.
    /// </summary>
    private void Awake()
    {
        Resolution = Mathf.Max(1, Resolution);

        PrepTexture();
        PrepQuad();
        PrepPheromoneMap();

        UpdateTexture();
    }

    /// <summary>
    /// Called every frame. This applies blur and decay to the pheromones so
    /// that they wis away over time.
    /// </summary>
    private void Update()
    {
        // apply blur

        UpdateTexture();
    }

    /// <summary>
    /// Updates the visualization of the pheromone map.
    /// </summary>
    private void UpdateTexture()
    {
        for (int x = 0, xlen = _pheromones.GetLength(0); x < xlen; x++)
        {
            for (int y = 0, ylen = _pheromones.GetLength(1); y < ylen; y++)
            {
                var values = _pheromones[x, y];
                _colors[x + y * Resolution] = new Color(
                    values[0],
                    values[1],
                    0, 1);
            }
        }

        _texture.SetPixels(_colors);
        _texture.Apply();
    }

    /// <summary>
    /// Prepares a texture for visualizing the map.
    /// </summary>
    private void PrepTexture()
    {
        _texture = new Texture2D(
            Resolution,
            Resolution,
            TextureFormat.RGBA32,
            true);
        _colors = new Color[Resolution * Resolution];
    }

    /// <summary>
    /// Prepares a quad for visualizing the map.
    /// </summary>
    private void PrepQuad()
    {
        Material.mainTexture = _texture;
        gameObject.AddComponent<MeshRenderer>().material = Material;
        var mesh = gameObject.AddComponent<MeshFilter>().mesh = new Mesh();
        mesh.vertices = new []
        {
            new Vector3(-Size / 2f, 0f, -Size / 2f),
            new Vector3(Size / 2f, 0f, -Size / 2f),
            new Vector3(Size / 2f, 0f, Size / 2f),
            new Vector3(-Size / 2f, 0f, Size / 2f),
        };
        mesh.uv = new []
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };
        mesh.triangles = new []
        {
            0, 2, 1,
            0, 3, 2
        };
    }

    /// <summary>
    /// Prepares the data structure that will hold the pheromones.
    /// </summary>
    private void PrepPheromoneMap()
    {
        _pheromones = new float[
            Resolution,
            Resolution][];
        for (int x = 0, xlen = _pheromones.GetLength(0); x < xlen; x++)
        {
            for (int y = 0, ylen = _pheromones.GetLength(1); y < ylen; y++)
            {
                _pheromones[x, y] = new float[NUM_PHEROMONES];
            }
        }
    }
}