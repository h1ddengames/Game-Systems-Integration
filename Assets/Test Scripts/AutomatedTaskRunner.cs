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
        [NaughtyAttributes.ReorderableList] public List<AutomatedTask> listOfAutomatedTasks = new List<AutomatedTask>();

        public AutomatedTask debugAutomatedTask;

        public float debugDelayBeforeStartingAutomation;
        public Action debugTaskToDoBeforeAutomating;
        public Action debugTaskToAutomate;
        public Action debugTaskToDoAfterAutomating;
        public float debugDelayAfterFinishingAutomation;

        public string debugTaskToDoBeforeName;
        public string debugTaskToDoName;
        public string debugTaskToDoAfterName;
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

        public void SetDebugVariables(AutomatedTask debugAutomatedTask) {
            debugDelayBeforeStartingAutomation = debugAutomatedTask.delayBeforeStartingAutomation;
            debugTaskToDoBeforeAutomating = debugAutomatedTask.taskToDoBeforeAutomating;
            debugTaskToAutomate = debugAutomatedTask.taskToAutomate;
            debugTaskToDoAfterAutomating = debugAutomatedTask.taskToDoAfterAutomating;
            debugDelayAfterFinishingAutomation = debugAutomatedTask.delayAfterFinishingAutomation;

            debugTaskToDoBeforeName = debugAutomatedTask.taskToDoBeforeName;
            debugTaskToDoName = debugAutomatedTask.taskToDoName;
            debugTaskToDoAfterName = debugAutomatedTask.taskToDoAfterName;
        }

        [NaughtyAttributes.Button("Clear Debug Variables")]
        public void ClearDebugVariables() {
            debugDelayBeforeStartingAutomation = 0;
            debugTaskToDoBeforeAutomating = null;
            debugTaskToAutomate = null;
            debugTaskToDoAfterAutomating = null;
            debugDelayAfterFinishingAutomation = 0;

            debugTaskToDoBeforeName = "";
            debugTaskToDoName = "";
            debugTaskToDoAfterName = "";
        }

        [NaughtyAttributes.Button("Test Method 1")]
        void TestMethod1() {
            debugAutomatedTask =
                AutomatedTaskBuilder.DefineTask()
                                    .WithDelayBefore(4f)
                                    .DoBefore(DoBeforeTask, "Task")
                                    .DoTask(DoTask)
                                    .DoPost(DoAfterTask)
                                    .WithDelayAfter(5f).Build();

            SetDebugVariables(debugAutomatedTask);

            Debug.Log(debugAutomatedTask.ToString());
        }        
        
        [NaughtyAttributes.Button("Test Method 2")]
        void TestMethod2() {

            debugAutomatedTask =
                AutomatedTaskBuilder.DefineTask()
                                    .WithDelayBefore(4f).DoBefore(delegate { Debug.Log("Delegate call before task"); }, "Before Task Delegate")
                                    .DoTask(delegate { Debug.Log("Delegate call as task"); }, "Random task name")
                                    .DoPost(delegate { Debug.Log("Delegate call after task"); })
                                    .WithDelayAfter(5f).Build();

            SetDebugVariables(debugAutomatedTask);

            Debug.Log(debugAutomatedTask.ToString());
        }
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
        #endregion
    }
}