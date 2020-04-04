// Created by h1ddengames
// Attributes being used within this class require:
// https://github.com/dbrizov/NaughtyAttributes

using System;
using System.Collections.Generic;
using UnityEngine;
using NaughtyAttributes;
using ReorderableListAttribute = NaughtyAttributes.ReorderableListAttribute;

namespace h1ddengames {
    public class AutomatedMoveModule : MonoBehaviour {
        #region Exposed Fields
        [SerializeField] private bool loopThroughAllWaypoints;
        [ReorderableList, SerializeField] private List<WayPoint> listOfWayPoints;
        #endregion

        #region Private Fields
        private CharacterController2D characterController2D;
        private bool waitingForDelayTimer = false;
        private int index = 0;
        private float beforeTempTimer = 0;
        private float afterTempTimer = 0;
        private float defaultMoveSpeed;
        #endregion

        #region Getters/Setters/Constructors
        #endregion

        #region Automation Methods
        public void MoveToLocation(Transform desiredTransform, float moveSpeed) {
            MoveToLocation(desiredTransform.position, moveSpeed);
        }

        public void MoveToLocation(GameObject objectToMoveTo, float moveSpeed) {
            MoveToLocation(objectToMoveTo.transform.position, moveSpeed);
        }

        public void MoveToLocation(Vector2 desiredPosition, float moveSpeed) {
            if(!waitingForDelayTimer) {
                characterController2D.transform.position = Vector2.MoveTowards(characterController2D.transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            }
        }

        public void Automate() {
            if(listOfWayPoints.Count == 0) {
                characterController2D.IsBeingControlledByCode = false;
                characterController2D.IsAcceptingInput = true;
                characterController2D.PlayerInputModule.IsAcceptingInput = true;
                return;
            }

            // Avoid Out of Bounds Exception.
            if(index < listOfWayPoints.Count) {
                // Check if the player is already at the waypoint.
                if(listOfWayPoints[index].HasArrived) {
                    index++;
                    afterTempTimer = 0;
                    afterTempTimer = 0;
                    return;
                }

                // Wait until delay before moving to waypoint is 0.
                if(beforeTempTimer < listOfWayPoints[index].DelayBeforeMovingToWaypoint) {
                    beforeTempTimer += Time.deltaTime;
                    return;
                }

                // Until the player hasn't arrived at the waypoint, keep moving the player.
                if(!listOfWayPoints[index].HasArrived) {
                    // Check to see if the player is close enough to the waypoint.
                    if(Vector2.Distance(characterController2D.transform.position, listOfWayPoints[index].Location) < 0.2f) {
                        // Stop the player and wait until delay after reaching waypoint is 0.
                        characterController2D.CharacterRigidbody2D.velocity = Vector2.zero;
                        //listOfWayPoints[index].DelayAfterReachingWaypoint -= Time.deltaTime;
                        if(afterTempTimer < listOfWayPoints[index].DelayAfterReachingWaypoint) {
                            afterTempTimer += Time.deltaTime;
                        } else {
                            listOfWayPoints[index].HasArrived = true;
                        }
                    } else {
                        MoveToLocation(listOfWayPoints[index].Location, listOfWayPoints[index].MoveSpeedToWaypoint);
                    }
                }
            } else {
                // If index is equal to or greater than the count of all waypoints, reset has arrived based on 
                // characterController2D preference.

                // TODO: Looping through all waypoints will not work currently. The above loop directly changes the
                // delay times in each waypoint object. Need to create a temp timer that is only set once at the
                // beginning to the delay time then subtracted by Time.deltaTime first set.
                if(loopThroughAllWaypoints) {
                    ResetWaypoints();
                    index = 0;
                } else {
                    characterController2D.IsBeingControlledByCode = false;
                    characterController2D.IsAcceptingInput = true;
                    characterController2D.PlayerInputModule.IsAcceptingInput = true;
                    ResetWaypoints();
                }
            }
        }
        #endregion

        #region My Methods
        public void ResetWaypoints() {
            foreach(var item in listOfWayPoints) {
                item.HasArrived = false;
            }
        }
        #endregion

        #region Unity Methods
        void OnEnable() {

        }

        void Start() {
            characterController2D = GetComponent<CharacterController2D>();
            defaultMoveSpeed = characterController2D.CharacterMoveSpeed;
        }

        void Update() {
            if(characterController2D.IsBeingControlledByCode) {
                characterController2D.IsAcceptingInput = false;
                characterController2D.PlayerInputModule.IsAcceptingInput = false;
                Automate();
            }
        }

        void OnDisable() {

        }
        #endregion

        #region Helper Methods
        #endregion
    }

    [Serializable]
    public class WayPoint {
        #region Exposed Fields
        [SerializeField] private Vector2 location;
        [SerializeField] private float moveSpeedToWaypoint;
        [SerializeField] private float delayBeforeMovingToWaypoint;
        [SerializeField] private float delayAfterReachingWaypoint;
        [SerializeField] private bool hasArrived;
        #endregion

        #region Getters/Setters/Constructors
        public WayPoint(Vector2 location, float moveSpeedToWaypoint, float delayBeforeMovingToWaypoint, float delayToNextWaypoint, bool hasArrived) {
            Location = location;
            MoveSpeedToWaypoint = moveSpeedToWaypoint;
            DelayBeforeMovingToWaypoint = delayBeforeMovingToWaypoint;
            DelayAfterReachingWaypoint = delayToNextWaypoint;
            HasArrived = hasArrived;
        }

        public Vector2 Location { get => location; set => location = value; }
        public float MoveSpeedToWaypoint { get => moveSpeedToWaypoint; set => moveSpeedToWaypoint = value; }
        public float DelayBeforeMovingToWaypoint { get => delayBeforeMovingToWaypoint; set => delayBeforeMovingToWaypoint = value; }
        public float DelayAfterReachingWaypoint { get => delayAfterReachingWaypoint; set => delayAfterReachingWaypoint = value; }
        public bool HasArrived { get => hasArrived; set => hasArrived = value; }
        #endregion
    }
}