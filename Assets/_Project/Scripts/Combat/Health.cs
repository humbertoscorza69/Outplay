using System;
using Outplay.Core;
using UnityEngine;

namespace Outplay.Combat
{
    /// <summary>
    /// Generic damageable HP container. Used by player, enemies, dummies, destructibles.
    /// Implements IResourceProvider so any UI bar can mirror it; implements IDamageable
    /// as the entry point for projectile/melee/ability damage.
    /// </summary>
    [DisallowMultipleComponent]
    public class Health : MonoBehaviour, IResourceProvider, IDamageable
    {
        [Header("Health Tuning")]
        [SerializeField] private float maxHP = 100f;

        private float currentHP;
        private bool isDead;

        public float Current => currentHP;
        public float Max => maxHP;
        public bool IsDead => isDead;

        public event Action<float, float> OnChanged;
        public event Action OnDeath;

        private void Awake()
        {
            currentHP = maxHP;
            OnChanged?.Invoke(currentHP, maxHP);
        }

        public void TakeDamage(float amount, GameObject source)
        {
            if (isDead || amount <= 0f) return;
            currentHP = Mathf.Max(currentHP - amount, 0f);
            OnChanged?.Invoke(currentHP, maxHP);
            if (currentHP <= 0f)
            {
                isDead = true;
                OnDeath?.Invoke();
            }
        }

        public void Heal(float amount)
        {
            if (isDead || amount <= 0f) return;
            currentHP = Mathf.Min(currentHP + amount, maxHP);
            OnChanged?.Invoke(currentHP, maxHP);
        }

        public void HealToFull()
        {
            isDead = false;
            currentHP = maxHP;
            OnChanged?.Invoke(currentHP, maxHP);
        }
    }
}
