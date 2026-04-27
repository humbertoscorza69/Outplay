using Outplay.Combat;
using Outplay.UI;
using UnityEngine;

namespace Outplay.Enemies
{
    /// <summary>
    /// Stationary practice target. Hides on death and respawns at full HP
    /// after a delay. Health bar is bound to its Health component on Start.
    /// </summary>
    [DisallowMultipleComponent]
    public class Dummy : MonoBehaviour
    {
        [Header("Dummy Tuning")]
        [SerializeField] private float respawnDelay = 3f;

        [Header("References")]
        [SerializeField] private Health health;
        [SerializeField] private SpriteRenderer spriteRenderer;
        [SerializeField] private Collider2D dummyCollider;
        [SerializeField] private Canvas healthBarCanvas;
        [SerializeField] private ResourceBar healthBar;

        private void Awake()
        {
            if (health == null) health = GetComponent<Health>();
            if (spriteRenderer == null) spriteRenderer = GetComponent<SpriteRenderer>();
            if (dummyCollider == null) dummyCollider = GetComponent<Collider2D>();
        }

        private void OnEnable()
        {
            if (health != null) health.OnDeath += HandleDeath;
        }

        private void OnDisable()
        {
            if (health != null) health.OnDeath -= HandleDeath;
        }

        private void Start()
        {
            if (healthBar != null) healthBar.Bind(health);
        }

        private void HandleDeath()
        {
            spriteRenderer.enabled = false;
            dummyCollider.enabled = false;
            if (healthBarCanvas != null) healthBarCanvas.enabled = false;
            Invoke(nameof(Respawn), respawnDelay);
        }

        private void Respawn()
        {
            health.HealToFull();
            spriteRenderer.enabled = true;
            dummyCollider.enabled = true;
            if (healthBarCanvas != null) healthBarCanvas.enabled = true;
        }
    }
}
