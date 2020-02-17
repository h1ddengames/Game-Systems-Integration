// Original Version Created by IronWarrior:
// https://roystan.net/articles/character-controller-2d.html
// https://github.com/IronWarrior/2DCharacterControllerTutorial
// Updated/Current Version by h1ddengames

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

namespace h1ddengames {
    public class CharacterController2D : MonoBehaviour {
        #region Exposed Variables
        [BoxGroup("Main Setting"), Tooltip("Max speed, in units per second, that the character moves."), SerializeField]
        private float movementSpeed = 3.5f;

        [BoxGroup("Main Setting"), Tooltip("Acceleration while grounded."), SerializeField]
        private float walkAcceleration = 25;

        [BoxGroup("Main Setting"), Tooltip("Acceleration while in the air."), SerializeField]
        private float airAcceleration = 25;

        [BoxGroup("Main Setting"), Tooltip("Deceleration applied when character is grounded and not attempting to move."), SerializeField]
        private float groundDeceleration = 50;

        [BoxGroup("Main Setting"), Tooltip("Max height the character will jump regardless of gravity"), SerializeField]
        private float jumpHeight = 1.25f;

        [BoxGroup("Quick Information"), Tooltip("Only rejects player input. All physics based movement will continue to work as intended."), SerializeField] private bool isAllowedToMove = true;
        [BoxGroup("Quick Information"), Tooltip("Is set to true when the character intersects a collider beneath them in the previous frame."), SerializeField] private bool isGrounded;
        [BoxGroup("Quick Information"), SerializeField] private Vector2 velocity;

        [BoxGroup("Configuration"), Tooltip("This collider is required to contain cinemachine camera to a certain area."), SerializeField] private PolygonCollider2D boundingBox;
        [BoxGroup("Configuration"), SerializeField] private Animator animator;
        [BoxGroup("Configuration"), SerializeField] private BoxCollider2D boxCollider;
        [BoxGroup("Configuration"), Tooltip("This component must be located on the first child of this gameObject."), SerializeField] private SpriteRenderer spriteRenderer;

        [BoxGroup("Events"), InfoBox("This event will run on every frame that isGrounded is true", InfoBoxType.Normal), SerializeField] private UnityEvent isOnGroundEvent;
        [BoxGroup("Events"), InfoBox("Can accidentally be called if there's an overlap between two colliders", InfoBoxType.Warning), InfoBox("This event will run on every frame that isGrounded is false", InfoBoxType.Normal), SerializeField] private UnityEvent isNotOnGroundEvent;
        [BoxGroup("Events"), InfoBox("This event will run on every frame that moveInput is not 0", InfoBoxType.Normal), SerializeField] private UnityEvent isMovingEvent;
        #endregion

        #region Private Variables
        #endregion

        #region Getters/Setters/Constructors
        public bool IsAllowedToMove { get => isAllowedToMove; set => isAllowedToMove = value; }
        public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
        public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
        public float WalkAcceleration { get => walkAcceleration; set => walkAcceleration = value; }
        public float AirAcceleration { get => airAcceleration; set => airAcceleration = value; }
        public float GroundDeceleration { get => groundDeceleration; set => groundDeceleration = value; }
        public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
        public PolygonCollider2D BoundingBox { get => boundingBox; set => boundingBox = value; }
        public Animator Animator { get => animator; set => animator = value; }
        public BoxCollider2D BoxCollider { get => boxCollider; set => boxCollider = value; }
        public SpriteRenderer SpriteRenderer { get => spriteRenderer; set => spriteRenderer = value; }
        public UnityEvent IsOnGroundEvent { get => isOnGroundEvent; set => isOnGroundEvent = value; }
        public UnityEvent IsNotOnGroundEvent { get => isNotOnGroundEvent; set => isNotOnGroundEvent = value; }
        public UnityEvent IsMovingEvent { get => isMovingEvent; set => isMovingEvent = value; }
        #endregion

        #region Animation Methods
        public void AnimateRun(float moveInput) {
            Animator.SetFloat("Velocity", Mathf.Abs(moveInput));
        }

        public void AnimateJump() {
            Animator.SetBool("isJumping", true);
        }

        // Used by the animation tab as an event.
        // To add an event, right click on the top row of the animation tab
        // and click on "Add Animation Event". Then select this method in the dropdown.
        public void EndJump() {
            Animator.SetBool("isJumping", false);
        }
        #endregion

