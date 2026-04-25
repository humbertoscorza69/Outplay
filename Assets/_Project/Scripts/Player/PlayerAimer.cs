using UnityEngine;

namespace Outplay.Player
{
    /// <summary>
    /// Rotates a child visual transform to face the mouse aim point.
    /// The root transform is intentionally not rotated so WASD remains world-relative.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerAimer : MonoBehaviour
    {
        [Header("References")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private Transform visualTransform;

        [Header("Rotation Tuning")]
        [Tooltip("If true, rotation is smoothed at rotationSpeed degrees/sec. Default false for skillshot precision.")]
        [SerializeField] private bool smoothRotation = false;
        [SerializeField] private float rotationSpeed = 720f;

        private void Awake()
        {
            if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        }

        private void LateUpdate()
        {
            if (visualTransform == null) return;

            var aim = playerInput.AimWorldPosition;
            var dir = aim - (Vector2)transform.position;
            if (dir.sqrMagnitude < 0.0001f) return;

            // Sprite "front" is local +Y; subtract 90 so a 0 rotation aligns +Y with +X-axis aim.
            var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg - 90f;
            var target = Quaternion.Euler(0f, 0f, angle);

            visualTransform.rotation = smoothRotation
                ? Quaternion.RotateTowards(visualTransform.rotation, target, rotationSpeed * Time.deltaTime)
                : target;
        }
    }
}
