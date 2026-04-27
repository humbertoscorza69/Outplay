using System;
using UnityEngine;
using UnityEngine.Pool;

namespace Outplay.Combat
{
    /// <summary>
    /// Pooled projectile. Move/lifetime owned here; on contact with an IDamageable
    /// it deals config.Damage and despawns. The casterRoot GameObject is filtered
    /// out so projectiles never hit their own caster (or any of the caster's children).
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(CircleCollider2D))]
    [RequireComponent(typeof(SpriteRenderer))]
    public class Projectile : MonoBehaviour
    {
        [Header("Visual Tuning")]
        [SerializeField] private Color color = new Color(1f, 0.55f, 0.1f, 1f);
        [SerializeField] private float visualRadius = 0.12f;
        [SerializeField] private int pixelsPerUnit = 100;

        private static Sprite sharedSprite;

        private Rigidbody2D body;
        private CircleCollider2D circleCollider;
        private SpriteRenderer spriteRenderer;

        private ProjectileConfig config;
        private IObjectPool<Projectile> owner;
        private GameObject casterRoot;
        private Vector2 velocity;
        private float lifetimeRemaining;
        private bool isActive;

        public event Action<Collider2D> OnHit;
        public ProjectileConfig Config => config;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            circleCollider = GetComponent<CircleCollider2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();

            if (sharedSprite == null) sharedSprite = BuildWhiteCircleSprite();
            spriteRenderer.sprite = sharedSprite;
            spriteRenderer.color = color;
        }

        public void Activate(Vector2 origin, Vector2 direction, ProjectileConfig config,
                             IObjectPool<Projectile> owner, GameObject casterRoot)
        {
            this.config = config;
            this.owner = owner;
            this.casterRoot = casterRoot;

            var dir = direction.sqrMagnitude > 0.0001f ? direction.normalized : Vector2.up;
            body.position = origin;
            body.rotation = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;

            velocity = dir * config.Speed;
            lifetimeRemaining = config.Lifetime;
            circleCollider.radius = config.HitboxRadius;

            isActive = true;
        }

        private void FixedUpdate()
        {
            if (!isActive) return;

            lifetimeRemaining -= Time.fixedDeltaTime;
            if (lifetimeRemaining <= 0f)
            {
                Despawn();
                return;
            }

            body.MovePosition(body.position + velocity * Time.fixedDeltaTime);
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!isActive) return;
            if (casterRoot != null && other.transform.IsChildOf(casterRoot.transform)) return;

            OnHit?.Invoke(other);

            var damageable = other.GetComponentInParent<IDamageable>();
            if (damageable != null)
            {
                damageable.TakeDamage(config.Damage, casterRoot);
                Despawn();
            }
        }

        private void Despawn()
        {
            isActive = false;
            owner?.Release(this);
        }

        private Sprite BuildWhiteCircleSprite()
        {
            var size = Mathf.RoundToInt(visualRadius * 2f * pixelsPerUnit);
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
