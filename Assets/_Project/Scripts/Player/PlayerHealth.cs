using System;
using UnityEngine;

namespace Outplay.Player
{
    /// <summary>
    /// Authoritative HP container. Damage application logic lives elsewhere
    /// (combat sprint); this script only owns the resource itself.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerHealth : MonoBehaviour
    {
        [Header("Health Tuning")]
        [SerializeField] private float maxHP = 100f;

        private float currentHP;
        private bool isDead;

        public float CurrentHP => currentHP;
        public float MaxHP => maxHP;
        public bool IsDead => isDead;

        public event Action<float, float> OnHealthChanged;
        public event Action OnDeath;

        private void Awake()
        {
            currentHP = maxHP;
            OnHealthChanged?.Invoke(currentHP, maxHP);
        }

        public void TakeDamage(float amount)
        {
            if (isDead || amount <= 0f) return;
            currentHP = Mathf.Max(currentHP - amount, 0f);
            OnHealthChanged?.Invoke(currentHP, maxHP);
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
            OnHealthChanged?.Invoke(currentHP, maxHP);
        }
    }
}
