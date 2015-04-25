using System;
using UnityEngine;

public class PheromoneMap : MonoBehaviour
{
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
    /// Controls how the pheromones disperse.
    /// </summary>
    public float Dispersion = 0.01f;

    /// <summary>
    /// Factor
    /// </summary>
    public float ConvolutionFactor = 1f;

    /// <summary>
    /// Bias
    /// </summary>
    public float ConvolutionBias = 0f;

    /// <summary>
    /// Blows pheromones!
    /// </summary>
    public Vector2 Wind = Vector2.zero;

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
    public void Pheromones(
        Vector3 point,
        float radius,
        out Vector3[] pheromones)
    {
        float u = Mathf.Clamp((point.x + Size / 2f) / Size, 0f, 1f);
        float v = Mathf.Clamp((point.z + Size / 2f) / Size, 0f, 1f);

        var x = Mathf.RoundToInt(Resolution * u);
        var y = Mathf.RoundToInt(Resolution * v);

        pheromones = null;
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

                _pheromones[x, y][pheromone] = Mathf.Clamp(
                    _pheromones[x, y][pheromone] + ratio * value,
                    0, 1);
            }
        }
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
        UpdateDecay();
        UpdateTexture();
    }

    /// <summary>
    /// Applies decay.
    /// </summary>
    private void UpdateDecay()
    {
        var dt = Time.deltaTime;

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

        for (int x = 0, xlen = _pheromones.GetLength(0); x < xlen; x++)
        for (int y = 0, ylen = _pheromones.GetLength(1); y < ylen; y++)
        {
            float r = 0f, g = 0f, b = 0f;

            for (int i = 0; i < convolutionSize; i++)
            for (int j = 0; j < convolutionSize; j++)
            {
                int imageX = (x - halfConvolutionSize + i + xlen) % xlen;
                int imageY = (y - halfConvolutionSize + j + ylen) % ylen;

                r += _pheromones[imageX, imageY][0] * convolution[i, j];
                g += _pheromones[imageX, imageY][1] * convolution[i, j];
                b += _pheromones[imageX, imageY][2] * convolution[i, j]; 
            }

            var decay = DecayPerSecond * dt;
            _pheromones[x, y][0] = Mathf.Clamp(
                ConvolutionFactor * r + ConvolutionBias - decay,
                0, 1f);
            _pheromones[x, y][1] = Mathf.Clamp(
                ConvolutionFactor * g + ConvolutionBias - decay,
                0, 1f);
            _pheromones[x, y][1] = Mathf.Clamp(
                ConvolutionFactor * b + ConvolutionBias - decay,
                0, 1f);
        }
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
                _pheromones[x, y] = new float[3];
            }
        }
    }
}