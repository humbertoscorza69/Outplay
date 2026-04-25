using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Outplay.Player;

namespace Outplay.UI
{
    /// <summary>
    /// HUD that mirrors PlayerHealth and PlayerMana into UI bars.
    /// Direct typed references; subscribes via events, never polls.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerHUD : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private PlayerMana playerMana;

        [Header("HP Bar")]
        [SerializeField] private Image hpFill;
        [SerializeField] private TextMeshProUGUI hpText;

        [Header("Mana Bar")]
        [SerializeField] private Image manaFill;
        [SerializeField] private TextMeshProUGUI manaText;

        private void OnEnable()
        {
            if (playerHealth == null || playerMana == null)
            {
                Debug.LogError($"[PlayerHUD] '{name}' is missing playerHealth and/or playerMana references. HUD will not function.", this);
                enabled = false;
                return;
            }

            playerHealth.OnHealthChanged += HandleHealthChanged;
            playerMana.OnManaChanged += HandleManaChanged;
        }

        private void Start()
        {
            // Initial sync runs in Start, after all Awake methods have completed
            // across the scene, so PlayerHealth/PlayerMana have initialized state.
            HandleHealthChanged(playerHealth.CurrentHP, playerHealth.MaxHP);
            HandleManaChanged(playerMana.CurrentMana, playerMana.MaxMana);
        }

        private void OnDisable()
        {
            if (playerHealth != null) playerHealth.OnHealthChanged -= HandleHealthChanged;
            if (playerMana != null) playerMana.OnManaChanged -= HandleManaChanged;
        }

        private void HandleHealthChanged(float current, float max)
        {
            hpFill.fillAmount = max > 0f ? current / max : 0f;
            hpText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
        }

        private void HandleManaChanged(float current, float max)
        {
            manaFill.fillAmount = max > 0f ? current / max : 0f;
            manaText.text = $"{Mathf.CeilToInt(current)} / {Mathf.CeilToInt(max)}";
        }
    }
}
