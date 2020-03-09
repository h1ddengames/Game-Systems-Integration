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
    public class AutomatedMoveModule {
        #region Exposed Fields
        
        #endregion

        #region Private Fields
        private bool waitingForDelayTimer = false;
        private int index = 0;
        private float tempTimer = 0;
        private CharacterController2D characterController2D;
        private float defaultMoveSpeed;
        #endregion

        #region Getters/Setters/Constructors
        public AutomatedMoveModule(CharacterController2D characterController2D) {
            this.characterController2D = characterController2D;
            defaultMoveSpeed = this.characterController2D.CharacterSpeed;
        }
        #endregion

        #region Automation Methods
        public void MoveToLocation(Transform desiredTransform, float moveSpeed) {
            MoveToLocation(desiredTransform.position, moveSpeed);
        }

        public void MoveToLocation(GameObject objectToMoveTo, float moveSpeed) {
            MoveToLocation(objectToMoveTo.transform.position, moveSpeed);
        }

        public void MoveToLocation(Vector2 desiredPosition, float moveSpeed) {
            if (!waitingForDelayTimer) {
                characterController2D.transform.position = Vector2.MoveTowards(characterController2D.transform.position, desiredPosition, moveSpeed * Time.deltaTime);
            }
        }

        public void Automate() {
            if (characterController2D.ListOfWayPoints.Count == 0) {
                characterController2D.IsBeingControlledByCode = false;
                return;
            }

            // Avoid Out of Bounds Exception.
            if(index < characterController2D.ListOfWayPoints.Count) {
                // Check if the player is already at the waypoint.
                if(characterController2D.ListOfWayPoints[index].HasArrived) {
                    index++;
                    return;
                }

                // Wait until delay before moving to waypoint is 0.
                if (characterController2D.ListOfWayPoints[index].DelayBeforeMovingToWaypoint > 0) {
                    characterController2D.ListOfWayPoints[index].DelayBeforeMovingToWaypoint -= Time.deltaTime;
                    return;
                } 
                
                // Until the player hasn't arrived at the waypoint, keep moving the player.
                if (!characterController2D.ListOfWayPoints[index].HasArrived) {

                    // Check to see if the player is close enough to the waypoint.
                    if (Vector2.Distance(characterController2D.transform.position, characterController2D.ListOfWayPoints[index].Location) < 0.2f) {
                        // Stop the player and wait until delay after reaching waypoint is 0.
                        characterController2D.Velocity = Vector2.zero;
                        characterController2D.ListOfWayPoints[index].DelayAfterReachingWaypoint -= Time.deltaTime;

                        if (characterController2D.ListOfWayPoints[index].DelayAfterReachingWaypoint <= 0) {
                            characterController2D.ListOfWayPoints[index].HasArrived = true;
                        }
                    } else {
                        MoveToLocation(characterController2D.ListOfWayPoints[index].Location, characterController2D.ListOfWayPoints[index].MoveSpeedToWaypoint);
                    }
                }
            } else {
                // If index is equal to or greater than the count of all waypoints, reset has arrived based on 
                // characterController2D preference.

                // TODO: Looping through all waypoints will not work currently. The above loop directly changes the
                // delay times in each waypoint object. Need to create a temp timer that is only set once at the
                // beginning to the delay time then subtracted by Time.deltaTime first set.
                if (characterController2D.LoopThroughAllWaypoints) {
                    for (int i = 0; i < characterController2D.ListOfWayPoints.Count; i++) {
                        characterController2D.ListOfWayPoints[i].HasArrived = false;
                        index = 0;
                    }
                } else {
                    characterController2D.IsBeingControlledByCode = false;
                }
            }
        }
        #endregion

        #region My Methods
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
            this.Location = location;
            this.MoveSpeedToWaypoint = moveSpeedToWaypoint;
            this.DelayBeforeMovingToWaypoint = delayBeforeMovingToWaypoint;
            this.DelayAfterReachingWaypoint = delayToNextWaypoint;
            this.HasArrived = hasArrived;
        }

        public Vector2 Location { get => location; set => location = value; }
        public float MoveSpeedToWaypoint { get => moveSpeedToWaypoint; set => moveSpeedToWaypoint = value; }
        public float DelayBeforeMovingToWaypoint { get => delayBeforeMovingToWaypoint; set => delayBeforeMovingToWaypoint = value; }
        public float DelayAfterReachingWaypoint { get => delayAfterReachingWaypoint; set => delayAfterReachingWaypoint = value; }
        public bool HasArrived { get => hasArrived; set => hasArrived = value; }
        #endregion
    }
}