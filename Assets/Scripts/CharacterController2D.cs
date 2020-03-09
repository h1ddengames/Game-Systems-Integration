// Created by h1ddengames
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

namespace h1ddengames {
    public class CharacterController2D : MonoBehaviour {
        #region Exposed Fields
        #region Configuration
        [BoxGroup("Configuration"),
        SerializeField]
        private bool showConfiguration = false;

        [BoxGroup("Configuration"),
        Tooltip("How fast should the player be able to move left and right?"),
        ShowIf("showConfiguration"),
        SerializeField]
        private float characterSpeed = 5.0f;

        [BoxGroup("Configuration"),
        Tooltip("How fast should the player be able to move left and right?"),
        ShowIf("showConfiguration"),
        SerializeField]
        private float characterJumpHeight = 1.0f;

        [BoxGroup("Configuration"), 
        Tooltip("The amount of time the player needs to wait until they can jump again."),
        ShowIf("showConfiguration"),
        SerializeField]
        private float jumpDelay = 0.25f;

        [BoxGroup("Configuration"), 
        Tooltip("How many times should the player be able to jump before landing on the ground?"),
        ShowIf("showConfiguration"),
        SerializeField] 
        private int currentConsecutiveJumps = 4;

        [BoxGroup("Configuration"), 
        Tooltip("How many times should the player be able to jump before landing on the ground?"),
        ShowIf("showConfiguration"),
        SerializeField] 
        private int maxConsecutiveJumps = 4;

        [BoxGroup("Configuration"),
        Tooltip("What layers should the player collide with?"),
        ShowIf("showConfiguration"),
        SerializeField]
        private LayerMask groundLayer;

        [BoxGroup("Configuration"),
        Tooltip("Should the script accept player input?"),
        ShowIf("showConfiguration"),
        SerializeField]
        private bool characterUsesSpriteRenderer = false;
        #endregion


        #region Quick Information
        [BoxGroup("Quick Information"), 
        SerializeField] 
        private bool showQuickInformation = false;

        [BoxGroup("Quick Information"),
        ShowIf("showQuickInformation"),
        SerializeField]
        private Vector2 velocity;

        [BoxGroup("Quick Information"), 
        Tooltip("The last time in milliseconds that the player has jumped."),
        ShowIf("showQuickInformation"),
        SerializeField] 
        private float lastJumped;

        [BoxGroup("Quick Information"),
        Tooltip("Is the player on the ground?"),
        ShowIf("showQuickInformation"),
        SerializeField]
        private bool isGrounded = true;

        [BoxGroup("Quick Information"),
        Tooltip("Is the player on the ground?"),
        ShowIf("showQuickInformation"),
        SerializeField]
        private bool isFacingRight = false;

        [BoxGroup("Quick Information"),
        Tooltip("Should the script accept player input?"),
        ShowIf("showQuickInformation"),
        SerializeField]
        private bool isAllowedToMove = true;
        #endregion


        #region Automation
        [BoxGroup("Automation"),
        SerializeField]
        private bool isBeingControlledByCode = false;

        [BoxGroup("Automation"),
        SerializeField]
        private bool loopThroughAllWaypoints = false;

        [BoxGroup("Automation")] public bool showWaypoints;

        [BoxGroup("Automation"), 
        NaughtyAttributes.ReorderableList,
        ShowIf("showWaypoints"),
        SerializeField] 
        private List<WayPoint> listOfWayPoints = new List<WayPoint>();
        #endregion


        #region Events
        [BoxGroup("Events"), 
        SerializeField] 
        private bool showHasJumpedEvent = false;

        [BoxGroup("Events"), 
        SerializeField] 
        private bool showHasLandedEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will only run on the frame that isGrounded becomes false.", EInfoBoxType.Normal),
        ShowIf("showHasJumpedEvent"),
        SerializeField]
        private UnityEvent hasJumpedEvent;

