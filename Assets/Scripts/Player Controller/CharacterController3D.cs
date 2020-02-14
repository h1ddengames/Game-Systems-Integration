using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class CharacterController3D : MonoBehaviour {
    [BoxGroup("Movement"), Tooltip("Should the player be allowed to move?"), SerializeField]
    private bool allowPlayerMovement = true;
    [BoxGroup("Movement"), Tooltip("How fast should the player walk?"), SerializeField]
    private float walkSpeed = 3f;
    [BoxGroup("Movement"), Tooltip("How fast should the player run?"), SerializeField]
    private float runSpeed = 6f;
    [BoxGroup("Movement"), Tooltip("How high should the player jump?"), SerializeField]
    private float jumpHeight = 1.5f;
    [BoxGroup("Movement"), Tooltip("How much control does the player have in the air?"), SerializeField, Range(0, 1)]
    private float airControlPercent;
    [BoxGroup("Movement"), Tooltip("How fast is gravity?"), SerializeField] private float gravity = -12f;
    [BoxGroup("FOV"), Tooltip("Should the FOV change when player is moving fast?"), SerializeField]
    private bool changeFOV = true;
    [BoxGroup("FOV"), Tooltip("The FOV when the player is standing still or moving slowly"), SerializeField]
    private float defaultFOV = 60;
    [BoxGroup("FOV"), Tooltip("The FOV when the player is moving fast"), SerializeField]
    private float runFOV = 70;
    [BoxGroup("Smoothing"), SerializeField, Range(0, 1)] private float _smoothFOV = 0.6f;
    [BoxGroup("Smoothing"), SerializeField] private float turnSmoothTime = 0.2f;
    [BoxGroup("Smoothing"), SerializeField] private float speedSmoothTime = 0.1f;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode runKey = KeyCode.LeftShift;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode jumpKey = KeyCode.Space;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode crouchKey = KeyCode.LeftControl;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode dieKey = KeyCode.T;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode hurtKey = KeyCode.G;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode unlockCursorKey = KeyCode.LeftControl;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode openSettingsMenu = KeyCode.Escape;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode openItemMenu = KeyCode.I;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode openEquipmentMenu = KeyCode.E;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode openSkillsMenu = KeyCode.K;
    [BoxGroup("Player Inputs"), SerializeField] private KeyCode openStatsMenu = KeyCode.L;
    
    [SerializeField] private GameObject _currentModel;

    float _turnSmoothVelocity;
    float _speedSmoothVelocity;
    float _currentSpeed;
    float _velocityY;

    Animator _animator;
    CharacterController _controller;
    ThirdPersonCameraMovement _thirdPersonCameraMovement;
    Transform _cameraT;
    Camera _camera;
    

    public bool AllowPlayerMovement { get => allowPlayerMovement; set => allowPlayerMovement = value; }
    public float WalkSpeed { get => walkSpeed; set => walkSpeed = value; }
    public float RunSpeed { get => runSpeed; set => runSpeed = value; }
    public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
    public float AirControlPercent { get => airControlPercent; set => airControlPercent = value; }
    public float Gravity { get => gravity; set => gravity = value; }
    public bool ChangeFOV { get => changeFOV; set => changeFOV = value; }
    public float DefaultFOV { get => defaultFOV; set => defaultFOV = value; }
    public float RunFOV { get => runFOV; set => runFOV = value; }
    public float SmoothFOV { get => _smoothFOV; set => _smoothFOV = value; }
    public float TurnSmoothTime { get => turnSmoothTime; set => turnSmoothTime = value; }
    public float SpeedSmoothTime { get => speedSmoothTime; set => speedSmoothTime = value; }
    public KeyCode RunKey { get => runKey; set => runKey = value; }
    public KeyCode JumpKey { get => jumpKey; set => jumpKey = value; }
    public KeyCode CrouchKey { get => crouchKey; set => crouchKey = value; }
    public KeyCode DieKey { get => dieKey; set => dieKey = value; }
    public KeyCode HurtKey { get => hurtKey; set => hurtKey = value; }
    public KeyCode UnlockCursorKey { get => unlockCursorKey; set => unlockCursorKey = value; }
    public KeyCode OpenSettingsMenu { get => openSettingsMenu; set => openSettingsMenu = value; }
    public KeyCode OpenItemMenu { get => openItemMenu; set => openItemMenu = value; }
    public KeyCode OpenEquipmentMenu { get => openEquipmentMenu; set => openEquipmentMenu = value; }
    public KeyCode OpenSkillsMenu { get => openSkillsMenu; set => openSkillsMenu = value; }
    public KeyCode OpenStatsMenu { get => openStatsMenu; set => openStatsMenu = value; }

    private void Start() {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _cameraT = Camera.main.transform;
        _controller = GetComponent<CharacterController>();
        _thirdPersonCameraMovement = _camera.gameObject.GetComponent<ThirdPersonCameraMovement>();
    }

    private void Update() {
        if(Input.GetKeyDown(UnlockCursorKey)) {
            // Toggle lock cursor.
            _thirdPersonCameraMovement.LockCursor = !_thirdPersonCameraMovement.LockCursor;
        }

        if (Input.GetKeyDown(OpenSettingsMenu)) {
            Debug.Log("Settings Menu button pressed.");
            _thirdPersonCameraMovement.SettingsMenu.SetActive(!_thirdPersonCameraMovement.SettingsMenu.activeSelf);
            _thirdPersonCameraMovement.LockCursor = !_thirdPersonCameraMovement.LockCursor;
            _thirdPersonCameraMovement.AllowCameraMovement = !_thirdPersonCameraMovement.AllowCameraMovement;
            AllowPlayerMovement = !AllowPlayerMovement;
        }
        //} else if(Input.GetKeyDown(openItemMenu)) {
        //    Debug.Log("Item Menu button pressed.");
        //} else if(Input.GetKeyDown(openEquipmentMenu)) {
        //    Debug.Log("Equipment Menu button pressed.");
        //} else if(Input.GetKeyDown(openSkillsMenu)) {
        //    Debug.Log("Skills Menu button pressed.");
        //} else if(Input.GetKeyDown(openStatsMenu)) {
        //    Debug.Log("Stats Menu button pressed.");
        //}

        if (AllowPlayerMovement) {
            // Input from player.
            Vector2 input = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
            Vector2 inputDir = input.normalized;
            bool running = Input.GetKey(RunKey) && !Input.GetKey(CrouchKey);

            if (!_animator.GetBool("isDead") && !_animator.GetBool("isGettingUp")) {
                // Move the player left/right front/back if the player is alive.
                Move(inputDir, running);
            }

            // Move the player up and down.
            if (_controller.isGrounded) {
                _animator.SetBool("jumping", false);

                if (Input.GetKeyDown(JumpKey)) {
                    Jump();
                }
            }

            if (Input.GetKey(DieKey)) {
                Die();
            }

            if (Input.GetKey(HurtKey)) {
                TakeDamage();
            }

            // Animate the player by using a blend tree that changes animation based on animationSpeedPercent.
            float animationSpeedPercent = (running) ? _currentSpeed / RunSpeed : _currentSpeed / WalkSpeed * 0.5f;
            _animator.SetFloat("speedPercent", animationSpeedPercent, SpeedSmoothTime, Time.deltaTime);

            // Change the FOV based on speed.
            if (ChangeFOV && animationSpeedPercent > 0.7f) {
                _camera.fieldOfView += RunFOV * SmoothFOV * Time.deltaTime;
                _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, DefaultFOV, RunFOV);
            } else if(ChangeFOV) {
                _camera.fieldOfView -= DefaultFOV * SmoothFOV * Time.deltaTime;
                _camera.fieldOfView = Mathf.Clamp(_camera.fieldOfView, DefaultFOV, RunFOV);
            }
        }
    }

    void Move(Vector2 inputDir, bool running) {
        if (inputDir != Vector2.zero) {
            float targetRotation = Mathf.Atan2(inputDir.x, inputDir.y) * Mathf.Rad2Deg + _cameraT.eulerAngles.y;
            transform.eulerAngles = Vector3.up * Mathf.SmoothDampAngle(transform.eulerAngles.y, targetRotation, ref _turnSmoothVelocity, GetModifiedSmoothTime(TurnSmoothTime));
        }

        float targetSpeed = ((running) ? RunSpeed : WalkSpeed) * inputDir.magnitude;
        _currentSpeed = Mathf.SmoothDamp(_currentSpeed, targetSpeed, ref _speedSmoothVelocity, GetModifiedSmoothTime(SpeedSmoothTime));

        _velocityY += Time.deltaTime * Gravity;
        Vector3 velocity = transform.forward * _currentSpeed + Vector3.up * _velocityY;

        _controller.Move(velocity * Time.deltaTime);
        _currentSpeed = new Vector2(_controller.velocity.x, _controller.velocity.z).magnitude;

        if (_controller.isGrounded) {
            _velocityY = 0;
        }
    }

    void Jump() {
        _animator.SetBool("jumping", true);
        float jumpVelocity = Mathf.Sqrt(-2 * Gravity * JumpHeight);
        _velocityY = jumpVelocity;
    }

    private void StandUp() {
        _animator.SetBool("isGettingUp", !_animator.GetBool("isGettingUp"));
    }

    private void TakeDamage() {
        _animator.SetBool("isHurt", !_animator.GetBool("isHurt"));
    }

    private void Die() {
        bool isDead = !_animator.GetBool("isDead");
        if(!isDead) {
            StandUp();
        }
        _animator.SetBool("isDead", isDead);
    }

    public void SwitchAvatar(Avatar avatar) {
        // Set the avatar on the Animation component
        _animator.avatar = avatar;
    }

    public void SwitchModel(GameObject model) {
        // Turn off current model.
        _currentModel.SetActive(false);

        // Turn on wanted model.
        _currentModel = model;
        _currentModel.SetActive(true);
    }

    public void QuitGame() {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
      Application.Quit();
#endif
    }

    #region Helper Methods
    float GetModifiedSmoothTime(float smoothTime) {
        if(_controller.isGrounded) {
            return smoothTime;
        }

        if(AirControlPercent == 0) {
            return float.MaxValue;
        }

        return smoothTime / AirControlPercent;
    }
    #endregion
}
