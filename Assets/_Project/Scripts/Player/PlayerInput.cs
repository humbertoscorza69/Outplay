using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Outplay.Player
{
    /// <summary>
    /// Sole owner of Input System wiring for the player.
    /// Other player scripts read movement/aim from here and subscribe to dash events.
    /// </summary>
    [DisallowMultipleComponent]
    public class PlayerInput : MonoBehaviour
    {
        [Header("Input Tuning")]
        [SerializeField] private float moveDeadzone = 0.1f;

        private PlayerControls controls;
        private Camera mainCamera;
        private Vector2 cachedAim;

        public Vector2 MoveInput { get; private set; }
        public Vector2 AimWorldPosition => cachedAim;
        public event Action OnDashPressed;

        private void Awake()
        {
            controls = new PlayerControls();
            mainCamera = Camera.main;
        }

        private void OnEnable()
        {
            controls.Player.Enable();
            controls.Player.Dash.performed += HandleDashPerformed;
        }

        private void OnDisable()
        {
            controls.Player.Dash.performed -= HandleDashPerformed;
            controls.Player.Disable();
        }

        private void OnDestroy()
        {
            controls?.Dispose();
        }

        private void Update()
        {
            var raw = controls.Player.Move.ReadValue<Vector2>();
            MoveInput = raw.sqrMagnitude < moveDeadzone * moveDeadzone ? Vector2.zero : raw;

            if (mainCamera != null)
            {
                var screen = controls.Player.Aim.ReadValue<Vector2>();
                var world = mainCamera.ScreenToWorldPoint(new Vector3(screen.x, screen.y, mainCamera.nearClipPlane));
                cachedAim = new Vector2(world.x, world.y);
            }
        }

        private void HandleDashPerformed(InputAction.CallbackContext ctx)
        {
            OnDashPressed?.Invoke();
        }
    }
}
