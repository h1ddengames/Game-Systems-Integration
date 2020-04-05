// Created by h1ddengames
// Attributes being used within this class require:
// https://github.com/dbrizov/NaughtyAttributes

using UnityEngine;
using NaughtyAttributes;
using ReorderableListAttribute = NaughtyAttributes.ReorderableListAttribute;

namespace h1ddengames {
	public class DebugModule : MonoBehaviour {
		#region Exposed Fields
		[BoxGroup("Configuration"), SerializeField] private bool debug;
		[BoxGroup("Configuration"), SerializeField] private bool displayDebug;

		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private Vector3 characterTransform;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private Vector2 characterMovementInput;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private Vector2 characterVelocity;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private bool isBeingControlledByCode = false;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private bool isAcceptingInput = true;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private bool isGrounded = true;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private bool isJumping = false;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private bool isFacingRight = false;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private float lastJumped = 0;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private bool hasChangedDirectionThisFrame = false;
		[BoxGroup("Debug"), ShowIf("displayDebug"), SerializeField] private int debugColliderOverlapsCounter;
		[BoxGroup("Debug"), ShowIf("displayDebug"), ReorderableList, SerializeField] private Collider2D[] debugColliderOverlaps;
		#endregion

		#region Private Fields
		private CharacterController2D characterController2D;
		#endregion

		#region Getters/Setters/Constructors
		#endregion

		#region My Methods
		#endregion

		#region Unity Methods
		void Start() {
			characterController2D = GetComponent<CharacterController2D>();
		}

		void Update() {
			if(debug) {
				characterTransform = transform.position;
				characterMovementInput = characterController2D.CharacterMovementInput;
				characterVelocity = characterController2D.CharacterVelocity;
				isBeingControlledByCode = characterController2D.IsBeingControlledByCode;
				isAcceptingInput = characterController2D.IsAcceptingInput;
				isGrounded = characterController2D.IsGrounded;
				isJumping = characterController2D.IsJumping;
				isFacingRight = characterController2D.IsFacingRight;
				lastJumped = characterController2D.LastJumped;
				hasChangedDirectionThisFrame = characterController2D.HasChangedDirectionThisFrame;
				debugColliderOverlapsCounter = characterController2D.DebugColliderOverlapsCounter;
				debugColliderOverlaps = characterController2D.DebugColliderOverlaps;
			}
		}
		#endregion

		#region Helper Methods
		#endregion
	}
}