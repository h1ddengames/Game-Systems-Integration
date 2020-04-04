// Created by h1ddengames
// Attributes being used within this class require:
// https://github.com/dbrizov/NaughtyAttributes

using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using ReorderableListAttribute = NaughtyAttributes.ReorderableListAttribute;

namespace h1ddengames {
    public class CharacterController2D : MonoBehaviour {
        #region Exposed Fields
        [BoxGroup("Configuration"), SerializeField] private float characterMoveSpeed = 1f;
        [BoxGroup("Configuration"), SerializeField] private float characterMaxSpeed = 5f;
        [BoxGroup("Configuration"), SerializeField] private float characterJumpHeight = 1f;

        [BoxGroup("Configuration"),
        Tooltip("The layers that this character can stand on."),
        SerializeField]
        private LayerMask groundLayer;

        [BoxGroup("Inputs"), SerializeField] private KeyCode moveLeftKey;
        [BoxGroup("Inputs"), SerializeField] private KeyCode moveRightKey;
        [BoxGroup("Inputs"), SerializeField] private KeyCode jumpKey;

        [BoxGroup("Quick Information"), SerializeField] private Vector2 characterMovementInput;
        [BoxGroup("Quick Information"), SerializeField] private Vector2 characterVelocity;
        [BoxGroup("Quick Information"), SerializeField] private bool isBeingControlledByCode = false;
        [BoxGroup("Quick Information"), SerializeField] private bool isAcceptingInput = true;
        [BoxGroup("Quick Information"), SerializeField] private bool isGrounded = true;
        [BoxGroup("Quick Information"), SerializeField] private bool isJumping = false;
        [BoxGroup("Quick Information"), SerializeField] private bool isFacingRight = false;
        [BoxGroup("Quick Information"), SerializeField] private bool hasChangedDirectionThisFrame = false;

        [BoxGroup("Reference"), SerializeField] private Rigidbody2D characterRigidbody2D;
        [BoxGroup("Reference"), SerializeField] private Transform characterGroundedChecker;
        [BoxGroup("Reference"), SerializeField] private BoxCollider2D characterGroundedCheckerBox;
        [BoxGroup("Reference"), SerializeField] private PlayerInputModule playerInputModule;
        [BoxGroup("Reference"), SerializeField] private AnimationModule animationModule;
        [BoxGroup("Reference"), SerializeField] private AutomatedMoveModule automatedMoveModule;

        [BoxGroup("Debug"), SerializeField] private bool showDebugInformation;
        [BoxGroup("Debug"), ShowIf("showDebugInformation"), SerializeField] private int debugColliderOverlapsCounter;
        [BoxGroup("Debug"), ShowIf("showDebugInformation"), ReorderableList, SerializeField] private Collider2D[] debugColliderOverlaps;
        #endregion

        #region Private Fields
        public Vector2 desiredVelocity;
        #endregion

