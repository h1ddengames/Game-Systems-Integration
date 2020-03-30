// Created by h1ddengames

using UnityEngine;

namespace h1ddengames {
	public class Teleporter : MonoBehaviour {
		#region Exposed Fields
		[SerializeField] private Teleporter linkedTeleporter;
		[SerializeField] private bool isTeleporterUsable = true;
		[SerializeField] private float teleportSpeed = 3f;
		#endregion

		#region Private Fields
		private GameObject player;
		private bool isReadyToTeleportPlayer;
		private bool isTeleporting;
		#endregion

		#region Getters/Setters/Constructors
		#endregion

		#region My Methods
		public void Teleport() {
			if(teleportSpeed == 0) {
				player.transform.position = linkedTeleporter.transform.position;
			} else {
				player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Kinematic;
				isTeleporting = true;
			}
		}
		#endregion

		#region Unity Methods
		void OnEnable() {

		}

		void Start() {

		}

		void Update() {
			if(!isTeleporterUsable) {
				return;
			}

			if(Input.GetKeyDown(KeyCode.W) && isReadyToTeleportPlayer) {
				Teleport();
			}

			if(isTeleporting) {
				// Check to see if the player is close enough to the linked teleporter.
				if(Vector2.Distance(player.transform.position, linkedTeleporter.transform.position) < 0.2f) {
					isTeleporting = false;
					player.GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
					return;
				}

				player.transform.position = Vector2.MoveTowards(player.transform.position, linkedTeleporter.transform.position, teleportSpeed * Time.deltaTime);
			}
		}

		void OnDisable() {

		}

		private void OnValidate() {
			if(teleportSpeed < 0) {
				teleportSpeed = 0;
			}
		}

		private void OnTriggerEnter2D(Collider2D collision) {
			if(collision.tag == "Player") {
				player = collision.gameObject;
				isReadyToTeleportPlayer = true;
			}
		}

		private void OnTriggerExit2D(Collider2D collision) {
			if(collision.tag == "Player") {
				isReadyToTeleportPlayer = false;
			}
		}
		#endregion

		#region Helper Methods
		#endregion
	}
}