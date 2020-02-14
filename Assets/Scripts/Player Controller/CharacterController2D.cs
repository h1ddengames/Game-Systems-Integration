// Created by IronWarrior:
// https://roystan.net/articles/character-controller-2d.html
// https://github.com/IronWarrior/2DCharacterControllerTutorial

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace h1ddengames {
    public class CharacterController2D : MonoBehaviour {
        #region Exposed Variables
        [SerializeField] private bool isAllowedToMove = true;

        [SerializeField, Tooltip("Max speed, in units per second, that the character moves.")]
        float speed = 3.5f;

        [SerializeField, Tooltip("Acceleration while grounded.")]
        float walkAcceleration = 25;

        [SerializeField, Tooltip("Acceleration while in the air.")]
        float airAcceleration = 25;

        [SerializeField, Tooltip("Deceleration applied when character is grounded and not attempting to move.")]
        float groundDeceleration = 50;

        [SerializeField, Tooltip("Max height the character will jump regardless of gravity")]
        float jumpHeight = 1.25f;

        [SerializeField] private PolygonCollider2D boundingBox;
        #endregion

        #region Private Variables
        private Animator animator;
        private BoxCollider2D boxCollider;
        private SpriteRenderer spriteRenderer;
        private Vector2 velocity;

        /// <summary>
        /// Set to true when the character intersects a collider beneath
        /// them in the previous frame.
        /// </summary>
        private bool grounded;
        #endregion

        #region Getters/Setters/Constructors
        public bool IsAllowedToMove { get => isAllowedToMove; set => isAllowedToMove = value; }
        public float Speed { get => speed; set => speed = value; }
        public float WalkAcceleration { get => walkAcceleration; set => walkAcceleration = value; }
        public float AirAcceleration { get => airAcceleration; set => airAcceleration = value; }
        public float GroundDeceleration { get => groundDeceleration; set => groundDeceleration = value; }
        public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
        #endregion

        #region Unity Methods
        private void Awake() {
            animator = GetComponent<Animator>();
            boxCollider = GetComponent<BoxCollider2D>();
            spriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        }

        void Start() {

        }

        void FixedUpdate() {
            if(!IsAllowedToMove) {
                return;
            }

            // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
            float moveInput = Input.GetAxisRaw("Horizontal");

            // Switch the direction of the sprite based on input.
            if(moveInput < -0.01f) {
                spriteRenderer.flipX = true;
            } else if(moveInput > 0.01f) {
                spriteRenderer.flipX = false;
            }

            if(moveInput == 0.00f) {
                animator.SetBool("isRunning", false);
            } else {
                animator.SetBool("isRunning", true);
            }

            if (grounded) {
                velocity.y = 0;

                if (Input.GetButtonDown("Jump")) {
                    // Calculate the velocity required to achieve the target jump height.
                    velocity.y = Mathf.Sqrt(2 * JumpHeight * Mathf.Abs(Physics2D.gravity.y));
                }
            }

            float acceleration = grounded ? WalkAcceleration : AirAcceleration;
            float deceleration = grounded ? GroundDeceleration : 0;

            if (moveInput != 0) {
                velocity.x = Mathf.MoveTowards(velocity.x, Speed * moveInput, acceleration * Time.deltaTime);
            } else {
                velocity.x = Mathf.MoveTowards(velocity.x, 0, deceleration * Time.deltaTime);
            }

            velocity.y += Physics2D.gravity.y * Time.deltaTime;

            transform.Translate(velocity * Time.deltaTime);

            grounded = false;

            // Retrieve all colliders we have intersected after velocity has been applied.
            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position, boxCollider.size * 1.2f, 0);

            foreach (Collider2D hit in hits) {
                // Ignore our own collider.
                if (hit == boxCollider)
                    continue;

                // Ignore bounding box for the level.
                if (hit == boundingBox)
                    continue;

                ColliderDistance2D colliderDistance = hit.Distance(boxCollider);

                // Ensure that we are still overlapping this collider.
                // The overlap may no longer exist due to another intersected collider
                // pushing us out of this one.
                if (colliderDistance.isOverlapped) {
                    transform.Translate(colliderDistance.pointA - colliderDistance.pointB);

                    // If we intersect an object beneath us, set grounded to true. 
                    if (Vector2.Angle(colliderDistance.normal, Vector2.up) < 90 && velocity.y < 0) {
                        grounded = true;
                    }
                }
            }
        }

        void OnDisable() {
            
        }
        #endregion

        #region Helper Methods
        #endregion
    }
}
