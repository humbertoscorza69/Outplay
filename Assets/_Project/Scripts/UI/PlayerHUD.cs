using UnityEngine;
using Outplay.Player;

namespace Outplay.UI
{
    /// <summary>
    /// Wires player resources to ResourceBar instances. Pure plumbing —
    /// rendering happens inside each ResourceBar.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerHUD : MonoBehaviour
    {
        [Header("Sources")]
        [SerializeField] private PlayerHealth playerHealth;
        [SerializeField] private PlayerMana playerMana;

        [Header("Bars")]
        [SerializeField] private ResourceBar hpBar;
        [SerializeField] private ResourceBar manaBar;

        private void Start()
        {
            if (playerHealth == null || playerMana == null || hpBar == null || manaBar == null)
            {
                Debug.LogError($"[PlayerHUD] '{name}' is missing references. HUD will not function.", this);
                enabled = false;
                return;
            }

            hpBar.Bind(playerHealth);
            manaBar.Bind(playerMana);
        }
    }
}
