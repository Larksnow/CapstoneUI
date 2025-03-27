using UnityEngine;
using UnityEngine.UI;

public class GradientBackground : MonoBehaviour
{
    public Color colorBottom = new Color(0.11f, 0.0f, 0.21f);    // Dark purple
    public Color colorTop = new Color(0.37f, 0.17f, 0.8f); // Electric violet
    public int width = 512;
    public int height = 512;
    public RawImage targetImage; // Assign in Inspector

    void Start()
    {
        Texture2D gradientTex = GenerateVerticalGradient(width, height, colorTop, colorBottom);
        targetImage.texture = gradientTex;
    }

    Texture2D GenerateVerticalGradient(int width, int height, Color top, Color bottom)
    {
        Texture2D texture = new Texture2D(width, height);
        texture.wrapMode = TextureWrapMode.Clamp;

        for (int y = 0; y < height; y++)
        {
            float t = (float)y / (height - 1);
            Color blendedColor = Color.Lerp(bottom, top, t);
            for (int x = 0; x < width; x++)
            {
                texture.SetPixel(x, y, blendedColor);
            }
        }

        texture.Apply();
        return texture;
    }
}
