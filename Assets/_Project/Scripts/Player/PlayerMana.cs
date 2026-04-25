using System;
using UnityEngine;

namespace Outplay.Player
{
    /// <summary>
    /// Authoritative mana container with constant deterministic per-second regen.
    /// Per the Skill First pillar: regen has no RNG.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerMana : MonoBehaviour
    {
        [Header("Mana Tuning")]
        [SerializeField] private float maxMana = 100f;
        [SerializeField] private float manaRegenPerSecond = 5f;

        private float currentMana;

        public float CurrentMana => currentMana;
        public float MaxMana => maxMana;

        public event Action<float, float> OnManaChanged;

        private void Awake()
        {
            currentMana = maxMana;
            OnManaChanged?.Invoke(currentMana, maxMana);
        }

        private void Update()
        {
            if (currentMana >= maxMana) return;
            currentMana = Mathf.Min(currentMana + manaRegenPerSecond * Time.deltaTime, maxMana);
            OnManaChanged?.Invoke(currentMana, maxMana);
        }

        public bool TrySpend(float amount)
        {
            if (amount <= 0f || currentMana < amount) return false;
            currentMana -= amount;
            OnManaChanged?.Invoke(currentMana, maxMana);
            return true;
        }

        public void Restore(float amount)
        {
            if (amount <= 0f) return;
            currentMana = Mathf.Min(currentMana + amount, maxMana);
            OnManaChanged?.Invoke(currentMana, maxMana);
        }
    }
}
