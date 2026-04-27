using UnityEngine;
using UnityEngine.Pool;

namespace Outplay.Combat
{
    /// <summary>
    /// Wraps Unity's ObjectPool around a Projectile prefab and provides a
    /// Spawn API. All pooled projectiles are parented under this transform
    /// to keep the hierarchy organized.
    /// </summary>
    [DisallowMultipleComponent]
    public class ProjectilePool : MonoBehaviour
    {
        [Header("Pool Source")]
        [SerializeField] private Projectile projectilePrefab;

        [Header("Pool Sizing")]
        [SerializeField] private int defaultCapacity = 32;
        [SerializeField] private int maxSize = 64;

        private IObjectPool<Projectile> pool;

        private void Awake()
        {
            pool = new ObjectPool<Projectile>(
                createFunc: CreateProjectile,
                actionOnGet: p => p.gameObject.SetActive(true),
                actionOnRelease: p => p.gameObject.SetActive(false),
                actionOnDestroy: p => Destroy(p.gameObject),
                collectionCheck: false,
                defaultCapacity: defaultCapacity,
                maxSize: maxSize);
        }

        public void Spawn(Vector2 origin, Vector2 direction, ProjectileConfig config, GameObject casterRoot)
        {
            if (projectilePrefab == null || config == null) return;
            var p = pool.Get();
            p.Activate(origin, direction, config, pool, casterRoot);
        }

        private Projectile CreateProjectile()
        {
            var p = Instantiate(projectilePrefab, transform);
            p.gameObject.SetActive(false);
            return p;
        }
    }
}
