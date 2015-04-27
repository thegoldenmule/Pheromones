using UnityEngine;

public class PheromoneViewer : MonoBehaviour
{
    public PheromoneMap Map;

    public int R;
    public int G;
    public int B;

    public bool AutodetectRange = true;

    public float Min = 0f;
    public float Max = 1f;

    /// <summary>
    /// Material to use to present a visualization.
    /// </summary>
    public Material Material;

    /// <summary>
    /// Color buffer for pheromone visualization.
    /// </summary>
    private Color[] _colors;

    /// <summary>
    /// Texture to update for visualization.
    /// </summary>
    private Texture2D _texture;

    /// <summary>
    /// Called to initialize textures and such.
    /// </summary>
    private void Start()
    {
        PrepTexture();
        PrepQuad();
    }

    /// <summary>
    /// Updates the visualization of the pheromone map.
    /// </summary>
    private void Update()
    {
        if (AutodetectRange)
        {
            DetectRange();
        }

        Max = Mathf.Max(Max, 1);
        Min = Mathf.Clamp(Min, 0f, Max - 1f);

        for (int x = 0, xlen = Map.Pheromones.GetLength(0); x < xlen; x++)
        {
            for (int y = 0, ylen = Map.Pheromones.GetLength(1); y < ylen; y++)
            {
                var values = Map.Pheromones[x, y];
                _colors[x + y * Map.Resolution] = new Color(
                    Mathf.Max(0f, values[R] - Min) / (Max - Min),
                    Mathf.Max(0f, values[G] - Min) / (Max - Min),
                    Mathf.Max(0f, values[B] - Min) / (Max - Min), 1);
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
            Map.Resolution,
            Map.Resolution,
            TextureFormat.RGBA32,
            true);
        _colors = new Color[Map.Resolution * Map.Resolution];
    }

    /// <summary>
    /// Prepares a quad for visualizing the map.
    /// </summary>
    private void PrepQuad()
    {
        Material.mainTexture = _texture;
        gameObject.AddComponent<MeshRenderer>().material = Material;

        var mesh = gameObject.AddComponent<MeshFilter>().mesh = new Mesh();
        mesh.vertices = new[]
        {
            new Vector3(-Map.Size / 2f, 0f, -Map.Size / 2f),
            new Vector3(Map.Size / 2f, 0f, -Map.Size / 2f),
            new Vector3(Map.Size / 2f, 0f, Map.Size / 2f),
            new Vector3(-Map.Size / 2f, 0f, Map.Size / 2f),
        };
        mesh.uv = new[]
        {
            new Vector2(0, 0),
            new Vector2(1, 0),
            new Vector2(1, 1),
            new Vector2(0, 1)
        };
        mesh.triangles = new[]
        {
            0, 2, 1,
            0, 3, 2
        };
    }

    /// <summary>
    /// Detects the range.
    /// </summary>
    private void DetectRange()
    {
        Min = 0f;

        // find max pheromone!
        var max = float.MinValue;
        for (int x = 0, xlen = Map.Pheromones.GetLength(0); x < xlen; x++)
        for (int y = 0, ylen = Map.Pheromones.GetLength(1); y < ylen; y++)
        {
            var value = Map.Pheromones[x, y];
            for (int p = 0, plen = value.Length; p < plen; p++)
            {
                if (value[p] > max)
                {
                    max = value[p];
                }
            }
        }

        Max = Mathf.Max(1f, max, Max);
    }
}