// Created by h1ddengames
// Attributes being used within this class require:
// https://github.com/dbrizov/NaughtyAttributes

using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;

namespace h1ddengames {
    public class CharacterController2D : MonoBehaviour {
        #region Exposed Fields

        [BoxGroup("Configuration"), SerializeField] private float characterMoveSpeed = 1f;
        [BoxGroup("Configuration"), SerializeField] private float characterMaxSpeed = 5f;
        [BoxGroup("Configuration"), SerializeField] private float characterJumpHeight = 1f;

        [BoxGroup("Inputs"), SerializeField] private KeyCode moveLeftKey;
        [BoxGroup("Inputs"), SerializeField] private KeyCode moveRightKey;
        [BoxGroup("Inputs"), SerializeField] private KeyCode jumpKey;

        [BoxGroup("Quick Information"), SerializeField] private Vector2 characterMovementInput;
        [BoxGroup("Quick Information"), SerializeField] private Vector2 characterVelocity;

        [BoxGroup("Reference"), SerializeField] private Rigidbody2D characterRigidbody2D;
        [BoxGroup("Reference"), SerializeField] private PlayerInputModule playerInputModule;
        #endregion

        #region Private Fields
        private AutomatedMoveModule automatedMoveModule;
        private AnimationModule animationModule;
        #endregion

        #region Getters/Setters/Constructors

        #endregion

        #region My Methods
        public void CheckForGround() {
            
        }

        public void Crouch() {
            
        }

        public void Jump() {
            characterRigidbody2D.AddForce(new Vector2(0, characterJumpHeight), ForceMode2D.Impulse);
        }

        public void Move() {
            if(characterMovementInput.x > 0) {
                characterRigidbody2D.AddForce(new Vector2(characterMoveSpeed, 0), ForceMode2D.Impulse);
            } else {
                characterRigidbody2D.AddForce(new Vector2(-characterMoveSpeed, 0), ForceMode2D.Impulse);
            }
        }

        // TODO: Apply Knockback
        public void ApplyKnockback() {

        }

        // TODO: Allow one-way Platforms that allow players to passthrough from below and stand on top.
        // TODO: Use the new Unity Input System
        // TODO: Add Dodge Roll Ability
        // TODO: Add Dash Ability
        // TODO: Apply Double/Triple/Quad Jump Ability
        // TODO: Apply Teleport Ability

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
        }

        void Awake() {
            automatedMoveModule = new AutomatedMoveModule();
            animationModule = new AnimationModule();
        }

        // Input should be obtained here.
        void Update() {
            if(playerInputModule == null) {
                // Getting horizontal input.
                if(Input.GetKeyDown(moveLeftKey) || Input.GetKey(moveLeftKey)) {
                    characterMovementInput = new Vector2(-1, characterMovementInput.y);
                } else if(Input.GetKeyDown(moveRightKey) || Input.GetKey(moveRightKey)) {
                    characterMovementInput = new Vector2(1, characterMovementInput.y);
                }

                // Getting vertical input.
                if(Input.GetKeyDown(jumpKey) || Input.GetKey(jumpKey)) {
                    characterMovementInput = new Vector2(characterMovementInput.x, 1);
                }

                // Ending horizontal input.
                if(Input.GetKeyUp(moveLeftKey) || Input.GetKeyUp(moveRightKey)) {
                    characterMovementInput = new Vector2(0, characterMovementInput.y);
                }

                // Ending vertical input.
                if(Input.GetKeyUp(jumpKey)) {
                    characterMovementInput = new Vector2(characterMovementInput.x, 0);
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
                characterRigidbody2D.velocity = new Vector2(0, characterRigidbody2D.velocity.y);
            }

            // Vertical movement.
            if(characterMovementInput.y > 0) {
                Jump();
            }


            characterVelocity = characterRigidbody2D.velocity;
        }

        // Animation methods and other visual changes should be called here.
        void LateUpdate() {
            if(!(characterMovementInput.x == 0)) {
                // Do move animation
            } else {
                // Do idle animation
            }

            if(characterMovementInput.y > 0) {
                // Do jump animation
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