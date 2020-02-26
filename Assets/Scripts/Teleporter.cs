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
		[BoxGroup("Configuration"), Tooltip("Which teleporter should this teleporter teleport to?"), SerializeField] private Teleporter linkedTeleporter;
		[BoxGroup("Configuration"), Tooltip("How fast should the player be teleported?"), SerializeField] private float speed = 10.0f;
		[BoxGroup("Configuration"), SerializeField] private bool isUsingTeleporter;

		[BoxGroup("Inputs"), Tooltip("What key should be pressed in order to activate the teleporter?"), SerializeField] private KeyCode activateTeleporterKey = KeyCode.W;

		[BoxGroup("Events"), InfoBox("This event will run on the frame that the player's collider enters the teleporter's collider", InfoBoxType.Normal), SerializeField] private UnityEvent enteredTeleporterColliderEvent;
		[BoxGroup("Events"), InfoBox("This event will run on the frame that the player's collider exits the teleporter's collider", InfoBoxType.Normal), SerializeField] private UnityEvent exitedTeleporterColliderEvent;
		[BoxGroup("Events"), InfoBox("This event will run on the frame that the player's collider enters the teleporter's collider AND the player presses the action or teleport button.", InfoBoxType.Warning), InfoBox("This event will run on every frame that isGrounded is false", InfoBoxType.Normal), SerializeField] private UnityEvent pressedActivateKeyEvent;
		#endregion

		#region Private Fields
		private bool isInCollider = false;
		[SerializeField] private bool isMoving = false;

		public bool IsMoving { get => isMoving; set => isMoving = value; } 
		#endregion

		#region Getters/Setters/Constructors
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
			GameManager.Instance.PlayerSpriteRenderer.enabled = false;
			GameManager.Instance.PlayerCharacterController2D.IsGravityOn = false;
			GameManager.Instance.PlayerboxCollider2D.enabled = false;
			IsMoving = true;
			isUsingTeleporter = true;
		}
		#endregion

		#region Unity Methods
		void OnEnable() {
			
		}
		
		void Start() {
			
		}

		void Update() {
			//if (isUsingTeleporter) {
			//	GameManager.Instance.PlayerRigidBody2D.bodyType = RigidbodyType2D.Static;
			//} else {
			//	GameManager.Instance.PlayerRigidBody2D.bodyType = RigidbodyType2D.Dynamic;
			//}

			if (isInCollider && Input.GetKeyDown(activateTeleporterKey)) {
				pressedActivateKeyEvent.Invoke();
			}

			if(IsMoving) {
				// Check to see if the player is close enough to the linked teleporter.
				if(Vector2.Distance(GameManager.Instance.Player.transform.position, linkedTeleporter.transform.position) < 0.2f) {

					IsMoving = false;
					GameManager.Instance.PlayerCharacterController2D.IsGravityOn = true;
					GameManager.Instance.PlayerboxCollider2D.enabled = true;
				}

				// Move the player to the linked teleporter at a constant speed.
				GameManager.Instance.Player.transform.position = Vector2.MoveTowards(GameManager.Instance.Player.transform.position, linkedTeleporter.transform.position, speed * Time.deltaTime);
			} else {
				isUsingTeleporter = false;
			}
		}

		void OnDisable() {
			
		}

		private void OnTriggerEnter2D(Collider2D collision) {
			if(collision.tag == "Player") {
				GameManager.Instance.PlayerSpriteRenderer.enabled = true;
				isInCollider = true;
				enteredTeleporterColliderEvent.Invoke();
			}
		}

		private void OnTriggerExit2D(Collider2D collision) {
			if (collision.tag == "Player") {
				isInCollider = false;
				exitedTeleporterColliderEvent.Invoke();
			}
		}
		#endregion

		#region Helper Methods
		#endregion
	}
}