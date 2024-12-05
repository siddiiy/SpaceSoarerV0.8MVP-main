using System.Collections;
using UnityEngine;

public class BackgroundScroller : MonoBehaviour
{
    public float scrollSpeed = 0.5f;
    private float offset;
    private Material mat;
    public Texture[] backgrounds;   // Array to store background textures
    public float changeInterval = 10f; // Time interval between background changes 
    public float fadeDuration = 0.1f;   // Duration of the fade effect
    private int currentBackgroundIndex = 0;
    private float timeSinceLastChange = 0f;
    private Coroutine fadeCoroutine;

    public RandomRockSpawner randomRockSpawner;  // Reference to the RandomRockSpawner script

    void Start()
    {
        mat = GetComponent<Renderer>().material;

        if (mat == null)
        {
            Debug.LogError("Material is missing. Ensure the Renderer has a material assigned.");
            return;
        }

        // Set material properties for transparency
        mat.SetFloat("_Mode", 2);
        mat.EnableKeyword("_ALPHABLEND_ON");
        mat.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        mat.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        mat.SetInt("_ZWrite", 0);
        mat.DisableKeyword("_ALPHATEST_ON");
        mat.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        mat.renderQueue = 1000;

        if (backgrounds == null || backgrounds.Length == 0)
        {
            Debug.LogError("Background textures are missing. Ensure at least one texture is assigned.");
            return;
        }

        mat.mainTexture = backgrounds[currentBackgroundIndex];
    }

    void Update()
    {
        timeSinceLastChange += Time.deltaTime;

        if (timeSinceLastChange >= changeInterval)
        {
            timeSinceLastChange = 0f;
            if (fadeCoroutine != null)
            {
                StopCoroutine(fadeCoroutine);
            }
            fadeCoroutine = StartCoroutine(FadeBackground());
        }

        offset += Time.deltaTime * scrollSpeed;
        mat.mainTextureOffset = new Vector2(offset, 0);
    }

    IEnumerator FadeBackground()
    {
        if (mat == null || backgrounds == null || backgrounds.Length == 0)
        {
            Debug.LogError("FadeBackground cannot proceed due to missing material or textures.");
            yield break;
        }

        int nextBackgroundIndex = (currentBackgroundIndex + 1) % backgrounds.Length;

        // Ensure the material starts fully opaque
        Color color = mat.color;
        color.a = 1f;
        mat.color = color;

        // Fade out to black
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            color = Color.Lerp(mat.color, Color.black, normalizedTime); // Interpolates to black
            mat.color = color;
            yield return null;
        }

        // Set the material color to black explicitly to avoid any residual colors
        mat.color = Color.black;

        // Switch to the next background texture
        mat.mainTexture = backgrounds[nextBackgroundIndex];
        currentBackgroundIndex = nextBackgroundIndex;

        // Notify RandomRockSpawner of the new theme
        if (randomRockSpawner != null)
        {
            randomRockSpawner.UpdateTheme(currentBackgroundIndex);
        }

        // Fade in from black to the new background's full color
        Color targetColor = Color.white; // Assuming the new background's color should be fully white
        for (float t = 0; t < fadeDuration; t += Time.deltaTime)
        {
            float normalizedTime = t / fadeDuration;
            color = Color.Lerp(Color.black, targetColor, normalizedTime); // Interpolates from black to target color
            mat.color = color;
            yield return null;
        }

        // Ensure fully opaque final color
        color = targetColor;
        color.a = 1f; // Fully opaque
        mat.color = color;
    }


}
