// Created by h1ddengames

using System;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

namespace h1ddengames {
	[Serializable]
	public class AnimationModule : MonoBehaviour {
		#region Exposed Fields
		#endregion

		#region Private Fields
		private Animator characterAnimator;
		private Transform characterTransform;
		private List<string> listOfAnimationClips = new List<string>();
		#endregion

		#region Getters/Setters/Constructors

		#endregion

		#region Animation Methods
		public void AnimateCharacterFlip() {
			Vector2 scale = characterTransform.localScale;
			scale.x *= -1;
			characterTransform.localScale = scale;
		}

		// Uses BlendTree for animating between movement and idle animations.
		// Does not require EndAnimateMove for the above reason.
		public void AnimateMove(float velocity) {
			characterAnimator.SetFloat("velocity", velocity);
		}

		public void AnimateJump() {
			if(listOfAnimationClips.Contains("Player Jump Animation"))
				characterAnimator.Play("Player Jump Animation");
		}

		public void EndJump() {
			characterAnimator.StopPlayback();
		}

		public void AnimateCrouch() {

		}

		public void EndCrouch() {

		}

		public void AnimateKnockback() {

		}

		public void AnimateSwordAttack() {
			if(listOfAnimationClips.Contains("Player Sword Attack Animation"))
				characterAnimator.Play("Player Sword Attack Animation");
		}

		public void AnimateShieldBash() {
			if(listOfAnimationClips.Contains("Player Shield Bash Animation"))
				characterAnimator.Play("Player Shield Bash Animation");
		}
		#endregion

		#region My Methods
		public void PrintAllAnimationClipNames() {
			listOfAnimationClips.ForEach(item => Debug.Log(item));
		}
		#endregion

		void Start() {
			characterTransform = GetComponent<Transform>();
			characterAnimator = GetComponent<Animator>();
			characterAnimator.runtimeAnimatorController.animationClips
				.ToList()
				.ForEach(c => listOfAnimationClips.Add(c.name));
		}

		#region Helper Methods
		#endregion
	}
}