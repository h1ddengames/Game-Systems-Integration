// Created by h1ddengames
// Attributes being used within this class require:
// https://github.com/dbrizov/NaughtyAttributes

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using ReorderableList = NaughtyAttributes.ReorderableListAttribute;

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
        [BoxGroup("Quick Information"), SerializeField] private bool isGrounded = true;
        [BoxGroup("Quick Information"), SerializeField] private bool isJumping = false;
        [BoxGroup("Quick Information"), SerializeField] private bool isFacingRight = false;
        [BoxGroup("Quick Information"), SerializeField] private bool hasChangedDirectionThisFrame = false;

        [BoxGroup("Reference"), SerializeField] private Rigidbody2D characterRigidbody2D;
        [BoxGroup("Reference"), SerializeField] private Transform characterGroundedChecker;
        [BoxGroup("Reference"), SerializeField] private BoxCollider2D characterGroundedCheckerBox;
        [BoxGroup("Reference"), SerializeField] private PlayerInputModule playerInputModule;
        [BoxGroup("Reference"), SerializeField] private AnimationModule animationModule;

        [BoxGroup("Debug"), SerializeField] private bool showDebugInformation;
        [BoxGroup("Debug"), ShowIf("showDebugInformation"), SerializeField] private int debugColliderOverlapsCounter;
        [BoxGroup("Debug"), ShowIf("showDebugInformation"), ReorderableList, SerializeField] private Collider2D[] debugColliderOverlaps;
        #endregion

        #region Private Fields
        private AutomatedMoveModule automatedMoveModule;
        #endregion

        #region Getters/Setters/Constructors

        #endregion

        #region My Methods
        public void CheckForGround() {
            bool lastFrameIsGrounded = isGrounded;
            bool currentFrameIsGrounded;

            debugColliderOverlaps = Physics2D.OverlapBoxAll(characterGroundedChecker.position, characterGroundedCheckerBox.size, 0, groundLayer);

            debugColliderOverlapsCounter = debugColliderOverlaps.Length;

            for(int i = 0; i < debugColliderOverlaps.Length; i++) {
                // Ignore any collider that is marked as a trigger.
                if(debugColliderOverlaps[i].isTrigger) {
                    debugColliderOverlapsCounter--;
                    continue;
                }

                // Ignore any collider that is on the player gameobject.
                if(debugColliderOverlaps[i].gameObject.tag == "Player") {
                    debugColliderOverlapsCounter--;
                    continue;
                }

                // Ingore the collider if it's not on the same layers defined by groundLayer.
                if(!((groundLayer.value & 1 << debugColliderOverlaps[i].gameObject.layer) == 1 << debugColliderOverlaps[i].gameObject.layer)) {
                    debugColliderOverlapsCounter--;
                    continue;
                }
            }

            currentFrameIsGrounded = debugColliderOverlapsCounter > 0;

            if(currentFrameIsGrounded) {
                isGrounded = true;
            }

            if(!currentFrameIsGrounded) {
                isGrounded = false;
            }

            // Just became ungrounded this frame.
            if(lastFrameIsGrounded && !currentFrameIsGrounded) {
                // Invoke not grounded event

            }

            // Just became grounded this frame.
            if(!lastFrameIsGrounded && currentFrameIsGrounded) {
                // Invoke landed event

                isJumping = false;
            }
        }

        public void Crouch() {
            
        }

        public void Jump() {
            characterRigidbody2D.AddForce(new Vector2(0, characterJumpHeight), ForceMode2D.Impulse);

            isJumping = true;

            // Invoke jump event

        }

        public void Move() {
            bool lastFrameWasFacingRight = isFacingRight;

            if(characterMovementInput.x > 0) {
                characterRigidbody2D.AddForce(new Vector2(characterMoveSpeed, 0), ForceMode2D.Impulse);
                isFacingRight = true;
            } else {
                characterRigidbody2D.AddForce(new Vector2(-characterMoveSpeed, 0), ForceMode2D.Impulse);
                isFacingRight = false;
            }

            bool currentFrameIsFacingRight = isFacingRight;

            if(!lastFrameWasFacingRight && currentFrameIsFacingRight || lastFrameWasFacingRight && !currentFrameIsFacingRight) {
                hasChangedDirectionThisFrame = true;
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
            characterMovementInput = new Vector2(input, characterMovementInput.y);
        }

        public void UpdateCharacterMovementInputY(float input) {
            // Getting vertical input.
            characterMovementInput = new Vector2(characterMovementInput.x, input);
        }
        #endregion

        #region Unity Methods
        private void OnEnable() {
            playerInputModule = GetComponent<PlayerInputModule>();
            animationModule = GetComponent<AnimationModule>();
        }

        void Awake() {
            automatedMoveModule = new AutomatedMoveModule();
        }

        // Input should be obtained here.
        void Update() {
            if(playerInputModule == null) {
                // Getting horizontal input.
                if(Input.GetKeyDown(moveLeftKey) || Input.GetKey(moveLeftKey)) {
                    UpdateCharacterMovementInputX(-1);
                } else if(Input.GetKeyDown(moveRightKey) || Input.GetKey(moveRightKey)) {
                    UpdateCharacterMovementInputX(1);
                }

                // Getting vertical input.
                if(Input.GetKeyDown(jumpKey) || Input.GetKey(jumpKey)) {
                    UpdateCharacterMovementInputY(1);
                }

                // Ending horizontal input.
                if(Input.GetKeyUp(moveLeftKey) || Input.GetKeyUp(moveRightKey)) {
                    UpdateCharacterMovementInputX(0);
                }

                // Ending vertical input.
                if(Input.GetKeyUp(jumpKey)) {
                    UpdateCharacterMovementInputY(0);
                }
            }
        }

        // Movement methods/ Physics related calculations should be called here.
        void FixedUpdate() {
            // Horizontal movement.
            if(!(characterMovementInput.x == 0)) {
                Move();

                // Clamping max move speed.
                characterRigidbody2D.velocity = new Vector2(Mathf.Clamp(characterRigidbody2D.velocity.x, -characterMaxSpeed, characterMaxSpeed), characterRigidbody2D.velocity.y);
            } else {
                // Stop the player's movement instantly when player input stops.
                if(isGrounded) {
                    characterRigidbody2D.velocity = new Vector2(0, characterRigidbody2D.velocity.y);
                }
            }

            // Vertical movement.
            if(characterMovementInput.y > 0) {
                if(isGrounded) {
                    Jump();
                }
            }

            CheckForGround();

            characterVelocity = characterRigidbody2D.velocity;
        }

        // Animation methods and other visual changes should be called here.
        void LateUpdate() {
            if(hasChangedDirectionThisFrame) {
                animationModule.AnimateCharacterFlip();
                hasChangedDirectionThisFrame = false;
            }

            animationModule.AnimateMove(Mathf.Abs(characterVelocity.x));

            if(characterMovementInput.y > 0 && !isJumping) {
                animationModule.AnimateJump();
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