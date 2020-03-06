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
	public class TimedEvent : MonoBehaviour {
		#region Exposed Fields
		[SerializeField] private float interval = 2f;
		[SerializeField] private bool paused = false;
		[SerializeField] private UnityEvent events;
		#endregion

		#region Private Fields
		private float timer;
		#endregion

		#region Getters/Setters
		public TimedEvent() {
			this.interval = 2f;
			this.paused = false;
		}

		public TimedEvent(float interval) {
			this.interval = interval;
			this.paused = false;
		}

		public TimedEvent(float interval, UnityEvent events) {
			this.interval = interval;
			this.paused = false;
			this.events = events;
		}


		public TimedEvent(float interval, bool paused, UnityEvent events) {
			this.interval = interval;
			this.paused = paused;
			this.events = events;
		}

		public float Interval { get => interval; set => interval = value; }
		public bool Paused { get => paused; set => paused = value; }
		public UnityEvent Events { get => events; set => events = value; }
		#endregion

		#region My Methods
		public void CallMethod1() {
			Debug.Log("first method");
		}

		public void CallMethod2() {
			Debug.Log("second method");
		}

		public void CallMethod3() {
			Debug.Log("third method");
		}

		public void CallMethod4() {
			Debug.Log("fourth method");
		}

		public void CallMethod5() {
			Debug.Log("fifth method");
		}
		#endregion

		#region Unity Methods
		void OnEnable() {
			
		}
		
		void Start() {
			
		}

		void Update() {
			timer -= Time.deltaTime;

			if(timer < 0) {
				Events?.Invoke();
				timer = Interval;
			}
		}
		
		void OnDisable() {
			
		}
		#endregion

		#region Helper Methods
		#endregion
	}
}