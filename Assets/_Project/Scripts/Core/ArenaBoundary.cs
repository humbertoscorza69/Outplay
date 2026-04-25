using UnityEngine;

namespace Outplay.Core
{
    /// <summary>
    /// Spawns a placeholder arena at runtime: a flat background quad and four
    /// thin boundary walls. Sprint 0 placeholder; replace with authored art and
    /// proper colliders when level art arrives.
    /// </summary>
    [DisallowMultipleComponent]
    public class ArenaBoundary : MonoBehaviour
    {
        [Header("Arena")]
        [SerializeField] private Vector2 arenaSize = new Vector2(20f, 20f);
        [SerializeField] private float wallThickness = 0.2f;
        [SerializeField] private Color wallColor = Color.black;

        [Header("Background")]
        [SerializeField] private Vector2 backgroundSize = new Vector2(24f, 24f);
        [SerializeField] private Color backgroundColor = new Color(0.18f, 0.18f, 0.18f, 1f);

        [Header("Sprite Settings")]
        [SerializeField] private int pixelsPerUnit = 100;

        private void Awake()
        {
            var sprite = BuildWhiteUnitSprite();

            CreateQuad("Background", Vector2.zero, backgroundSize, backgroundColor, sprite, -10);

            var halfX = arenaSize.x * 0.5f;
            var halfY = arenaSize.y * 0.5f;
            CreateQuad("Wall_Top",    new Vector2(0f,  halfY), new Vector2(arenaSize.x, wallThickness), wallColor, sprite, 0);
            CreateQuad("Wall_Bottom", new Vector2(0f, -halfY), new Vector2(arenaSize.x, wallThickness), wallColor, sprite, 0);
            CreateQuad("Wall_Left",   new Vector2(-halfX, 0f), new Vector2(wallThickness, arenaSize.y), wallColor, sprite, 0);
            CreateQuad("Wall_Right",  new Vector2( halfX, 0f), new Vector2(wallThickness, arenaSize.y), wallColor, sprite, 0);
        }

        private void CreateQuad(string objName, Vector2 position, Vector2 size, Color color, Sprite sprite, int sortingOrder)
        {
            var go = new GameObject(objName);
            go.transform.SetParent(transform, false);
            go.transform.localPosition = position;
            go.transform.localScale = new Vector3(size.x, size.y, 1f);

            var sr = go.AddComponent<SpriteRenderer>();
            sr.sprite = sprite;
            sr.color = color;
            sr.sortingOrder = sortingOrder;
        }

        private Sprite BuildWhiteUnitSprite()
        {
            var tex = new Texture2D(1, 1, TextureFormat.RGBA32, false) { filterMode = FilterMode.Point };
            tex.SetPixel(0, 0, Color.white);
            tex.Apply();
            return Sprite.Create(tex, new Rect(0, 0, 1, 1), new Vector2(0.5f, 0.5f), pixelsPerUnit);
        }
    }
}
