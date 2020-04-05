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
        [BoxGroup("Configuration"), SerializeField] private int currentJumpCount = 2;
        [BoxGroup("Configuration"), SerializeField] private int maxJumpCount = 2;
        [BoxGroup("Configuration"), SerializeField] private float jumpDelay = 0.3f;

        [BoxGroup("Configuration"),
        Tooltip("The layers that this character can stand on."),
        SerializeField]
        private LayerMask groundLayer;

        [BoxGroup("Inputs"), SerializeField] private KeyCode moveLeftKey;
        [BoxGroup("Inputs"), SerializeField] private KeyCode moveRightKey;
        [BoxGroup("Inputs"), SerializeField] private KeyCode jumpKey;

        [BoxGroup("Reference"), SerializeField] private Rigidbody2D characterRigidbody2D;
        [BoxGroup("Reference"), SerializeField] private Transform characterGroundedChecker;
        [BoxGroup("Reference"), SerializeField] private BoxCollider2D characterGroundedCheckerBox;
        [BoxGroup("Reference"), SerializeField] private PlayerInputModule playerInputModule;
        [BoxGroup("Reference"), SerializeField] private AnimationModule characterAnimationModule;
        [BoxGroup("Reference"), SerializeField] private AutomatedMoveModule CharacterAutomatedMoveModule;
        [BoxGroup("Reference"), SerializeField] private SkillModule skillModule;
        #endregion

        #region Private Fields
        //private Vector2 characterMovementInput;
        //private Vector2 characterVelocity;
        //private bool isBeingControlledByCode = false;
        //private bool isAcceptingInput = true;
        //private bool isGrounded = true;
        //private bool isJumping = false;
        //private bool isFacingRight = false;
        //private float lastJumped = 0;
        //private bool hasChangedDirectionThisFrame = false;
        //private int debugColliderOverlapsCounter;
        //private Collider2D[] debugColliderOverlaps;

        public Vector2 CharacterMovementInput { get; set; }
        public Vector2 CharacterVelocity { get; set; }
        public bool IsBeingControlledByCode { get; set; }
        public bool IsAcceptingInput { get; set; }
        public bool IsGrounded { get; set; }
        public bool IsJumping { get; set; }
        public bool IsFacingRight { get; set; }
        public float LastJumped { get; set; }
        public bool HasChangedDirectionThisFrame { get; set; }
        public int DebugColliderOverlapsCounter { get; set; }
        public Collider2D[] DebugColliderOverlaps { get; set; }
        #endregion

        #region Getters/Setters/Constructors
        public float CharacterMoveSpeed { get => characterMoveSpeed; set => characterMoveSpeed = value; }
        public float CharacterMaxSpeed { get => characterMaxSpeed; set => characterMaxSpeed = value; }
        public float CharacterJumpHeight { get => characterJumpHeight; set => characterJumpHeight = value; }
        public int CurrentJumpCount { get => currentJumpCount; set => currentJumpCount = value; }
        public int MaxJumpCount { get => maxJumpCount; set => maxJumpCount = value; }
        public float JumpDelay { get => jumpDelay; set => jumpDelay = value; }
        public LayerMask GroundLayer { get => groundLayer; set => groundLayer = value; }

        public KeyCode MoveLeftKey { get => moveLeftKey; set => moveLeftKey = value; }
        public KeyCode MoveRightKey { get => moveRightKey; set => moveRightKey = value; }
        public KeyCode JumpKey { get => jumpKey; set => jumpKey = value; }

        public Rigidbody2D CharacterRigidbody2D { get => characterRigidbody2D; set => characterRigidbody2D = value; }
        public Transform CharacterGroundedChecker { get => characterGroundedChecker; set => characterGroundedChecker = value; }
        public BoxCollider2D CharacterGroundedCheckerBox { get => characterGroundedCheckerBox; set => characterGroundedCheckerBox = value; }
        public PlayerInputModule PlayerInputModule { get => playerInputModule; set => playerInputModule = value; }
        public AnimationModule CharacterAnimationModule { get => characterAnimationModule; set => characterAnimationModule = value; }
        public AutomatedMoveModule CharacterAutomatedMoveModule1 { get => CharacterAutomatedMoveModule; set => CharacterAutomatedMoveModule = value; }
        public SkillModule SkillModule { get => skillModule; set => skillModule = value; }
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
            //CharacterRigidbody2D.AddForce(new Vector2(0, CharacterJumpHeight), ForceMode2D.Impulse);
            CharacterRigidbody2D.velocity = new Vector2(CharacterRigidbody2D.velocity.x, Mathf.Sqrt(2 * CharacterJumpHeight * Mathf.Abs(Physics2D.gravity.y)));

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

        // TODO: Allow one-way Platforms that allow players to passthrough from below and stand on top.
        // TODO: Use the new Unity Input System

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
            CharacterAnimationModule = GetComponent<AnimationModule>();
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

            LastJumped += Time.deltaTime;
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
                if(IsGrounded || (CurrentJumpCount > 0 && LastJumped > JumpDelay)) {
                    Jump();
                    CurrentJumpCount--;
                    LastJumped = 0;
                }
            }

            CheckForGround();

            if(IsGrounded) {
                CurrentJumpCount = MaxJumpCount;
            }

            CharacterVelocity = CharacterRigidbody2D.velocity;
        }

        // Animation methods and other visual changes should be called here.
        void LateUpdate() {
            if(HasChangedDirectionThisFrame) {
                CharacterAnimationModule.AnimateCharacterFlip();
                HasChangedDirectionThisFrame = false;
            }

            CharacterAnimationModule.AnimateMove(Mathf.Abs(CharacterVelocity.x));

            if(CharacterMovementInput.y > 0 && !IsJumping) {
                CharacterAnimationModule.AnimateJump();
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