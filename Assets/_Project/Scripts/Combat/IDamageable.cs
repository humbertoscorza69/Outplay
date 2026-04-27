using UnityEngine;

namespace Outplay.Combat
{
    /// <summary>
    /// Anything that takes damage. Source identifies the attacker for
    /// future kill-credit, assist tracking, and bounty systems.
    /// </summary>
    public interface IDamageable
    {
        void TakeDamage(float amount, GameObject source);
    }
}
