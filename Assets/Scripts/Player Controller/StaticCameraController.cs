// Created by h1ddengames
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace h1ddengames {
    public class StaticCameraController : MonoBehaviour {
        #region Exposed Variables
        [SerializeField] private GameObject player;
        [SerializeField] private Vector3 offset;
        #endregion

        #region Private Variables
        #endregion

        #region Getters/Setters/Constructors
        #endregion

        #region Unity Methods
        void OnEnable() {
            
        }

        void Start() {
            // Calculate and store the offset value by getting the distance between the player's position and camera's position.
            offset = transform.position - player.transform.position;
        }

        void Update() {
            
        }

        void LateUpdate() {
            // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
            transform.position = player.transform.position + offset;
        }

        void OnDisable() {
            
        }
        #endregion

        #region Helper Methods
        #endregion
    }
}
