// Created by h1ddengames
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using CodeMonkey.Utils;

namespace h1ddengames {
	public class FunctionsTest : MonoBehaviour {
		#region Exposed Fields
		int jumpCount = 0;
		int leftClickCount = 0;
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
			UtilsClass.CreateKeyCodeAction(KeyCode.Space, () => {
				jumpCount++;
				//Debug.Log(KeyCode.Space + " was pressed " + i + " times.");
				Debug.Log(String.Concat(KeyCode.Space, " was pressed ", jumpCount, " times."));
			});

			UtilsClass.CreateMouseClickAction(0, () => {
				leftClickCount++;
				Debug.Log(String.Concat("Mouse key 0 was pressed ", leftClickCount, " times."));
			});

			FunctionTimer.Create(() => { Debug.Log("WHY THO"); }, 3f, "WHY");
			FunctionTimer.StopFirstTimerWithName("WHY");
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