using UnityEngine;

public static class Texture2DExtensions
{
    public static void Fill(this Texture2D @this, Color color)
    {
        var colors = new Color[@this.width * @this.height];
        colors.Fill(color);

        @this.SetPixels(colors);
        @this.Apply();
    }
}