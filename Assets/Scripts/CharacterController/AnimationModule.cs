// Created by h1ddengames
using System;
using System.Linq;
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
	[Serializable]
	public class AnimationModule {
		#region Exposed Fields
		#endregion

		#region Private Fields
		private Animator characterAnimator;
		private List<string> listOfAnimationClips = new List<string>();
		#endregion

		#region Getters/Setters/Constructors
		public AnimationModule(Animator characterAnimator) {
			this.characterAnimator = characterAnimator;
			this.characterAnimator.runtimeAnimatorController.animationClips
				.ToList()
				.ForEach(c => listOfAnimationClips.Add(c.name));
		}
		#endregion

		#region Animation Methods
		// Uses BlendTree for animating between movement and idle animations.
		// Does not require EndAnimateMove for the above reason.
		public void AnimateMove() {

		}

		public void AnimateJump() {

		}

		public void EndJump() {

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

		#region Helper Methods
		#endregion
	}
}