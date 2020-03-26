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
        #region Configuration
        [BoxGroup("Configuration"),
        SerializeField]
        private bool showConfiguration = false;

        [BoxGroup("Configuration"),
        Tooltip("How fast should the player be able to move left and right?"),
        ShowIf("showConfiguration"),
        SerializeField]
        private float characterMoveSpeed = 5.0f;

        [BoxGroup("Configuration"),
        Tooltip("How fast should the player be able to move left and right?"),
        ShowIf("showConfiguration"),
        SerializeField]
        private float characterJumpHeight = 1.0f;

        [BoxGroup("Configuration"),
        Tooltip("The amount of time the player needs to wait until they can jump again."),
        ShowIf("showConfiguration"),
        SerializeField]
        private float characterJumpDelay = 0.25f;

        [BoxGroup("Configuration"),
        Tooltip("How many times should the player be able to jump before landing on the ground?"),
        ShowIf("showConfiguration"),
        SerializeField]
        private int currentConsecutiveJumps = 2;

        [BoxGroup("Configuration"),
        Tooltip("How many times should the player be able to jump before landing on the ground?"),
        ShowIf("showConfiguration"),
        SerializeField]
        private int maxConsecutiveJumps = 2;

        [BoxGroup("Configuration"),
        Tooltip("What layers should the player collide with to reset the jump count?"),
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
        private Vector2 movementInput;

        [BoxGroup("Quick Information"),
        Tooltip("The last time in milliseconds that the player has jumped."),
        ShowIf("showQuickInformation"),
        SerializeField]
        private float lastJumped;

        [BoxGroup("Quick Information"),
        Tooltip("Is the character on the ground?"),
        ShowIf("showQuickInformation"),
        SerializeField]
        private bool isCharacterGrounded = true;

        [BoxGroup("Quick Information"),
        Tooltip("Is the character facing right?"),
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

        [BoxGroup("Automation"),
        SerializeField] private bool showWaypoints = false;

        [BoxGroup("Automation"),
        NaughtyAttributes.ReorderableList,
        ShowIf("showWaypoints"),
        SerializeField]
        private List<WayPoint> listOfWayPoints = new List<WayPoint>();
        #endregion

        #region Events
        #region Has Jumped Event
        [BoxGroup("Events"),
        SerializeField]
        private bool showHasJumpedEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will only run on the frame that isGrounded becomes false.", EInfoBoxType.Normal),
        ShowIf("showHasJumpedEvent"),
        SerializeField]
        private UnityEvent hasJumpedEvent;
        #endregion

        #region Has Landed Event
        [BoxGroup("Events"),
        SerializeField]
        private bool showHasLandedEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will only run on the frame that isGrounded becomes true.", EInfoBoxType.Normal),
        ShowIf("showHasLandedEvent"),
        SerializeField]
        private UnityEvent hasLandedEvent;
        #endregion

        #region Is Grounded Event
        [BoxGroup("Events"),
        SerializeField]
        private bool showIsGroundedEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will run on every frame that the character is grounded.", EInfoBoxType.Normal),
        ShowIf("showIsGroundedEvent"),
        SerializeField]
        private UnityEvent isGroundedEvent;
        #endregion

        #region Is Not Grounded Event
        [BoxGroup("Events"),
        SerializeField]
        private bool showIsNotGroundedEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will run on every frame that the character is not grounded.", EInfoBoxType.Normal),
        ShowIf("showIsNotGroundedEvent"),
        SerializeField]
        private UnityEvent isNotGroundedEvent;
        #endregion

        #region Has Changed Direction Event
        [BoxGroup("Events"),
        SerializeField]
        private bool showHasChangedDirectionEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will only run on the frame that the character changes direction.", EInfoBoxType.Normal),
        ShowIf("showHasChangedDirectionEvent"),
        SerializeField]
        private UnityEvent hasChangedDirectionEvent;
        #endregion

        #region Is Moving Event
        [BoxGroup("Events"),
        SerializeField]
        private bool showIsMovingEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will run on every frame that the character's velocity is not 0.", EInfoBoxType.Normal),
        ShowIf("showIsMovingEvent"),
        SerializeField]
        private UnityEvent isMovingEvent;
        #endregion

        #region Is Not Moving Event
        [BoxGroup("Events"),
        SerializeField]
        private bool showIsNotMovingEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will run on every frame that the character's velocity is 0.", EInfoBoxType.Normal),
        ShowIf("showIsNotMovingEvent"),
        SerializeField]
        private UnityEvent isNotMovingEvent;
        #endregion

        #region Has Started Moving Event
        [BoxGroup("Events"),
        SerializeField]
        private bool showHasStartedMovingEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will only run on the frame that the character starts moving.", EInfoBoxType.Normal),
        ShowIf("showHasStartedMovingEvent"),
        SerializeField]
        private UnityEvent hasStartedMovingEvent;
        #endregion

        #region Is Not Grounded Event
        [BoxGroup("Events"),
        SerializeField]
        private bool showHasStoppedMovingEvent = false;

        [BoxGroup("Events"),
        InfoBox("This event will only run on the frame that the character stops moving.", EInfoBoxType.Normal),
        ShowIf("showHasStoppedMovingEvent"),
        SerializeField]
        private UnityEvent hasStoppedMovingEvent;
        #endregion
        #endregion

        #region References
        [BoxGroup("References"),
         SerializeField] 
        private bool showReferences = false;

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

        [BoxGroup("References"),
            ShowIf("showReferences"),
            SerializeField]
        private PhysicsMaterial2D stickyMaterial;

        [BoxGroup("References"),
            ShowIf("showReferences"),
            SerializeField]
        private PhysicsMaterial2D slipperyMaterial;
        #endregion
        #endregion

        #region Private Fields
        private AutomatedMoveModule automatedMoveModule;
        private AnimationModule animationModule;
        private PlayerInputModule playerInputModule;

        private Collider2D[] debugHits;
        private int debugGroundColliderCounter;
        #endregion

        #region Getters/Setters/Constructors
        public float CharacterMoveSpeed { get => characterMoveSpeed; set => characterMoveSpeed = value; }
        public float CharacterJumpHeight { get => characterJumpHeight; set => characterJumpHeight = value; }
        public float CharacterJumpDelay { get => characterJumpDelay; set => characterJumpDelay = value; }
        public int CurrentConsecutiveJumps { get => currentConsecutiveJumps; set => currentConsecutiveJumps = value; }
        public int MaxConsecutiveJumps { get => maxConsecutiveJumps; set => maxConsecutiveJumps = value; }
        public LayerMask GroundLayer { get => groundLayer; set => groundLayer = value; }

        public float LastJumped { get => lastJumped; set => lastJumped = value; }
        public bool IsCharacterGrounded { get => isCharacterGrounded; set => isCharacterGrounded = value; }
        public bool IsFacingRight { get => isFacingRight; set => isFacingRight = value; }
        public bool IsAllowedToMove { get => isAllowedToMove; set => isAllowedToMove = value; }

        public bool IsBeingControlledByCode { get => isBeingControlledByCode; set => isBeingControlledByCode = value; }
        public bool LoopThroughAllWaypoints { get => loopThroughAllWaypoints; set => loopThroughAllWaypoints = value; }
        public List<WayPoint> ListOfWayPoints { get => listOfWayPoints; set => listOfWayPoints = value; }

        public UnityEvent HasJumpedEvent { get => hasJumpedEvent; set => hasJumpedEvent = value; }
        public UnityEvent HasLandedEvent { get => hasLandedEvent; set => hasLandedEvent = value; }
        public UnityEvent IsGroundedEvent { get => isGroundedEvent; set => isGroundedEvent = value; }
        public UnityEvent IsNotGroundedEvent { get => isNotGroundedEvent; set => isNotGroundedEvent = value; }
        public UnityEvent HasChangedDirectionEvent { get => hasChangedDirectionEvent; set => hasChangedDirectionEvent = value; }
        public UnityEvent IsMovingEvent { get => isMovingEvent; set => isMovingEvent = value; }
        public UnityEvent IsNotMovingEvent { get => isNotMovingEvent; set => isNotMovingEvent = value; }
        public UnityEvent HasStartedMovingEvent { get => hasStartedMovingEvent; set => hasStartedMovingEvent = value; }
        public UnityEvent HasStoppedMovingEvent { get => hasStoppedMovingEvent; set => hasStoppedMovingEvent = value; }

        public bool CharacterUsesSpriteRenderer { get => characterUsesSpriteRenderer; set => characterUsesSpriteRenderer = value; }
        public Animator CharacterAnimator { get => characterAnimator; set => characterAnimator = value; }
        public SpriteRenderer CharacterSpriteRenderer { get => characterSpriteRenderer; set => characterSpriteRenderer = value; }
        public Rigidbody2D CharacterRigidBody2D { get => characterRigidBody2D; set => characterRigidBody2D = value; }
        public BoxCollider2D CharacterBoxCollider2D { get => characterBoxCollider2D; set => characterBoxCollider2D = value; }
        public CircleCollider2D CharacterCircleCollider2D { get => characterCircleCollider2D; set => characterCircleCollider2D = value; }
        #endregion

        #region My Methods
        public void CheckForGround() {

        }

        public void Crouch() {
            CharacterBoxCollider2D.enabled = !CharacterBoxCollider2D.enabled;
        }

        public void Jump() {
            CharacterRigidBody2D.AddForce(new Vector2(0f, CharacterJumpHeight), ForceMode2D.Impulse);
        }

        public void Move() {
            float xPosition = CharacterRigidBody2D.position.x + (movementInput.x * CharacterMoveSpeed * Time.fixedDeltaTime);
            float yPosition = CharacterRigidBody2D.position.y + (0.5f * Physics2D.gravity.y * Time.fixedDeltaTime);
            CharacterRigidBody2D.MovePosition(new Vector2(xPosition, yPosition));
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
        #endregion

        #region Unity Methods
        private void OnEnable() {

        }

        void Awake() {
            automatedMoveModule = new AutomatedMoveModule(this);
            animationModule = new AnimationModule(gameObject);
            playerInputModule = PlayerInputModule.Instance;
        }

        // For Physics related calculations of objects. Movement methods should be called here.
        private void FixedUpdate() {

        }

        // For moving the visuals of objects. Animation methods should be called here.
        private void LateUpdate() {

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