using UnityEngine;

namespace Outplay.Player
{
    /// <summary>
    /// Weighted movement with separate acceleration and deceleration.
    /// Sole writer to the Rigidbody2D; PlayerDash communicates via velocity overrides.
    /// </summary>
    [DisallowMultipleComponent]
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMover : MonoBehaviour
    {
        [Header("Movement Tuning")]
        [SerializeField] private float topSpeed = 7f;
        [SerializeField] private float acceleration = 60f;
        [SerializeField] private float deceleration = 50f;

        [Header("References")]
        [SerializeField] private PlayerInput playerInput;

        private Rigidbody2D body;
        private Vector2 currentVelocity;
        private bool hasOverride;
        private Vector2 overrideVelocity;

        public Vector2 Velocity => currentVelocity;

        private void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            if (playerInput == null) playerInput = GetComponent<PlayerInput>();
        }

        private void FixedUpdate()
        {
            if (hasOverride)
            {
                currentVelocity = overrideVelocity;
            }
            else
            {
                var input = playerInput.MoveInput;
                if (input.sqrMagnitude > 1f) input = input.normalized;

                var target = input * topSpeed;
                var rate = input.sqrMagnitude > 0f ? acceleration : deceleration;
                currentVelocity = Vector2.MoveTowards(currentVelocity, target, rate * Time.fixedDeltaTime);
            }

            body.MovePosition(body.position + currentVelocity * Time.fixedDeltaTime);
        }

        public void SetVelocityOverride(Vector2 velocity)
        {
            hasOverride = true;
            overrideVelocity = velocity;
        }

        public void ClearVelocityOverride()
        {
            hasOverride = false;
        }
    }
}
