using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using SFB;
using DG.Tweening;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

namespace h1ddengames {
	public class AttackController : MonoBehaviour {
		#region Exposed Fields
		Animator playerAnimator;
		#endregion
		
		#region Private Fields
		#endregion
		
		#region Getters/Setters/Constructors
		#endregion
		
		#region My Methods
		#endregion
		
		#region Unity Methods
		void OnEnable() {
			
		}
		
		void Start() {
			playerAnimator = GetComponent<Animator>();
		}

		void Update() {
			if(Input.GetKeyDown(KeyCode.T) || Input.GetKey(KeyCode.T)) {
				playerAnimator.Play("Player Sword Attack Animation");
			}

			if (Input.GetKeyDown(KeyCode.G) || Input.GetKey(KeyCode.G)) {
				playerAnimator.Play("Player Shield Bash Animation");
			}
		}
		
		void OnDisable() {
			
		}
		#endregion
		
		#region Helper Methods
		#endregion
	}
}