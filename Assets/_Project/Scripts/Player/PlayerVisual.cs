using UnityEngine;

namespace Outplay.Player
{
    /// <summary>
    /// Generates a placeholder elliptical "capsule" body with a facing wedge at runtime.
    /// Lives on the rotating Visual child; the wedge points in local +Y.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SpriteRenderer))]
    public class PlayerVisual : MonoBehaviour
    {
        [Header("Body Tuning")]
        [SerializeField] private Color bodyColor = new Color(0.290f, 0.565f, 0.886f, 1f); // #4A90E2
        [SerializeField, Range(0.5f, 2f)] private float bodyWidth = 0.9f;
        [SerializeField, Range(0.5f, 2f)] private float bodyHeight = 1.1f;

        [Header("Wedge Tuning")]
        [SerializeField] private Color wedgeColor = Color.white;
        [SerializeField, Range(0.05f, 0.5f)] private float wedgeRelativeSize = 0.25f;

        [Header("Sprite Settings")]
        [SerializeField] private int pixelsPerUnit = 100;

        private SpriteRenderer spriteRenderer;

        private void Awake()
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            spriteRenderer.sprite = BuildSprite();
        }

        private Sprite BuildSprite()
        {
            var width = Mathf.RoundToInt(bodyWidth * pixelsPerUnit);
            var height = Mathf.RoundToInt(bodyHeight * pixelsPerUnit);
            var tex = new Texture2D(width, height, TextureFormat.RGBA32, false) { filterMode = FilterMode.Bilinear };

            var pixels = new Color[width * height];
            var clear = new Color(0f, 0f, 0f, 0f);
            for (var i = 0; i < pixels.Length; i++) pixels[i] = clear;

            var cx = (width - 1) * 0.5f;
            var cy = (height - 1) * 0.5f;
            var rx = width * 0.5f;
            var ry = height * 0.5f;

            var wedgeHeight = height * wedgeRelativeSize;
            var wedgeBaseHalfWidth = wedgeHeight * 0.6f;
            var wedgeTipY = height - 1;
            var wedgeBaseY = wedgeTipY - wedgeHeight;

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    var dx = (x - cx) / rx;
                    var dy = (y - cy) / ry;
                    if (dx * dx + dy * dy > 1f) continue;

                    var color = bodyColor;

                    if (y >= wedgeBaseY && y <= wedgeTipY)
                    {
                        var t = (wedgeTipY - y) / wedgeHeight; // 0 at base, 1 at tip
                        var halfWidthAtY = Mathf.Lerp(wedgeBaseHalfWidth, 0f, t);
                        if (Mathf.Abs(x - cx) <= halfWidthAtY) color = wedgeColor;
                    }

                    pixels[y * width + x] = color;
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();

            return Sprite.Create(tex, new Rect(0, 0, width, height), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }
    }
}
