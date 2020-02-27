// Created by h1ddengames
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

namespace h1ddengames {
	public class PlayerInput : MonoBehaviour {
		#region Exposed Fields
		[InfoBox("RigidBody2D, BoxCollider2D, and Animator components should be on the top level GameObject. (The GameObject this script is attached to.)", InfoBoxType.Normal)]
		[BoxGroup("References"), SerializeField] private Rigidbody2D playerRigidBody2D;
		[BoxGroup("References"), SerializeField] private BoxCollider2D playerBoxCollider2D;
		[BoxGroup("References"), SerializeField] private Animator playerAnimator;
		[BoxGroup("References"), InfoBox("SpriteRenderer should be on the second level GameObject. (The GameObject that is a child of the GameObject this script is attached to.)", InfoBoxType.Normal), SerializeField] private SpriteRenderer playerSpriteRenderer;
		[BoxGroup("References"), SerializeField] private PolygonCollider2D levelBoundingBox;

		[BoxGroup("Configuration"), Tooltip("How fast should the player be able to move left and right?"), SerializeField] private float playerSpeed = 5.0f;
		[BoxGroup("Configuration"), Tooltip("How high should the player be able to jump?"), SerializeField] private float playerJumpHeight = 5.0f;
		[BoxGroup("Configuration"), Tooltip("How many times should the player be able to jump before landing on the ground?"), SerializeField] private int currentConsecutiveJumps = 4;
		[BoxGroup("Configuration"), Tooltip("How many times should the player be able to jump before landing on the ground?"), SerializeField] private int maxConsecutiveJumps = 4;

		[BoxGroup("Configuration"), SerializeField] private LayerMask groundLayer;
		[BoxGroup("Configuration"), Tooltip("Is the player on the ground?"), SerializeField] private bool isGrounded = true;
		[BoxGroup("Configuration"), Tooltip("Should the script accept player input?"), SerializeField] private bool isAllowedToMove = true;
		[BoxGroup("Configuration"), Tooltip("Using a raycast, is the area in front of the player a place where the player can move to?"), SerializeField] private bool isAllowedToMoveForward = true;

		[BoxGroup("Information"), SerializeField] private Vector2 desiredPosition;
		[BoxGroup("Information"), SerializeField] private Vector2 velocity;

		[BoxGroup("Automation"), SerializeField] private bool beingControlledByCode = false;
		[BoxGroup("Automation"), NaughtyAttributes.ReorderableList, SerializeField] private List<WayPoint> listOfWayPoints = new List<WayPoint>();

		[BoxGroup("Events"), SerializeField] private UnityEvent hasMovedLeftEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent hasStoppedMovingLeftEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent hasMovedRightEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent hasStoppedMovingRightEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent hasJumpedEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent hasLandedEvent;
		#endregion

		#region Private Fields
		#endregion

		#region Getters/Setters
		public Rigidbody2D PlayerRigidBody2D { get => playerRigidBody2D; set => playerRigidBody2D = value; }
		public BoxCollider2D PlayerBoxCollider2D { get => playerBoxCollider2D; set => playerBoxCollider2D = value; }
		public Animator PlayerAnimator { get => playerAnimator; set => playerAnimator = value; }
		public SpriteRenderer PlayerSpriteRenderer { get => playerSpriteRenderer; set => playerSpriteRenderer = value; }
		public PolygonCollider2D LevelBoundingBox { get => levelBoundingBox; set => levelBoundingBox = value; }
		public float PlayerSpeed { get => playerSpeed; set => playerSpeed = value; }
		public float PlayerJumpHeight { get => playerJumpHeight; set => playerJumpHeight = value; }
		public LayerMask GroundLayer { get => groundLayer; set => groundLayer = value; }
		public bool IsGrounded {
			get => isGrounded;
			set {
				isGrounded = value;
				if (value) {
					HasLandedEvent.Invoke();
				}
			}
		}

