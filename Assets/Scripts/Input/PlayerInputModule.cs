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
    public class PlayerInputModule : Singleton<PlayerInputModule> {
        #region Exposed Fields
        [NaughtyAttributes.ReorderableList, SerializeField] private List<InputLibrary> inputLibrary = new List<InputLibrary>();
        #endregion

        #region Private Fields
        #endregion

        #region Getters/Setters/Constructors
        public List<InputLibrary> InputLibrary { get => inputLibrary; set => inputLibrary = value; }
        #endregion

        #region My Methods
        #endregion

        #region Unity Methods
        void OnEnable() {

        }

        void Start() {

        }

        void Update() {
            // Go through every element in inputLibrary and check for keypress.
            foreach(var item in InputLibrary) {

                // Checking if a key has been pressed once.
                if(Input.GetKeyDown(item.Key)) {

                    Debug.Log(item.Key + " has been pressed!");

                    // Checking if a key has been double pressed within a given timeframe.
                    if((Time.time - item.LastTapTime) < item.DoubleTapThreshold) {
                        Debug.Log(item.Key + " has been double tapped!");
                    }

                    item.LastTapTime = Time.time;
                }

                // Checking if a key is being held down.
                if(Input.GetKey(item.Key)) {
                    item.Timer += Time.deltaTime;

                    if(item.Timer > item.HoldThreshold) {
                        Debug.Log(item.Key + " has been held for " + item.HoldThreshold + " seconds!");
                        item.Timer = 0;
                    }
                }

                // Reset hold timer if the key is unpressed.
                if(Input.GetKeyUp(item.Key)) {
                    item.Timer = 0;
                }
            }
        }

        void OnDisable() {

        }
        #endregion

        #region Helper Methods
        #endregion
    }

    [System.Serializable]
    public class InputLibrary {
        #region Configuration
        [BoxedHeader("Configuration"),
        Tooltip("The key that is being listened for."),
        SerializeField]
        private KeyCode key;

        [Tooltip("The length of time required for the key to be held down until the hold event fires."),
        SerializeField]
        private float holdThreshold = 0.5f;

        [Tooltip("The length of time between two single presses. " +
            "If less than this value then it's a double press. " +
            "If greater than this value it is a single press."),
        SerializeField]
        private float doubleTapThreshold = 0.3f;
        #endregion

        #region Quick Information
        [BoxedHeader("Quick Information"), Space(10)]
        [BoxGroup("Quick Information"), SerializeField] private bool showQuickInformation;
        [BoxGroup("Quick Information"), ShowIf("showQuickInformation"), SerializeField] private float timer = 0f;
        [BoxGroup("Quick Information"), ShowIf("showQuickInformation"), SerializeField] private float lastTapTime = 0f;
        #endregion

        #region Getters/Setters/Constructors
        public InputLibrary(KeyCode key, float holdThreshold, float doubleTapThreshold) {
            this.key = key;
            this.holdThreshold = holdThreshold;
            this.doubleTapThreshold = doubleTapThreshold;
        }

        public KeyCode Key { get => key; set => key = value; }
        public float Timer { get => timer; set => timer = value; }
        public float LastTapTime { get => lastTapTime; set => lastTapTime = value; }
        public float HoldThreshold { get => holdThreshold; set => holdThreshold = value; }
        public float DoubleTapThreshold { get => doubleTapThreshold; set => doubleTapThreshold = value; }
        #endregion
    }
}