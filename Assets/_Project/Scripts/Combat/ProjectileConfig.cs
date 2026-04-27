using UnityEngine;

namespace Outplay.Combat
{
    /// <summary>
    /// Static stats for a projectile attack (basic wand shot, ability projectile, etc.).
    /// One asset per attack type — Firebolt, Ice Shard, etc.
    /// </summary>
    [CreateAssetMenu(menuName = "Outplay/Combat/Projectile Config", fileName = "ProjectileConfig")]
    public class ProjectileConfig : ScriptableObject
    {
        [SerializeField] private float speed = 14f;
        [SerializeField] private float lifetime = 2f;
        [SerializeField] private float damage = 10f;
        [SerializeField] private float hitboxRadius = 0.15f;

        public float Speed => speed;
        public float Lifetime => lifetime;
        public float Damage => damage;
        public float HitboxRadius => hitboxRadius;
    }
}
