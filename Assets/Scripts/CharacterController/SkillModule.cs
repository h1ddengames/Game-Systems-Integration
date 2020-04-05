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
	public class SkillModule : MonoBehaviour {
		#region Exposed Fields
		#endregion

		#region Private Fields
		private CharacterController2D characterController2D;
		#endregion

		#region Getters/Setters/Constructors
		#endregion

		#region My Methods
		public void NormalAttack() {
			Debug.Log("Using Normal Attack Skill");
			characterController2D.CharacterAnimationModule.AnimateSwordAttack();

		}
		
		public void ShieldBash() {
			Debug.Log("Using Shield Bash Skill");
		}

		// TODO: Apply Knockback
		public void ApplyKnockback(Rigidbody2D rigidbody2D, float horizontalKnockback, float verticalKnockback) {
			Debug.Log("Applying knockback");
			rigidbody2D.AddForce(new Vector2(horizontalKnockback, verticalKnockback), ForceMode2D.Impulse);
		}

		// TODO: Add Dodge Roll Ability
		public void DodgeRoll() {
			Debug.Log("Using Dodge Roll Skill");
		}

		// TODO: Add Dash Ability
		public void Dash() {
			Debug.Log("Using Dash Skill");
		}

		// TODO: Apply Teleport Ability
		public void Teleport() {
			Debug.Log("Using Teleport Skill");
		}

		// TODO: Apply Jetpack Ability
		public void Jetpack() {
			Debug.Log("Using Jetpack Skill");
		}
		#endregion

		#region Unity Methods
		void OnEnable() {
			
		}
		
		void Start() {
			characterController2D = GetComponent<CharacterController2D>();
		}

		void Update() {
			
		}
		
		void OnDisable() {
			
		}
		#endregion

		#region Helper Methods
		#endregion
	}
}