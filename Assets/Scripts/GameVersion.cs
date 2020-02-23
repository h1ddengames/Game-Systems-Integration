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
	public class GameVersion : MonoBehaviour {
		#region Exposed Fields
		[SerializeField] private TextMeshProUGUI text;
		#endregion

		#region Private Fields
		#endregion
		
		#region Getters/Setters
		#endregion

		#region My Methods
		#endregion
		
		#region Unity Methods
		void OnEnable() {
			text.text = GameManager.Instance.GameVersion;
		}
		
		void Start() {
			
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