        [BoxGroup("Events"),
        InfoBox("This event will only run on the frame that isGrounded becomes true.", EInfoBoxType.Normal),
        ShowIf("showHasLandedEvent"),
        SerializeField]
        private UnityEvent hasLandedEvent;
        #endregion


        #region References
        [BoxGroup("References")] public bool showReferences;

        [BoxGroup("References"),
        ShowIf("showReferences"),
        SerializeField]
        private Animator characterAnimator;

        [BoxGroup("References"),
        ShowIf("showReferences"),
        SerializeField]
        private SpriteRenderer characterSpriteRenderer;

        [BoxGroup("References"),
        ShowIf("showReferences"),
        SerializeField]
        private Rigidbody2D characterRigidBody2D;

        [BoxGroup("References"),
        ShowIf("showReferences"),
        SerializeField]
        private BoxCollider2D characterBoxCollider2D;

        [BoxGroup("References"),
        ShowIf("showReferences"),
        SerializeField]
        private CircleCollider2D characterCircleCollider2D;
        #endregion
        #endregion

        #region Private Fields
        private AutomatedMoveModule automatedMoveModule;
        private float horizontalInput;
        private float verticalInput;

        private Collider2D[] debugHits;
        private int debugGroundColliderCounter;
        #endregion

        #region Getters/Setters/Constructors
        public bool IsGrounded { get => isGrounded; set => isGrounded = value; }
        public Animator CharacterAnimator { get => characterAnimator; set => characterAnimator = value; }
        public SpriteRenderer CharacterSpriteRenderer { get => characterSpriteRenderer; set => characterSpriteRenderer = value; }
        public Rigidbody2D CharacterRigidBody2D { get => characterRigidBody2D; set => characterRigidBody2D = value; }
        public BoxCollider2D CharacterBoxCollider2D { get => characterBoxCollider2D; set => characterBoxCollider2D = value; }
        public CircleCollider2D CharacterCircleCollider2D { get => characterCircleCollider2D; set => characterCircleCollider2D = value; }
        public UnityEvent HasJumpedEvent { get => hasJumpedEvent; set => hasJumpedEvent = value; }
        public UnityEvent HasLandedEvent { get => hasLandedEvent; set => hasLandedEvent = value; }
        public List<WayPoint> ListOfWayPoints { get => listOfWayPoints; set => listOfWayPoints = value; }
        public bool IsBeingControlledByCode { get => isBeingControlledByCode; set => isBeingControlledByCode = value; }
        public float LastJumped { get => lastJumped; set => lastJumped = value; }
        public bool IsFacingRight { get => isFacingRight; set => isFacingRight = value; }
        public bool IsAllowedToMove { get => isAllowedToMove; set => isAllowedToMove = value; }
        public Vector2 Velocity { get => velocity; set => velocity = value; }
        public float CharacterSpeed { get => characterSpeed; set => characterSpeed = value; }
        public float CharacterJumpHeight { get => characterJumpHeight; set => characterJumpHeight = value; }
        public float JumpDelay { get => jumpDelay; set => jumpDelay = value; }
        public int CurrentConsecutiveJumps { get => currentConsecutiveJumps; set => currentConsecutiveJumps = value; }
        public int MaxConsecutiveJumps { get => maxConsecutiveJumps; set => maxConsecutiveJumps = value; }
        public LayerMask GroundLayer { get => groundLayer; set => groundLayer = value; }
        public bool CharacterUsesSpriteRenderer { get => characterUsesSpriteRenderer; set => characterUsesSpriteRenderer = value; }
        public bool LoopThroughAllWaypoints { get => loopThroughAllWaypoints; set => loopThroughAllWaypoints = value; }
        #endregion

        #region Animation Methods
        public void AnimateRun(float moveInput) {
            if (CharacterAnimator != null)
                CharacterAnimator.SetFloat("velocity", Mathf.Abs(moveInput));
        }

        public void AnimateJump() {
            if (CharacterAnimator != null)
                CharacterAnimator.SetBool("isJumping", true);
        }

