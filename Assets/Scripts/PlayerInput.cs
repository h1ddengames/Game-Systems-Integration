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

		#region Exposed Fields
		[BoxGroup("Configuration"), SerializeField] private Rigidbody2D playerRigidBody2D;
		[BoxGroup("Configuration"), SerializeField] private BoxCollider2D playerBoxCollider2D;
		[BoxGroup("Configuration"), SerializeField] private SpriteRenderer playerSpriteRenderer;
		[BoxGroup("Configuration"), SerializeField] private Animator playerAnimator;

		[BoxGroup("Configuration"), SerializeField] private float playerSpeed = 5.0f;
		[BoxGroup("Configuration"), SerializeField] private float playerJumpHeight = 5.0f;

		[BoxGroup("Configuration"), SerializeField] private LayerMask groundLayer;
		[BoxGroup("Configuration"), SerializeField] private bool isGrounded;
		[BoxGroup("Configuration"), SerializeField] private bool allowedToMove;
		[BoxGroup("Configuration"), SerializeField] private bool allowedToMoveForward;

		[BoxGroup("Information"), SerializeField] private Vector2 desiredPosition;
		[BoxGroup("Information"), SerializeField] private Vector2 velocity;

		[BoxGroup("Automation"), SerializeField] private bool beingControlledByCode = false;
		[BoxGroup("Automation"), NaughtyAttributes.ReorderableList, SerializeField] private List<WayPoint> listOfWayPoints = new List<WayPoint>();

		[BoxGroup("Events"), SerializeField] private UnityEvent hasMovedLeftEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent hasStoppedMovingLeftEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent hasMovedRightEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent hasStoppedMovingRightEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent hasJumpedEvent;
		[BoxGroup("Events"), SerializeField] private UnityEvent haslandedEvent;

		#endregion

		#region Private Fields
		#endregion

		#region Getters/Setters
		public bool IsGrounded {
			get => isGrounded;
			set {
				isGrounded = value;
				if(value) {
					haslandedEvent.Invoke();
				}
			}
		}
		#endregion

		#region My Methods

		public void AnimateRun(float moveInput) {
			playerAnimator.SetFloat("velocity", Mathf.Abs(moveInput));
		}

		public void AnimateJump() {
			playerAnimator.SetBool("isJumping", true);
		}

		public void EndJump() {
			playerAnimator.SetBool("isJumping", false);
		}

		// Sets up the UnityEvent listeners automatically with the added benefit of keeping the
		// UnityEvents in the inspector clean for any user of this script.
		public void SetupEvents() {
			hasMovedLeftEvent.AddListener(delegate { FlipCharacter(true); });
			hasMovedLeftEvent.AddListener(delegate { Move(-1f); });
			
			hasStoppedMovingLeftEvent.AddListener(delegate { Stop(); });

			hasMovedRightEvent.AddListener(delegate { FlipCharacter(false); });
			hasMovedRightEvent.AddListener(delegate { Move(1f); });
			
			hasStoppedMovingRightEvent.AddListener(delegate { Stop(); });

			hasJumpedEvent.AddListener(new UnityAction(Jump));
		}

		// True faces the front of the character to the left, false faces the front of the character to the right.
		public void FlipCharacter(bool input) {
			if (input) {
				playerSpriteRenderer.flipX = true;
			} else {
				playerSpriteRenderer.flipX = false;
			}
		}

		public void Move(float direction) {
			//desiredPosition = new Vector2(transform.position.x + direction, transform.position.y);
			//MoveToLocation(desiredPosition);

			playerRigidBody2D.velocity = new Vector2(direction * playerSpeed, playerRigidBody2D.velocity.y);
		}

		public void Stop() {
			playerRigidBody2D.velocity = new Vector2(0, 0);
		}

		public void Jump() {
			isGrounded = false;
			if (playerRigidBody2D.velocity.x != 0) {
				playerRigidBody2D.velocity = new Vector2(playerRigidBody2D.velocity.x, playerJumpHeight);
			} else {
				playerRigidBody2D.velocity = new Vector2(0, playerJumpHeight);
			}

			AnimateJump();

		}

		public void MoveToLocation(Transform desiredTransform) {
			MoveToLocation(desiredTransform.position);
		}

		public void MoveToLocation(GameObject objectToMoveTo) {
			MoveToLocation(objectToMoveTo.transform.position);
		}

		public void MoveToLocation(Vector2 desiredPosition) {
			transform.position = Vector2.MoveTowards(transform.position, desiredPosition, playerSpeed * Time.deltaTime);
		}

		public void CheckForObstacle() {
			Vector2 position = transform.position;
			Vector2 direction = Vector2.right;
			float distance = 1f;

			if(playerSpriteRenderer.flipX) {
				direction = Vector2.left;
			}

			Debug.DrawRay(position, direction, Color.red);
			Debug.DrawRay(new Vector2(position.x, position.y + 0.25f), direction, Color.blue);
			Debug.DrawRay(new Vector2(position.x, position.y - 0.25f), direction, Color.blue);
			Debug.DrawRay(new Vector2(position.x, position.y - 0.5f), direction, Color.blue);

			RaycastHit2D hitUpper = Physics2D.Raycast(new Vector2(position.x, position.y + 0.25f), direction, distance, groundLayer);
			RaycastHit2D hitMiddle = Physics2D.Raycast(position, direction, distance, groundLayer);
			RaycastHit2D hitLower = Physics2D.Raycast(new Vector2(position.x, position.y - 0.25f), direction, distance, groundLayer);
			RaycastHit2D hitLowest = Physics2D.Raycast(new Vector2(position.x, position.y - 0.5f), direction, distance, groundLayer);

			if (hitUpper.collider != null || hitMiddle.collider != null || hitLower.collider != null || hitLowest.collider != null) {
				allowedToMoveForward = false;
			} else {
				allowedToMoveForward = true;
			}
		}

		public void CheckForGround() {
			Vector2 position = transform.position;
			Vector2 direction = Vector2.down;
			float distance = 0.75f;

			Debug.DrawRay(position, direction, Color.green);
			RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
			if (hit.collider != null) {
				isGrounded = true;
			} else {
				isGrounded = false;
			}
		}
		#endregion

		#region Unity Methods
		void OnEnable() {
			
		}
		
		void Start() {
			SetupEvents();
		}

		void Update() {
			velocity = playerRigidBody2D.velocity;
			if(velocity.y < 0) {
				velocity = new Vector2(velocity.x, velocity.y * 2);
			}

			if(beingControlledByCode) {
				if (listOfWayPoints.Count != 0) {
					if (!listOfWayPoints[0].HasArrived) {
						MoveToLocation(listOfWayPoints[0].Location);
					}

					// Check to see if the player is close enough to the linked teleporter.
					if (Vector2.Distance(transform.position, listOfWayPoints[0].Location) < 0.2f) {
						listOfWayPoints[0].HasArrived = true;
						listOfWayPoints.RemoveAt(0);
					}
				}
			}

			CheckForGround();

			if (IsGrounded) {
				if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow)) {
					hasJumpedEvent.Invoke();
				}

				if (Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.UpArrow)) {
					Jump();
					hasJumpedEvent.Invoke();
				}
			}

			CheckForObstacle();

			if(allowedToMoveForward) {
				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
					hasMovedLeftEvent.Invoke();
				} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
					hasMovedRightEvent.Invoke();
				}
			} else {
				if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) {
					FlipCharacter(true);
				} else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) {
					FlipCharacter(false);
				}
			}
			

			AnimateRun(playerRigidBody2D.velocity.x);

			if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.LeftArrow)) {
				hasStoppedMovingLeftEvent.Invoke();
			} else if (Input.GetKeyUp(KeyCode.D) || Input.GetKeyUp(KeyCode.RightArrow)) {
				hasStoppedMovingRightEvent.Invoke();
			}
		}
		
		void OnDisable() {
			
		}
		#endregion

		#region Helper Methods
		#endregion
	}
}