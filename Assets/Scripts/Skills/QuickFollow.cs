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
    public class QuickFollow : MonoBehaviour {
        #region Exposed Fields
        [SerializeField] private Transform target;

        [SerializeField] private bool shouldFollowTarget = true;

        // Setting speed lower than the target's movespeed gives a delayed follow effect.
        [SerializeField] private float speed = 5f;

        // How far should the target be before this gameobject needs to teleport?
        [SerializeField] private float teleportDistance = 5f;

        // How far away can the target be from this gameObject before movement stops?
        [SerializeField] private float followDistance = 0.5f;

        // Simulates the target being at a different x position.
        [SerializeField] private float xOffset = 0f;

        // Simulates the target being at a different y position.
        [SerializeField] private float yOffset = 1.25f;
        #endregion

        #region Private Fields
        #endregion

        #region Getters/Setters/Constructors
        public Transform Target { get => target; set => target = value; }
        public bool ShouldFollowTarget { get => shouldFollowTarget; set => shouldFollowTarget = value; }
        public float Speed { get => speed; set => speed = value; }
        public float FollowDistance { get => followDistance; set => followDistance = value; }
        public float XOffset { get => xOffset; set => xOffset = value; }
        public float YOffset { get => yOffset; set => yOffset = value; }
        #endregion

        #region My Methods

        public void Flip() {
            teleportDistance *= -1;
            followDistance *= -1;

            Vector3 flipped = transform.localScale;
            flipped.x *= -1f;
            transform.localScale = flipped;
        }
        #endregion

        #region Unity Methods
        void OnEnable() {

        }

        void Start() {

        }

        void FixedUpdate() {
            // Slowly move towards the target when the target is close by.
            if(ShouldFollowTarget && Mathf.Abs(Vector2.Distance(transform.position, target.position)) > FollowDistance) {
                transform.position =
                    Vector2.MoveTowards(transform.position, new Vector2(Target.position.x + XOffset, Target.position.y + YOffset), Speed * Time.deltaTime);
            }
            
            // Directly teleport to the offset location when the target is too far away.
            if (ShouldFollowTarget && Mathf.Abs(Vector2.Distance(transform.position, target.position)) > teleportDistance) {
                transform.position = new Vector2(Target.position.x + XOffset, Target.position.y + YOffset);
            }
        }

        void OnDisable() {

        }
        #endregion

        #region Helper Methods
        #endregion
    }
}