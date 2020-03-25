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
	public class Console : MonoBehaviour {
		#region Exposed Fields
		[SerializeField] private TMP_InputField inputField;
		[NaughtyAttributes.ReorderableList, SerializeField] private List<ConsoleCommand> consoleCommands = new List<ConsoleCommand>();
		#endregion

		#region Private Fields
		UnityAction<string> executeAction;
		UnityAction<string> clearTextAction;
		string[] currentParameters;
		#endregion

		#region Getters/Setters/Constructors
		public TMP_InputField InputField { get => inputField; set => inputField = value; }
		public List<ConsoleCommand> ConsoleCommands { get => consoleCommands; set => consoleCommands = value; }
		#endregion

		#region My Methods
		public void SingleParameterExample() {
			if(currentParameters != null && currentParameters.Length >= 1) {
				Debug.Log($"Increased by {currentParameters[0]}x!");
			} else {
				Debug.Log("Not enough parameters for this command!");
			}
		}

		public void DoubleParameterExample() {
			if(currentParameters != null && currentParameters.Length >= 2) {
				Debug.Log($"Increased {currentParameters[0]} HP by {currentParameters[1]}x!");
			} else {
				Debug.Log("Not enough parameters for this command!");
			}
		}

		public void MultiParameterExample() {
			if(currentParameters != null && currentParameters.Length >= 3) {
				Debug.Log($"Will decrease {currentParameters[0]} HP by {currentParameters[1]}x in {currentParameters[2]} seconds!");
			} else {
				Debug.Log("Not enough parameters for this command!");
			}
		}
		#endregion

		#region My Methods
		// Used by the TMP_InputField GameObject's On End Edit event. 
		public void ExecuteCommand() {
			// Split will split the entire string that the player gives into the command part (input[0])
			// and the parameters (input[1...n])
			string[] input = InputField.text.Split();

			foreach(var item in ConsoleCommands) {
				// Match command name to the command part of the player's input.
				if(input[0] == item.CommandName) {
					// The amount of letters entered by the player can only 
					// be equal to or greater than the length of this command name to enter this block.
					if(InputField.text.Length == item.CommandName.Length) {
						// Since only the amount of letters matches the command name there's nothing to remove.
						InputField.text = InputField.text.Remove(0, item.CommandName.Length);
					} else {
						// Since the amount of letters is greater than the command name, remove the white space after the command.
						InputField.text = InputField.text.Remove(0, item.CommandName.Length + 1);
					}
					
					if(!string.IsNullOrEmpty(InputField.text)) {
						currentParameters = InputField.text.Split();
					} else {
						currentParameters = null;
					}

					// Invoke the command as long as the UnityEvent is not empty.
					item.CommandAction?.Invoke();

					// No need to continue if the command has been found and invoked.
					return;
				}
			}
		}

		// Used by the TMP_InputField GameObject's On End Edit event. 
		public void ClearText() {
			InputField.text = "";
			InputField.ActivateInputField();
		}
		#endregion

		#region Unity Methods
		void OnEnable() {
			EnableListeners();
		}

		void OnDisable() {
			DisableListeners();
		}
		#endregion

		#region Helper Methods
		public void EnableListeners() {
			executeAction = (_) => ExecuteCommand();
			clearTextAction = (_) => ClearText();

			if(InputField != null) {
				InputField.onEndEdit.AddListener(executeAction);
				InputField.onEndEdit.AddListener(clearTextAction);
			}
		}

		public void DisableListeners() {
			if(InputField != null) {
				InputField.onEndEdit.RemoveListener(executeAction);
				InputField.onEndEdit.RemoveListener(clearTextAction);
			}

			executeAction = null;
			clearTextAction = null;
		}
		#endregion
	}

	[Serializable]
	public class ConsoleCommand {
		#region Exposed Fields
		[SerializeField] private string commandName;
		[SerializeField] private UnityEvent commandAction;
		#endregion

		#region Getters/Setters/Constructors
		public string CommandName { get => commandName; set => commandName = value; }
		public UnityEvent CommandAction { get => commandAction; set => commandAction = value; }
		#endregion
	}
}