		public bool IsAllowedToMove { get => isAllowedToMove; set => isAllowedToMove = value; }
		public bool IsAllowedToMoveForward { get => isAllowedToMoveForward; set => isAllowedToMoveForward = value; }
		public Vector2 DesiredPosition { get => desiredPosition; set => desiredPosition = value; }
		public Vector2 Velocity { get => velocity; set => velocity = value; }
		public bool BeingControlledByCode { get => beingControlledByCode; set => beingControlledByCode = value; }
		public List<WayPoint> ListOfWayPoints { get => listOfWayPoints; set => listOfWayPoints = value; }
		public UnityEvent HasMovedLeftEvent { get => hasMovedLeftEvent; set => hasMovedLeftEvent = value; }
		public UnityEvent HasStoppedMovingLeftEvent { get => hasStoppedMovingLeftEvent; set => hasStoppedMovingLeftEvent = value; }
		public UnityEvent HasMovedRightEvent { get => hasMovedRightEvent; set => hasMovedRightEvent = value; }
		public UnityEvent HasStoppedMovingRightEvent { get => hasStoppedMovingRightEvent; set => hasStoppedMovingRightEvent = value; }
		public UnityEvent HasJumpedEvent { get => hasJumpedEvent; set => hasJumpedEvent = value; }
		public UnityEvent HasLandedEvent { get => hasLandedEvent; set => hasLandedEvent = value; }
		#endregion

		#region Animaton/Visual Methods
		public void AnimateRun(float moveInput) {
			PlayerAnimator.SetFloat("velocity", Mathf.Abs(moveInput));
		}

		public void AnimateJump() {
			PlayerAnimator.SetBool("isJumping", true);
		}

		public void EndJump() {
			PlayerAnimator.SetBool("isJumping", false);
		}

		// True faces the front of the character to the left, false faces the front of the character to the right.
		public void FlipCharacter(bool input) {
			if (input) {
				PlayerSpriteRenderer.flipX = true;
			} else {
				PlayerSpriteRenderer.flipX = false;
			}
		}
		#endregion

		#region Setup Methods
		[ContextMenu("Find References")]
		public void FindReferences() {
			PlayerRigidBody2D = GetComponent<Rigidbody2D>();
			PlayerBoxCollider2D = GetComponent<BoxCollider2D>();
			PlayerAnimator = GetComponent<Animator>();
			PlayerSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
		}

		// Sets up the UnityEvent listeners automatically with the added benefit of keeping the
		// UnityEvents in the inspector clean for any user of this script.
		[ContextMenu("Setup Events")]
		public void SetupEvents() {
			HasMovedLeftEvent.AddListener(delegate { FlipCharacter(true); });
			HasMovedLeftEvent.AddListener(delegate { Move(-1f); });

			HasStoppedMovingLeftEvent.AddListener(delegate { Stop(); });

			HasMovedRightEvent.AddListener(delegate { FlipCharacter(false); });
			HasMovedRightEvent.AddListener(delegate { Move(1f); });

			HasStoppedMovingRightEvent.AddListener(delegate { Stop(); });

			HasJumpedEvent.AddListener(new UnityAction(Jump));
			HasJumpedEvent.AddListener(delegate { currentConsecutiveJumps -= 1; });

			HasLandedEvent.AddListener(delegate { currentConsecutiveJumps = maxConsecutiveJumps; });
		}
		#endregion

		#region Movement Methods
		public void Move(float direction) {
			PlayerRigidBody2D.velocity = new Vector2(direction * PlayerSpeed, PlayerRigidBody2D.velocity.y);
		}

		public void Stop() {
			PlayerRigidBody2D.velocity = new Vector2(0, 0);
		}

		public void Jump() {
			IsGrounded = false;
			if (PlayerRigidBody2D.velocity.x != 0) {
				PlayerRigidBody2D.velocity = new Vector2(PlayerRigidBody2D.velocity.x, PlayerJumpHeight);
			} else {
				PlayerRigidBody2D.velocity = new Vector2(0, PlayerJumpHeight);
			}

			AnimateJump();
		}
		#endregion

		#region Automation Methods
		public void MoveToLocation(Transform desiredTransform) {
			MoveToLocation(desiredTransform.position);
		}

