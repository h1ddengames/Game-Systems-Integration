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
    public class AutomatedTaskRunner : MonoBehaviour {
        #region Exposed Fields
        public List<AutomatedTask> listOfAutomatedTasks = new List<AutomatedTask>();

        public AutomatedTask debugAutomatedTask;
        #endregion

        #region Private Fields
        #endregion

        #region Getters/Setters/Constructors
        #endregion

        #region My Methods
        public void DoBeforeTask() {
            Debug.Log("Called from inside Do Before Task Method.");
        }

        public void DoTask() {
            Debug.Log("Called from inside Do Task Method.");
        }

        public void DoAfterTask() {
            Debug.Log("Called from inside Do After Method.");
        }
        #endregion

        #region Unity Methods
        void OnEnable() {

        }

        void Start() {
            debugAutomatedTask =
                AutomatedTaskBuilder.DefineTask()
                                    .WithDelayBefore(4f)
                                    .DoBefore(DoBeforeTask)
                                    .DoTask(DoTask)
                                    .DoPost(DoAfterTask)
                                    .WithDelayAfter(5f).Build();

            Debug.Log(debugAutomatedTask.ToString());

            debugAutomatedTask =
                AutomatedTaskBuilder.DefineTask()
                                    .WithDelayBefore(4f).DoBefore(delegate { Debug.Log("Delegate call before task"); }, "Before Task Delegate")
                                    .DoTask(delegate { Debug.Log("Delegate call as task"); }, "Random task name")
                                    .DoPost(delegate { Debug.Log("Delegate call after task"); })
                                    .WithDelayAfter(5f).Build();

            Debug.Log(debugAutomatedTask.ToString());
        }

        void Update() {

        }

        void OnDisable() {

        }
        #endregion

        #region Helper Methods
        #endregion
    }
}