// Created by h1ddengames
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
	public class Teleporter : MonoBehaviour {
		#region Exposed Fields
		[BoxGroup("Configuration"), 
		Tooltip("Which teleporter should this teleporter teleport to?"), 
		SerializeField] 
		private Teleporter linkedTeleporter;

		[BoxGroup("Configuration"), 
		Tooltip("How fast should the player be teleported?"), 
		SerializeField] 
		private float speed = 10.0f;

		[BoxGroup("Configuration"), 
		SerializeField] 
		private bool isUsingTeleporter;

		[BoxGroup("Configuration"), 
		SerializeField] 
		private bool isMoving = false;

		[BoxGroup("Configuration"), 
		SerializeField]
		private bool isInCollider = false;

		[BoxGroup("Inputs"), 
		Tooltip("What key should be pressed in order to activate the teleporter?"), 
		SerializeField] 
		private KeyCode activateTeleporterKey = KeyCode.W;

		[BoxGroup("Events"),
		InfoBox("This event will run on the frame that the player's collider enters the teleporter's collider", EInfoBoxType.Normal), 
		SerializeField] 
		private UnityEvent enteredTeleporterColliderEvent;

		[BoxGroup("Events"), 
		InfoBox("This event will run on the frame that the player's collider exits the teleporter's collider", EInfoBoxType.Normal), 
		SerializeField] 
		private UnityEvent exitedTeleporterColliderEvent;

		[BoxGroup("Events"), 
		InfoBox("This event will run on the frame that the player's collider enters the teleporter's collider AND the player presses the action or teleport button.", EInfoBoxType.Warning), 
		InfoBox("This event will run on every frame that isGrounded is false", EInfoBoxType.Normal), 
		SerializeField] 
		private UnityEvent pressedActivateKeyEvent;
		#endregion

		#region Private Fields
		#endregion

		#region Getters/Setters/Constructors
		public Teleporter LinkedTeleporter { get => linkedTeleporter; set => linkedTeleporter = value; }
		public float Speed { get => speed; set => speed = value; }
		public bool IsUsingTeleporter { get => isUsingTeleporter; set => isUsingTeleporter = value; }
		public bool IsMoving { get => isMoving; set => isMoving = value; }
		public bool IsInCollider { get => isInCollider; set => isInCollider = value; }
		public KeyCode ActivateTeleporterKey { get => activateTeleporterKey; set => activateTeleporterKey = value; }
		public UnityEvent EnteredTeleporterColliderEvent { get => enteredTeleporterColliderEvent; set => enteredTeleporterColliderEvent = value; }
		public UnityEvent ExitedTeleporterColliderEvent { get => exitedTeleporterColliderEvent; set => exitedTeleporterColliderEvent = value; }
		public UnityEvent PressedActivateKeyEvent { get => pressedActivateKeyEvent; set => pressedActivateKeyEvent = value; }
		#endregion

		#region Test Methods
		public void InsideTeleporterColliderTest() {
			Debug.Log("Inside the collider.");
		}

		public void OutsideTeleporterColliderTest() {
			Debug.Log("Outside the collider.");
		}

		public void PressedKeyTeleporterColliderTest() {
			Debug.Log("Pressed inside collider.");
		}
		#endregion

		#region My Methods
		public void TeleportToLinkedTeleporter() {
			//GameManager.Instance.PlayerCharacterController2D.IsGravityOn = false;
			GameManager.Instance.PlayerboxCollider2D.enabled = false;
			GameManager.Instance.PlayerCharacterController2D.IsAllowedToMove = false;
			IsMoving = true;
			IsUsingTeleporter = true;
		}
		#endregion

		#region Unity Methods
		void OnEnable() {
			PressedActivateKeyEvent.AddListener(delegate { TeleportToLinkedTeleporter(); });
		}
		
		void Start() {
			
		}

		void Update() {
			if (isUsingTeleporter) {
				GameManager.Instance.PlayerRigidBody2D.bodyType = RigidbodyType2D.Kinematic;
			} else {
				GameManager.Instance.PlayerRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
				
			}

			if (IsInCollider && Input.GetKeyDown(ActivateTeleporterKey)) {
				PressedActivateKeyEvent.Invoke();
			}

			if (IsMoving) {
				// Check to see if the player is close enough to the linked teleporter.
				if (Vector2.Distance(GameManager.Instance.Player.transform.position, LinkedTeleporter.transform.position) < 0.2f) {

					IsMoving = false;
					//GameManager.Instance.PlayerCharacterController2D.IsGravityOn = true;
					GameManager.Instance.PlayerboxCollider2D.enabled = true;
				}

				// Move the player to the linked teleporter at a constant speed.
				//GameManager.Instance.Player.transform.position = Vector2.MoveTowards(GameManager.Instance.Player.transform.position, LinkedTeleporter.transform.position, Speed * Time.deltaTime);
				GameManager.Instance.Player.transform.position = LinkedTeleporter.transform.position;
			} else {
				IsUsingTeleporter = false;
			}
		}

		void OnDisable() {
			
		}

		private void OnTriggerEnter2D(Collider2D collision) {
			if(collision.tag == "Player") {
				GameManager.Instance.PlayerCharacterController2D.IsAllowedToMove = true;
				IsInCollider = true;
				EnteredTeleporterColliderEvent.Invoke();
			}
		}

		private void OnTriggerExit2D(Collider2D collision) {
			if (collision.tag == "Player") {
				IsInCollider = false;
				ExitedTeleporterColliderEvent.Invoke();
			}
		}
		#endregion

		#region Helper Methods
		#endregion
	}
}