        // Used by the animation tab as an event.
        // To add an event, right click on the top row of the animation tab
        // and click on "Add Animation Event". Then select this method in the dropdown.
        public void EndJump() {
            if (CharacterAnimator != null)
                CharacterAnimator.SetBool("isJumping", false);
        }

        public void FlipCharacter() {
            if (CharacterUsesSpriteRenderer) {
                FlipCharacterWithSpriteRenderer(horizontalInput);
            } else {
                FlipCharacterWithTransform(horizontalInput);
            }
        }

        public void FlipCharacterWithSpriteRenderer(float horizontalInput) {
            if (CharacterSpriteRenderer != null) {
                if (horizontalInput < -0.01f) {
                    CharacterSpriteRenderer.flipX = true;
                } else if (horizontalInput > 0.01f) {
                    CharacterSpriteRenderer.flipX = false;
                }
            }
        }

        // True faces the front of the character to the left, false faces the front of the character to the right.
        public void FlipCharacterWithTransform(float horizontalInput) {
            if ((horizontalInput > 0 && !IsFacingRight) || (horizontalInput < 0 && IsFacingRight)) {
                // Switch the way the player is labelled as facing.
                IsFacingRight = !IsFacingRight;

                Vector3 flipped = transform.localScale;
                flipped.x *= -1f;
                transform.localScale = flipped;
            }
        }
        #endregion

        #region My Methods

        public void GetInput() {
            // Use GetAxisRaw to ensure our input is either 0, 1 or -1.
            horizontalInput = IsAllowedToMove == true ? Input.GetAxisRaw("Horizontal") : 0;
            verticalInput = IsAllowedToMove == true ? Input.GetAxisRaw("Vertical") : 0;

            if(verticalInput > 0) {
                Jump();
            } else {
                if (LastJumped < JumpDelay) {
                    LastJumped += Time.deltaTime;
                }
            }

            if(horizontalInput != 0) {
                Move();
            } else {
                CharacterRigidBody2D.velocity = new Vector2(0, CharacterRigidBody2D.velocity.y);
            }
        }

        public void Move() {
            CharacterRigidBody2D.velocity = new Vector2(horizontalInput * CharacterSpeed, CharacterRigidBody2D.velocity.y);
        }

        public void Crouch(bool isCrouching) {
            CharacterBoxCollider2D.enabled = isCrouching;
        }

        public void Jump() {
            if (IsGrounded) {
                velocity.y = 0;
            }

            // Case: When player holds the jump key when on current consecutive jump count is equal to 1,
            // they will be able to gain additional height and hover.
            // This return stops the above behaviour.
            if (CurrentConsecutiveJumps <= 0) {
                return;
            }

            // Case: When player presses or holds the jump key, they should only be able to jump if they have
            // waited longer than the jump delay.
            // This return stops the HasJumpedEvent from firing more than once during the jump delay duration.
            if(LastJumped < JumpDelay) {
                return;
            }

            if (IsAllowedToMove && CurrentConsecutiveJumps > 0) {
                if (Input.GetButtonDown("Jump")) {
                    // Checking for GetButton vs GetButtonDown and GetButtonUp means that the player
                    // can hold down the Jump button and keep jumping at the same frame that
                    // isGrounded is set to true.
                    // Calculate the velocity required to achieve the target jump height.
                    velocity.y = Mathf.Sqrt(2 * CharacterJumpHeight * Mathf.Abs(Physics2D.gravity.y));
                    AnimateJump();
                    IsGrounded = false;
                    CurrentConsecutiveJumps--;
                    LastJumped = 0f;
                } else if (Input.GetButton("Jump") && IsGrounded) {
                    velocity.y = Mathf.Sqrt(2 * CharacterJumpHeight * Mathf.Abs(Physics2D.gravity.y));
                    AnimateJump();
                    IsGrounded = false;
                    CurrentConsecutiveJumps--;
                    LastJumped = 0f;
                } else if(Input.GetButton("Jump") && !IsGrounded) {
                    // Case: When player holds the jump key, they will be able to gain additional height and hover.
                    // This return stops the above behaviour.
                    return;
                }

                if(!IsGrounded) {
                    HasJumpedEvent.Invoke();
                }
            }

            CharacterRigidBody2D.velocity = new Vector2(CharacterRigidBody2D.velocity.x, velocity.y);
        }

