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
        [BoxGroup("Main Setting"), Tooltip("Max height the character will jump regardless of gravity."), SerializeField]
        private float jumpHeight = 1.25f;
        [BoxGroup("Main Setting"), Tooltip("The amount of time the player needs to wait until they can jump again."), SerializeField]
        private float jumpDelay = 0.25f;
        [BoxGroup("Main Setting"), Tooltip("The number of times the player can jump before having to touch the ground."), SerializeField] private int maxNumberOfJumps = 2;

        [BoxGroup("Quick Information"), Tooltip("Only rejects player input. All physics based movement will continue to work as intended."), SerializeField] private bool isAllowedToMove = true;
        [BoxGroup("Quick Information"), Tooltip("Player receives constant gravity as long as isGrounded is false."), SerializeField] private bool isGravityOn = true;
        [BoxGroup("Quick Information"), Tooltip("Is set to true when the character intersects a collider beneath them in the previous frame."), SerializeField] private bool isGrounded;
        [BoxGroup("Quick Information"), Tooltip("A count of how many times the player can jump without having to touch the ground."), SerializeField] private int currentNumberOfJumps;
        [BoxGroup("Quick Information"), Tooltip("The last time in milliseconds that the player has jumped."), SerializeField] private float m_lastJumped;
        [BoxGroup("Quick Information"), SerializeField] private Vector2 velocity;
        [BoxGroup("Quick Information"), SerializeField] private Vector2 defaultBoxColliderSize;

        [BoxGroup("Configuration"), Tooltip("This collider is required to contain cinemachine camera to a certain area."), SerializeField] private PolygonCollider2D levelBoundingBox;
        [BoxGroup("Configuration"), SerializeField] private Animator characterAnimator;
        [BoxGroup("Configuration"), SerializeField] private BoxCollider2D characterBoxCollider2D;
        [BoxGroup("Configuration"), Tooltip("This component must be located on the first child of this gameObject."), SerializeField] private SpriteRenderer characterSpriteRenderer;

        [BoxGroup("Events"), InfoBox("This event will run on every frame that isGrounded is true", InfoBoxType.Normal), SerializeField] private UnityEvent isOnGroundEvent;
        [BoxGroup("Events"), InfoBox("Can accidentally be called if there's an overlap between two colliders", InfoBoxType.Warning), InfoBox("This event will run on every frame that isGrounded is false", InfoBoxType.Normal), SerializeField] private UnityEvent isNotOnGroundEvent;
        [BoxGroup("Events"), InfoBox("This event will run on every frame that moveInput is not 0", InfoBoxType.Normal), SerializeField] private UnityEvent isMovingEvent;
        #endregion

        #region Private Variables
        #endregion

        #region Getters/Setters/Constructors
        public float MovementSpeed { get => movementSpeed; set => movementSpeed = value; }
        public float WalkAcceleration { get => walkAcceleration; set => walkAcceleration = value; }
        public float AirAcceleration { get => airAcceleration; set => airAcceleration = value; }
        public float GroundDeceleration { get => groundDeceleration; set => groundDeceleration = value; }
        public float JumpHeight { get => jumpHeight; set => jumpHeight = value; }
        public float JumpDelay { get => jumpDelay; set => jumpDelay = value; }
        public int MaxNumberOfJumps { get => maxNumberOfJumps; set => maxNumberOfJumps = value; }
        public bool IsAllowedToMove { get => isAllowedToMove; set => isAllowedToMove = value; }
        public bool IsGravityOn { get => isGravityOn; set => isGravityOn = value; }
        public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
        public int CurrentNumberOfJumps { get => currentNumberOfJumps; set => currentNumberOfJumps = value; }
        public float LastJumped { get => m_lastJumped; set => m_lastJumped = value; }
        public Vector2 DefaultBoxColliderSize { get => defaultBoxColliderSize; set => defaultBoxColliderSize = value; }
        public PolygonCollider2D LevelBoundingBox { get => levelBoundingBox; set => levelBoundingBox = value; }
        public Animator CharacterAnimator { get => characterAnimator; set => characterAnimator = value; }
        public BoxCollider2D CharacterBoxCollider2D { get => characterBoxCollider2D; set => characterBoxCollider2D = value; }
        public SpriteRenderer CharacterSpriteRenderer { get => characterSpriteRenderer; set => characterSpriteRenderer = value; }
        public UnityEvent IsOnGroundEvent { get => isOnGroundEvent; set => isOnGroundEvent = value; }
        public UnityEvent IsNotOnGroundEvent { get => isNotOnGroundEvent; set => isNotOnGroundEvent = value; }
        public UnityEvent IsMovingEvent { get => isMovingEvent; set => isMovingEvent = value; }
        #endregion

        #region Animation Methods
        public void AnimateRun(float moveInput) {
            CharacterAnimator.SetFloat("velocity", Mathf.Abs(moveInput));
        }

        public void AnimateJump() {
            CharacterAnimator.SetBool("isJumping", true);
        }

        // Used by the animation tab as an event.
        // To add an event, right click on the top row of the animation tab
        // and click on "Add Animation Event". Then select this method in the dropdown.
        public void EndJump() {
            CharacterAnimator.SetBool("isJumping", false);
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
                CharacterSpriteRenderer.flipX = true;
            } else if (moveInput > 0.01f) {
                CharacterSpriteRenderer.flipX = false;
            }
        }

        public void Crouch() {
            if (Input.GetKey(KeyCode.S)) {
                CharacterBoxCollider2D.size = new Vector2(CharacterBoxCollider2D.size.x, DefaultBoxColliderSize.y / 2);
            } else {
                CharacterBoxCollider2D.size = DefaultBoxColliderSize;
            }
        }

        // Only works in LateUpdate.
        public void Jump() {
            if (IsGrounded) {
                velocity.y = 0;
            }

            if (IsAllowedToMove && CurrentNumberOfJumps > 0) {
                if (LastJumped < JumpDelay) {
                    LastJumped += Time.deltaTime;
                } else if (Input.GetButtonDown("Jump")) {
                    // Checking for GetButton vs GetButtonDown and GetButtonUp means that the player
                    // can hold down the Jump button and keep jumping at the same frame that
                    // isGrounded is set to true.
                    // Calculate the velocity required to achieve the target jump height.
                    velocity.y = Mathf.Sqrt(2 * JumpHeight * Mathf.Abs(Physics2D.gravity.y));
                    AnimateJump();
                    IsGrounded = false;
                    CurrentNumberOfJumps--;
                    LastJumped = 0f;
                } else if(Input.GetButton("Jump") && IsGrounded) {
                    velocity.y = Mathf.Sqrt(2 * JumpHeight * Mathf.Abs(Physics2D.gravity.y));
                    AnimateJump();
                    IsGrounded = false;
                    CurrentNumberOfJumps--;
                    LastJumped = 0f;
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

            if(IsGravityOn) {
                velocity.y += Physics2D.gravity.y * Time.deltaTime;
            }

            transform.Translate(velocity * Time.deltaTime);
        }

        public void CheckForGround() {
            // Retrieve all colliders we have intersected after velocity has been applied.
            // When lining up the collider there is almost always a need to add an offset to perfectly
            // line the collider up with the sprite. Since the offset is almost always negative, the
            // offset is added rather than subtracted.
            Collider2D[] hits = Physics2D.OverlapBoxAll(new Vector3(transform.position.x + CharacterBoxCollider2D.offset.x, transform.position.y + CharacterBoxCollider2D.offset.y), CharacterBoxCollider2D.size, 0);

            foreach (Collider2D hit in hits) {
                // Ignore our own collider.
                if (hit == CharacterBoxCollider2D)
                    continue;

                // Ignore bounding box for the level.
                // Used to ignore a Bounding Shape 2D that is required by Cinemachine Confiner component.
                if (hit == LevelBoundingBox)
                    continue;

                // Ignore any collider that has isTrigger option checked.
                if(hit.isTrigger)
                    continue;

                ColliderDistance2D colliderDistance = hit.Distance(CharacterBoxCollider2D);

                // Ensure that we are still overlapping this collider.
                // The overlap may no longer exist due to another intersected collider
                // pushing us out of this one.
                // Divide the distance by 2 so that if there are multiple colliders under the sprite,
                // the sprite wont vibrate noticably on screen.
                if (colliderDistance.isOverlapped) {
                    transform.Translate((colliderDistance.pointA - colliderDistance.pointB)/2);

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

        // TODO: Allow one-way Platforms that allow players to passthrough from below and stand on top.

        // TODO: Apply Knockback

        // TODO: Apply Double/Triple/Quad Jump

        // TODO: Apply Dash

        // TODO: Apply Teleport
        #endregion

        #region My Methods
        // Creates a button on the inspector that runs this method that finds required components.
        [NaughtyAttributes.Button("Find Configuration Elements")]
        private void FindConfigurationElements() {
            CharacterAnimator = GetComponent<Animator>();
            CharacterBoxCollider2D = GetComponent<BoxCollider2D>();
            CharacterSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            CurrentNumberOfJumps = MaxNumberOfJumps;
            DefaultBoxColliderSize = CharacterBoxCollider2D.size;
        }
        #endregion

        #region Unity Methods
        private void Awake() {
            FindConfigurationElements();
        }

        private void LateUpdate() {
            Jump();
        }

        void FixedUpdate() {
            Crouch();

            // TODO: Get input in Update then run physics related code in FixedUpdate.
            // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
            float moveInput = IsAllowedToMove == true ? Input.GetAxisRaw("Horizontal") : 0;

            // The above line and the following if statement is the same.
            //if (IsAllowedToMove) {
            //    moveInput = Input.GetAxisRaw("Horizontal");
            //} else { moveInput = 0; }
            
            if (CharacterBoxCollider2D != null) {
                FlipCharacter(moveInput);
            }

            if (CharacterAnimator != null) {
                AnimateRun(moveInput);
            }

            Move(moveInput);

            if (moveInput != 0) {
                IsMovingEvent.Invoke();
            }

            IsGrounded = false;

            if (CharacterBoxCollider2D != null) {
                CheckForGround();
            }

            if (IsGrounded) {
                CurrentNumberOfJumps = MaxNumberOfJumps;
                IsOnGroundEvent.Invoke();
            } else {
                IsNotOnGroundEvent.Invoke();
            }
        }

        //private void OnDrawGizmos() {
        //    Gizmos.DrawCube(new Vector3(transform.position.x + CharacterBoxCollider2D.offset.x, transform.position.y + CharacterBoxCollider2D.offset.y, transform.position.z), CharacterBoxCollider2D.size);
        //}
        #endregion

        #region Helper Methods
        #endregion
    }
}