        #region Getters/Setters/Constructors
        public float CharacterMoveSpeed { get => characterMoveSpeed; set => characterMoveSpeed = value; }
        public float CharacterMaxSpeed { get => characterMaxSpeed; set => characterMaxSpeed = value; }
        public float CharacterJumpHeight { get => characterJumpHeight; set => characterJumpHeight = value; }
        public LayerMask GroundLayer { get => groundLayer; set => groundLayer = value; }
        public KeyCode MoveLeftKey { get => moveLeftKey; set => moveLeftKey = value; }
        public KeyCode MoveRightKey { get => moveRightKey; set => moveRightKey = value; }
        public KeyCode JumpKey { get => jumpKey; set => jumpKey = value; }
        public Vector2 CharacterMovementInput { get => characterMovementInput; set => characterMovementInput = value; }
        public Vector2 CharacterVelocity { get => characterVelocity; set => characterVelocity = value; }
        public bool IsBeingControlledByCode { get => isBeingControlledByCode; set => isBeingControlledByCode = value; }
        public bool IsAcceptingInput { get => isAcceptingInput; set => isAcceptingInput = value; }
        public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
        public bool IsJumping { get => isJumping; set => isJumping = value; }
        public bool IsFacingRight { get => isFacingRight; set => isFacingRight = value; }
        public bool HasChangedDirectionThisFrame { get => hasChangedDirectionThisFrame; set => hasChangedDirectionThisFrame = value; }
        public Rigidbody2D CharacterRigidbody2D { get => characterRigidbody2D; set => characterRigidbody2D = value; }
        public Transform CharacterGroundedChecker { get => characterGroundedChecker; set => characterGroundedChecker = value; }
        public BoxCollider2D CharacterGroundedCheckerBox { get => characterGroundedCheckerBox; set => characterGroundedCheckerBox = value; }
        public PlayerInputModule PlayerInputModule { get => playerInputModule; set => playerInputModule = value; }
        public AnimationModule AnimationModule { get => animationModule; set => animationModule = value; }
        public AutomatedMoveModule AutomatedMoveModule { get => automatedMoveModule; set => automatedMoveModule = value; }
        public int DebugColliderOverlapsCounter { get => debugColliderOverlapsCounter; set => debugColliderOverlapsCounter = value; }
        public Collider2D[] DebugColliderOverlaps { get => debugColliderOverlaps; set => debugColliderOverlaps = value; }
        #endregion

        #region My Methods
        public void CheckForGround() {
            bool lastFrameIsGrounded = IsGrounded;
            bool currentFrameIsGrounded;

            DebugColliderOverlaps = Physics2D.OverlapBoxAll(CharacterGroundedChecker.position, CharacterGroundedCheckerBox.size, 0, GroundLayer);

            DebugColliderOverlapsCounter = DebugColliderOverlaps.Length;

            for(int i = 0; i < DebugColliderOverlaps.Length; i++) {
                // Ignore any collider that is marked as a trigger.
                if(DebugColliderOverlaps[i].isTrigger) {
                    DebugColliderOverlapsCounter--;
                    continue;
                }

                // Ignore any collider that is on the player gameobject.
                if(DebugColliderOverlaps[i].gameObject.tag == "Player") {
                    DebugColliderOverlapsCounter--;
                    continue;
                }

                // Ingore the collider if it's not on the same layers defined by groundLayer.
                if(!((GroundLayer.value & 1 << DebugColliderOverlaps[i].gameObject.layer) == 1 << DebugColliderOverlaps[i].gameObject.layer)) {
                    DebugColliderOverlapsCounter--;
                    continue;
                }
            }

            currentFrameIsGrounded = DebugColliderOverlapsCounter > 0;

            if(currentFrameIsGrounded) {
                IsGrounded = true;
            }

            if(!currentFrameIsGrounded) {
                IsGrounded = false;
            }

            // Just became ungrounded this frame.
            if(lastFrameIsGrounded && !currentFrameIsGrounded) {
                // Invoke not grounded event

            }

            // Just became grounded this frame.
            if(!lastFrameIsGrounded && currentFrameIsGrounded) {
                // Invoke landed event

                IsJumping = false;
            }
        }

        public void Crouch() {
            
        }

        public void Jump() {
            CharacterRigidbody2D.AddForce(new Vector2(0, CharacterJumpHeight), ForceMode2D.Impulse);

            IsJumping = true;

            // Invoke jump event

        }

        public void Move() {
            bool lastFrameWasFacingRight = IsFacingRight;

            if(CharacterMovementInput.x > 0) {
                CharacterRigidbody2D.AddForce(new Vector2(CharacterMoveSpeed, 0), ForceMode2D.Impulse);
                IsFacingRight = true;
            } else {
                CharacterRigidbody2D.AddForce(new Vector2(-CharacterMoveSpeed, 0), ForceMode2D.Impulse);
                IsFacingRight = false;
            }

            bool currentFrameIsFacingRight = IsFacingRight;

            if(!lastFrameWasFacingRight && currentFrameIsFacingRight || lastFrameWasFacingRight && !currentFrameIsFacingRight) {
                HasChangedDirectionThisFrame = true;
            }
        }

        // TODO: Apply Knockback
        public void ApplyKnockback() {
            Debug.Log("Applying knockback");
        }

