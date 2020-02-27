// Created by h1ddengames
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

namespace h1ddengames {
    public class Bouncer : MonoBehaviour {
        #region Exposed Fields
        [SerializeField] private float bounceHeight = 15.0f;
        #endregion

        #region Private Fields
        #endregion

        #region Getters/Setters/Constructors
        public float BounceHeight { get => bounceHeight; set => bounceHeight = value; }
        #endregion

        #region My Methods
        #endregion

        #region Unity Methods
        void OnEnable() {

        }

        void Start() {

        }

        void Update() {

        }

        void OnDisable() {

        }

        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.tag == "Player") {
                GameManager.Instance.Player.GetComponent<CharacterController2D>().Velocity += new Vector2(0, BounceHeight);
            }
        }
        #endregion

        #region Helper Methods
        #endregion
    }
}