// Created by h1ddengames
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

namespace h1ddengames {
	public class PropertyChange : MonoBehaviour, INotifyPropertyChanged {
		#region Exposed Fields
		[SerializeField] private string name;
		[SerializeField] private int id;
		#endregion

		#region Private Fields
		[SerializeField] private string oldName;
		[SerializeField] private int oldId;
        #endregion

        #region Getters/Setters/Constructors
        public string Name { get => name; set => SetField(ref name, value); }
		public int Id { get => id; set => SetField(ref id, value); }
		#endregion

		#region My Methods
		// Methods that want to listen to the PropertyChanged event need to implement the
		// following parameters: object sender, PropertyChangedEventArgs.
		public void DoSomething(object sender, PropertyChangedEventArgs args) {
			// Sender is the gameobject whose script had it's value changed.
			// PropertyChangedEventArgs is the name of the property whose value has changed.
			Debug.Log(args.PropertyName + " " + "has changed in: " + sender);
		}
		#endregion

		#region Unity Methods
		void Start() {
			//PropertyChanged += DoSomething;
			Name = "f";
			Name = "n";
			Id = 3;
			Id = 4;
			Name = "na";
			Id = 6;
			//PropertyChanged -= DoSomething;
		}
		#endregion

		#region Helper Methods
		// Boiler-plate Code
		// The property that every class listening to this class must subscribe to.
		public event PropertyChangedEventHandler PropertyChanged = delegate { };

		// Invokes the event and sends the gameObject this script is attached to
		// along with the property that has changed
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		   => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));

		// Changes the value on the property only if the new value is different from the old value.
		protected bool SetField<T>(ref T field, T value, [CallerMemberName] string propertyName = null) {
			if (EqualityComparer<T>.Default.Equals(field, value)) return false;
			//SetOldValues(field, value, propertyName);
			field = value;
			OnPropertyChanged(propertyName);
			return true;
		}

		private void SetOldValues<T>(T field, T value, string propertyName) {
			Debug.Log(propertyName + " " + field + " " + value);
			// Replace with a dictionary to map propertyName => OldPropertyName
			// Then update OldPropertyName with ref.
			if(propertyName == "Id") {
				oldId = (int) (object) field;
			} else if(propertyName == "Name") {
				oldName = (string) (object) field;
			}
		}
		#endregion
	}
}