        #region Test Methods
        public void TestNotOnGroundEvent() {
            Debug.LogError("NOT ON THE GROUND");
        }

        public void TestOnGroundEvent() {
            Debug.LogWarning("ON THE GROUND");
        }

        public void TestOnMovingEvent() {
            Debug.Log("I'M WALKIN' HERE");
        }
        #endregion

        #region Movement Methods
        // Switch the direction of the sprite based on input.
        public void FlipCharacter(float moveInput) {
            if (moveInput < -0.01f) {
                SpriteRenderer.flipX = true;
            } else if (moveInput > 0.01f) {
                SpriteRenderer.flipX = false;
            }
        }

        public void Jump() {
            if (IsGrounded) {
                velocity.y = 0;

                if (IsAllowedToMove) {
                    // Triple check for the jump button since sometimes the script wont register the input.
                    if (Input.GetButtonDown("Jump") || Input.GetButtonUp("Jump") || Input.GetButton("Jump")) {
                        // Checking for GetButton vs GetButtonDown and GetButtonUp means that the player
                        // can hold down the Jump button and keep jumping at the same frame that
                        // isGrounded is set to true.
                        // Calculate the velocity required to achieve the target jump height.
                        velocity.y = Mathf.Sqrt(2 * JumpHeight * Mathf.Abs(Physics2D.gravity.y));
                        AnimateJump();
                        IsGrounded = false;
                    }
                }
            }
        }

        public void Move(float moveInput) {
            float acceleration = IsGrounded ? WalkAcceleration : AirAcceleration;
            float deceleration = IsGrounded ? GroundDeceleration : 0;

            if (moveInput != 0) {
                velocity.x = Mathf.MoveTowards(velocity.x, MovementSpeed * moveInput, acceleration * Time.deltaTime);
            } else {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
            }

            velocity.y += Physics2D.gravity.y * Time.deltaTime;

            transform.Translate(velocity * Time.deltaTime);
        }

        public void CheckForGround() {
            // Retrieve all colliders we have intersected after velocity has been applied.
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, BoxCollider.size * 1.01f, 0);

            foreach (Collider2D hit in hits) {
                // Ignore our own collider.
                if (hit == BoxCollider)
                    continue;

                // Ignore bounding box for the level.
                if (hit == BoundingBox)
                    continue;

                ColliderDistance2D colliderDistance = hit.Distance(BoxCollider);

                // Ensure that we are still overlapping this collider.
                // The overlap may no longer exist due to another intersected collider
                // pushing us out of this one.
                if (colliderDistance.isOverlapped) {
                    transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                    // If we intersect an object beneath us, set grounded to true. 
                    if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0) {
                        IsGrounded = true;
                    }
                }
            }
        }
        #endregion

        #region Skill Methods
        // TODO: Move the character with code

        // TODO: Apply Knockback

        // TODO: Apply Double/Triple Jump

        // TODO: Apply Dash

        // TODO: Apply Teleport
        #endregion

        #region My Methods
        // Creates a button on the inspector that runs this method that finds required components.
        [NaughtyAttributes.Button("Find Configuration Elements")]
        private void FindConfigurationElements() {
            Animator = GetComponent<Animator>();
            BoxCollider = GetComponent<BoxCollider2D>();
            SpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            FindConfigurationElements();
        }

        void FixedUpdate() {
            // TODO: Get input in Update then run physics related code in FixedUpdate.
            // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
            float moveInput = isAllowedToMove == true ? Input.GetAxisRaw("Horizontal") : 0;

            // The above line and the following if statement is the same.
            //if (IsAllowedToMove) {
            //    moveInput = Input.GetAxisRaw("Horizontal");
            //} else { moveInput = 0; }
            
            if (SpriteRenderer != null) {
                FlipCharacter(moveInput);
            }

            if (Animator != null) {
                AnimateRun(moveInput);
            }

            Jump();

            Move(moveInput);

            if (moveInput != 0) {
                IsMovingEvent.Invoke();
            }

            IsGrounded = false;

            if (BoxCollider != null) {
                CheckForGround();
            }

            if (IsGrounded) {
                IsOnGroundEvent.Invoke();
            } else {
                IsNotOnGroundEvent.Invoke();
            }
        }
        #endregion

        #region Helper Methods
        #endregion
    }
}
