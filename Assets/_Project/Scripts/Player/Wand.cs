using Outplay.Combat;
using UnityEngine;

namespace Outplay.Player
{
    /// <summary>
    /// Player's casting component. Listens for fire input, gates on cooldown
    /// and mana, then asks the projectile pool to spawn a configured shot.
    /// </summary>
    [DisallowMultipleComponent]
    public class Wand : MonoBehaviour
    {
        [Header("Attack")]
        [SerializeField] private ProjectileConfig basicAttackConfig;
        [SerializeField] private float manaCostPerShot = 5f;
        [SerializeField] private float fireCooldown = 0.25f;

        [Header("References")]
        [SerializeField] private ProjectilePool projectilePool;
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerMana playerMana;
        [SerializeField] private PlayerVisual playerVisual;

        private float cooldownTimer;

        private void Awake()
        {
            if (playerInput == null) playerInput = GetComponent<PlayerInput>();
            if (playerMana == null) playerMana = GetComponent<PlayerMana>();
        }

        private void OnEnable()
        {
            playerInput.OnFirePressed += HandleFirePressed;
        }

        private void OnDisable()
        {
            if (playerInput != null) playerInput.OnFirePressed -= HandleFirePressed;
        }

        private void Update()
        {
            if (cooldownTimer > 0f) cooldownTimer -= Time.deltaTime;
        }

        private void HandleFirePressed()
        {
            if (cooldownTimer > 0f) return;
            if (basicAttackConfig == null || projectilePool == null || playerVisual == null) return;

            var tip = playerVisual.WandTip;
            if (tip == null) return;

            if (!playerMana.TrySpend(manaCostPerShot)) return;

            projectilePool.Spawn(tip.position, tip.up, basicAttackConfig, gameObject);
            cooldownTimer = fireCooldown;
        }
    }
}