        public void CheckForGround() {
            // Case: The player will be marked as isGrounded when falling off any platforms.
            // Setting isGrounded to false stops the above behaviour.
            //isGrounded = false;

            bool lastFrameIsGrounded = IsGrounded;
            bool currentFrameIsGrounded;

            Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + new Vector3(0, -0.10f, 0), CharacterBoxCollider2D.size, 0);

            debugHits = hits;
            debugGroundColliderCounter = hits.Length;

            int groundColliderCounter = hits.Length;

            for (int i = 0; i < hits.Length; i++) {
                // Ignore any collider that is marked as a trigger.
                if(hits[i].isTrigger) {
                    groundColliderCounter--;
                    continue;
                }

                // Ignore the level bounding box that is used for CineMachine confining.
                if (GameManager.Instance.LevelBoundingBox != null) {
                    if (hits[i] == GameManager.Instance.LevelBoundingBox) {
                        groundColliderCounter--;
                        continue;
                    }
                }

                // Ignore any collider that is on the player gameobject.
                if (hits[i].gameObject.tag == "Player") {
                    groundColliderCounter--;
                    continue;
                }

                if(!((GroundLayer.value & 1 << hits[i].gameObject.layer) == 1 << hits[i].gameObject.layer)) {
                    groundColliderCounter--;
                    continue;
                } 
            }

            if(velocity.y > 0 && groundColliderCounter >= 1) {
                groundColliderCounter--;
            }

            debugGroundColliderCounter = groundColliderCounter;

            currentFrameIsGrounded = groundColliderCounter > 0;

            if(lastFrameIsGrounded && currentFrameIsGrounded) {
                // No change. Was grounded last frame and is grounded this frame.
                IsGrounded = true;
            } else if(!lastFrameIsGrounded && currentFrameIsGrounded) {
                // Just became grounded this frame.
                IsGrounded = true;
                HasLandedEvent.Invoke();
            }
        }

        // TODO: Allow one-way Platforms that allow players to passthrough from below and stand on top.

        // TODO: Apply Knockback

        // TODO: Apply Double/Triple/Quad Jump

        // TODO: Apply Dash

        // TODO: Apply Teleport

        #endregion

        #region Unity Methods
        void OnEnable() {
            automatedMoveModule = new AutomatedMoveModule(this);
            HasJumpedEvent.AddListener(delegate { Debug.Log("HasJumped"); });
            HasLandedEvent.AddListener(delegate { Debug.Log("hasLanded"); CurrentConsecutiveJumps = MaxConsecutiveJumps; });
        }

        void Start() {

        }

        void Update() {
            if (IsBeingControlledByCode) {
                automatedMoveModule.Automate();
            }

            // Get current key down.
            //Debug.Log(Input.inputString);

            GetInput();

            FlipCharacter();
        }

        private void FixedUpdate() {
            velocity = CharacterRigidBody2D.velocity;

            CheckForGround();

            if (IsGrounded && (LastJumped < JumpDelay)) {
                LastJumped += Time.deltaTime;
            }
        }

        void OnDisable() {

        }
        #endregion

        #region Helper Methods
        [NaughtyAttributes.Button("Find References")]
        public void FindReferences() {
            CharacterRigidBody2D = GetComponent<Rigidbody2D>();
            CharacterBoxCollider2D = GetComponent<BoxCollider2D>();
            CharacterCircleCollider2D = GetComponent<CircleCollider2D>();
            CharacterAnimator = GetComponent<Animator>();

            if(CharacterUsesSpriteRenderer) {
                CharacterSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
            }    
        }
        #endregion
    }
}