using System;
using Outplay.Core;
using UnityEngine;

namespace Outplay.Player
{
    /// <summary>
    /// Authoritative mana container with constant deterministic per-second regen.
    /// Implements IResourceProvider for HUD/UI binding. Per the Skill First pillar:
    /// regen has no RNG.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerMana : MonoBehaviour, IResourceProvider
    {
        [Header("Mana Tuning")]
        [SerializeField] private float maxMana = 100f;
        [SerializeField] private float manaRegenPerSecond = 5f;

        private float currentMana;

        public float Current => currentMana;
        public float Max => maxMana;

        public event Action<float, float> OnChanged;

        private void Awake()
        {
            currentMana = maxMana;
            OnChanged?.Invoke(currentMana, maxMana);
        }

        private void Update()
        {
            if (currentMana >= maxMana) return;
            currentMana = Mathf.Min(currentMana + manaRegenPerSecond * Time.deltaTime, maxMana);
            OnChanged?.Invoke(currentMana, maxMana);
        }

        public bool TrySpend(float amount)
        {
            if (amount <= 0f || currentMana < amount) return false;
            currentMana -= amount;
            OnChanged?.Invoke(currentMana, maxMana);
            return true;
        }

        public void Restore(float amount)
        {
            if (amount <= 0f) return;
            currentMana = Mathf.Min(currentMana + amount, maxMana);
            OnChanged?.Invoke(currentMana, maxMana);
        }
    }
}
