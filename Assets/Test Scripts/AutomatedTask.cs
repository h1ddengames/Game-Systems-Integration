// Created by h1ddengames
using System;
using System.Text;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;
using System.Linq.Expressions;

namespace h1ddengames {
    [CreateAssetMenu(fileName = "New Automated Task", menuName = "hiddengames/New Automated Task")]
    public class AutomatedTask : ScriptableObject {
        #region Exposed Fields
        public float delayBeforeStartingAutomation;
        public Action taskToDoBeforeAutomating;
        public Action taskToAutomate;
        public Action taskToDoAfterAutomating;
        public float delayAfterFinishingAutomation;

        public string taskToDoBeforeName;
        public string taskToDoName;
        public string taskToDoAfterName;
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

        }

        void Update() {

        }

        void OnDisable() {

        }
        #endregion

        #region Helper Methods
        public override string ToString() {
            StringBuilder stringBuilder = new StringBuilder();

            stringBuilder.Append("Delay before starting automation: ")
                        .Append(delayBeforeStartingAutomation);

            if (taskToDoBeforeAutomating is Delegate) {
                stringBuilder.Append(" seconds.\r\nTask to do before starting automation: ")
                            .Append(taskToDoBeforeName);
            } else {
                stringBuilder.Append(" seconds.\r\nTask to do before starting automation: ")
                            .Append(taskToDoBeforeAutomating.Method.Name);
            }

            if (taskToAutomate is Delegate) {
                stringBuilder.Append("\r\nTask to do: ")
                            .Append(taskToDoName);

            } else {
                stringBuilder.Append("\r\nTask to do: ")
                            .Append(taskToAutomate.Method.Name);
            }

            if (taskToDoAfterAutomating is Delegate) {
                stringBuilder.Append(".\r\nTask to do after finishing automation: ")
                            .Append(taskToDoAfterName);
            } else {
                stringBuilder.Append(".\r\nTask to do after finishing automation: ")
                            .Append(taskToDoAfterAutomating.Method.Name);
            }

            stringBuilder.Append(".\r\nDelay after task is finished: ")
                            .Append(delayAfterFinishingAutomation)
                            .Append(" seconds.");

            return stringBuilder.ToString();
        }
        #endregion
    }
}