		public void MoveToLocation(GameObject objectToMoveTo) {
			MoveToLocation(objectToMoveTo.transform.position);
		}

		public void MoveToLocation(Vector2 desiredPosition) {
			transform.position = Vector2.MoveTowards(transform.position, desiredPosition, PlayerSpeed * Time.deltaTime);
		}
		#endregion

		#region Obstacle Checker
		public void CheckForObstacle() {
			Collider2D[] hits;
			if (PlayerSpriteRenderer.flipX) {
				// When facing left, the overlapbox should be moved further to the left to put the box in front of the player.
				hits = Physics2D.OverlapBoxAll(transform.position - new Vector3(1, 0, 0), PlayerBoxCollider2D.size, 0);
			} else {
				hits = Physics2D.OverlapBoxAll(transform.position + new Vector3(1, 0, 0), PlayerBoxCollider2D.size, 0);
			}

			foreach (var collider in hits) {
				if (collider.isTrigger)
					continue;

				if (collider == LevelBoundingBox)
					continue;

				if (collider == PlayerBoxCollider2D)
					continue;

				if (collider.gameObject.tag == "Player")
					continue;

				IsAllowedToMoveForward = false;
			}

			IsAllowedToMoveForward = true;
		}

		public void CheckForGround() {
			Collider2D[] hits = Physics2D.OverlapBoxAll(transform.position + new Vector3(0, -0.5f, 0), PlayerBoxCollider2D.size, 0);

			foreach (var collider in hits) {
				if (collider.isTrigger)
					continue;

				if (LevelBoundingBox != null) {
					if (collider == LevelBoundingBox)
						continue;
				}

				if (collider == PlayerBoxCollider2D)
					continue;

				if (collider.gameObject.tag == "Player")
					continue;

				IsGrounded = true;
			}
		}
		#endregion

		#region Unity Methods
		void OnEnable() {

		}

		void Start() {
			FindReferences();
			SetupEvents();
		}

		void Update() {
			Velocity = PlayerRigidBody2D.velocity;
			if (Velocity.y < 0) {
				Velocity = new Vector2(Velocity.x, Velocity.y * 2);
			}

			// For automating the movement of the player.
			if (BeingControlledByCode) {
				if (ListOfWayPoints.Count != 0) {
					if (!ListOfWayPoints[0].HasArrived) {
						MoveToLocation(ListOfWayPoints[0].Location);
					}

					// Check to see if the player is close enough to the linked teleporter.
					if (Vector2.Distance(transform.position, ListOfWayPoints[0].Location) < 0.2f) {
						ListOfWayPoints[0].HasArrived = true;
						ListOfWayPoints.RemoveAt(0);
					}
				}
			}

			CheckForGround();

			if (IsGrounded && IsAllowedToMove) {
				if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) {
					Jump();
					HasJumpedEvent.Invoke();
				}
			} else if (IsAllowedToMove && currentConsecutiveJumps > 0) {
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
					HasJumpedEvent.Invoke();
				}
			}

			CheckForObstacle();

			if (IsAllowedToMoveForward && IsAllowedToMove) {
				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
					HasMovedLeftEvent.Invoke();
				} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
					HasMovedRightEvent.Invoke();
				}
			} else {
				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
					FlipCharacter(true);
				} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
					FlipCharacter(false);
				}
			}

			AnimateRun(PlayerRigidBody2D.velocity.x);

			if (IsAllowedToMove) {
				if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
					HasStoppedMovingLeftEvent.Invoke();
				} else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) {
					HasStoppedMovingRightEvent.Invoke();
				}
			}
		}

		void OnDisable() {

		}
		#endregion

		#region Helper Methods
		#endregion
	}

	[Serializable]
	public class WayPoint {
		[SerializeField] private Vector2 location;
		[SerializeField] private bool hasArrived;

		public WayPoint(Vector2 location, bool hasArrived) {
			this.location = location;
			this.hasArrived = hasArrived;
		}

		public Vector2 Location { get => location; set => location = value; }
		public bool HasArrived { get => hasArrived; set => hasArrived = value; }
	}
}