using UnityEngine;

namespace Outplay.Player
{
    /// <summary>
    /// Dash state machine: Idle -> Dashing -> Cooldown -> Idle.
    /// Drives PlayerMover's velocity override during the dash window.
    /// Exposes IsInvulnerable for combat systems to query during i-frames.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerDash : MonoBehaviour
    {
        [Header("Dash Tuning")]
        [SerializeField] private float dashDistance = 4f;
        [SerializeField] private float dashDuration = 0.18f;
        [SerializeField] private float dashCooldown = 1.2f;
        [SerializeField] private float iFrameWindow = 0.12f;

        [Header("References")]
        [SerializeField] private PlayerInput playerInput;
        [SerializeField] private PlayerMover playerMover;

        private enum State { Idle, Dashing, Cooldown }
        private State state = State.Idle;
        private float stateTimer;
        private float iFrameTimer;

        public bool IsDashing => state == State.Dashing;
        public bool IsInvulnerable => iFrameTimer > 0f;
        public bool CanDash => state == State.Idle;

        private void Awake()
        {
            if (playerInput == null) playerInput = GetComponent<PlayerInput>();
            if (playerMover == null) playerMover = GetComponent<PlayerMover>();
        }

        private void OnEnable()
        {
            playerInput.OnDashPressed += HandleDashPressed;
        }

        private void OnDisable()
        {
            playerInput.OnDashPressed -= HandleDashPressed;
        }

        private void Update()
        {
            if (iFrameTimer > 0f) iFrameTimer -= Time.deltaTime;

            switch (state)
            {
                case State.Dashing:
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0f)
                    {
                        playerMover.ClearVelocityOverride();
                        stateTimer = dashCooldown;
                        state = State.Cooldown;
                    }
                    break;

                case State.Cooldown:
                    stateTimer -= Time.deltaTime;
                    if (stateTimer <= 0f) state = State.Idle;
                    break;
            }
        }

        private void HandleDashPressed()
        {
            if (state != State.Idle) return;

            var direction = ResolveDashDirection();
            if (direction.sqrMagnitude < 0.0001f) return;

            var dashSpeed = dashDistance / dashDuration;
            playerMover.SetVelocityOverride(direction * dashSpeed);

            state = State.Dashing;
            stateTimer = dashDuration;
            iFrameTimer = iFrameWindow;
        }

        private Vector2 ResolveDashDirection()
        {
            var input = playerInput.MoveInput;
            if (input.sqrMagnitude > 0.01f) return input.normalized;

            var toAim = playerInput.AimWorldPosition - (Vector2)transform.position;
            return toAim.sqrMagnitude > 0.0001f ? toAim.normalized : Vector2.up;
        }
    }
}