        // TODO: Add Dodge Roll Ability
        public void DodgeRoll() {
            Debug.Log("Using Dodge Roll Skill");
        }

        // TODO: Add Dash Ability
        public void Dash() {
            Debug.Log("Using Dash Skill");
        }

        // TODO: Apply Teleport Ability
        public void Teleport() {
            Debug.Log("Using Teleport Skill");
        }

        // TODO: Apply Jetpack Ability
        public void Jetpack() {
            Debug.Log("Using Jetpack Skill");
        }

        // TODO: Allow one-way Platforms that allow players to passthrough from below and stand on top.
        // TODO: Use the new Unity Input System
        // TODO: Apply Double/Triple/Quad Jump Ability

        public void UpdateCharacterMovementInputX(float input) {
            // Getting horizontal input.
            CharacterMovementInput = new Vector2(input, CharacterMovementInput.y);
        }

        public void UpdateCharacterMovementInputY(float input) {
            // Getting vertical input.
            CharacterMovementInput = new Vector2(CharacterMovementInput.x, input);
        }
        #endregion

        #region Unity Methods
        private void OnEnable() {
            PlayerInputModule = GetComponent<PlayerInputModule>();
            AnimationModule = GetComponent<AnimationModule>();
        }

        void Awake() {
            //automatedMoveModule = new AutomatedMoveModule();
        }

        // Input should be obtained here.
        void Update() {
            if(!IsAcceptingInput) {
                return;
            }

            if(PlayerInputModule != null && !PlayerInputModule.IsAcceptingInput) {
                return;
            }

            if(PlayerInputModule == null) {
                // Getting horizontal input.
                if(Input.GetKeyDown(MoveLeftKey) || Input.GetKey(MoveLeftKey)) {
                    UpdateCharacterMovementInputX(-1);
                } else if(Input.GetKeyDown(MoveRightKey) || Input.GetKey(MoveRightKey)) {
                    UpdateCharacterMovementInputX(1);
                }

                // Getting vertical input.
                if(Input.GetKeyDown(JumpKey) || Input.GetKey(JumpKey)) {
                    UpdateCharacterMovementInputY(1);
                }

                // Ending horizontal input.
                if(Input.GetKeyUp(MoveLeftKey) || Input.GetKeyUp(MoveRightKey)) {
                    UpdateCharacterMovementInputX(0);
                }

                // Ending vertical input.
                if(Input.GetKeyUp(JumpKey)) {
                    UpdateCharacterMovementInputY(0);
                }
            }
        }

        // Movement methods/ Physics related calculations should be called here.
        void FixedUpdate() {
            // Horizontal movement.
            if(!(CharacterMovementInput.x == 0)) {
                Move();

                // Clamping max move speed.
                CharacterRigidbody2D.velocity = new Vector2(Mathf.Clamp(CharacterRigidbody2D.velocity.x, -CharacterMaxSpeed, CharacterMaxSpeed), CharacterRigidbody2D.velocity.y);
            } else {
                // Stop the player's movement instantly when player input stops.
                if(IsGrounded) {
                    CharacterRigidbody2D.velocity = new Vector2(0, CharacterRigidbody2D.velocity.y);
                }
            }

            // Vertical movement.
            if(CharacterMovementInput.y > 0) {
                if(IsGrounded) {
                    Jump();
                }
            }

            CheckForGround();

            CharacterVelocity = CharacterRigidbody2D.velocity;
        }

        // Animation methods and other visual changes should be called here.
        void LateUpdate() {
            if(HasChangedDirectionThisFrame) {
                AnimationModule.AnimateCharacterFlip();
                HasChangedDirectionThisFrame = false;
            }

            AnimationModule.AnimateMove(Mathf.Abs(CharacterVelocity.x));

            if(CharacterMovementInput.y > 0 && !IsJumping) {
                AnimationModule.AnimateJump();
            }
        }

        void OnDisable() {

        }
        #endregion

        #region Helper Methods
        [NaughtyAttributes.Button("Find References")]
        public void FindReferences() {
            
        }
        #endregion
    }
}