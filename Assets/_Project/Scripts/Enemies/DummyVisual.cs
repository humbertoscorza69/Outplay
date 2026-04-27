using UnityEngine;

namespace Outplay.Enemies
{
    /// <summary>
    /// Generates a placeholder gray circle sprite for the dummy at runtime.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class DummyVisual : MonoBehaviour
    {
        [SerializeField] private Color bodyColor = new Color(0.6f, 0.6f, 0.6f, 1f);
        [SerializeField, Range(0.5f, 3f)] private float diameter = 1.4f;
        [SerializeField] private int pixelsPerUnit = 100;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = BuildCircleSprite();
            spriteRenderer.color = bodyColor;
        }

        private Sprite BuildCircleSprite()
        {
            var size = Mathf.RoundToInt(diameter * pixelsPerUnit);
            var tex = new Texture2D(size, size, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear };

            var pixels = new Color[size * size];
            var clear = new Color(0f, 0f, 0f, 0f);
            for (var i = 0; i < pixels.Length; i++) pixels[i] = clear;

            var center = (size - 1) * 0.5f;
            var radius = size * 0.5f;
            for (var y = 0; y < size; y++)
            {
                for (var x = 0; x < size; x++)
                {
                    var dx = x - center;
                    var dy = y - center;
                    if (dx * dx + dy * dy > radius * radius) continue;
                    pixels[y * size + x] = Color.white;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, size, size), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }
    }
}
