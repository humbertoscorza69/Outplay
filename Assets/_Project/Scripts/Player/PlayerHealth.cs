using System;
using Outplay.Core;
using UnityEngine;

namespace Outplay.Player
{
    /// <summary>
    /// Authoritative HP container. Implements IResourceProvider for HUD/UI binding.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerHealth : MonoBehaviour, IResourceProvider
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

        public void TakeDamage(float amount)
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
    }
}
