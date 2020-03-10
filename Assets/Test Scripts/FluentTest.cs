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
    public class FluentTest : MonoBehaviour {
        #region Exposed Fields
        public AutomatedTask task;
        #endregion

        #region Private Fields
        #endregion

        #region Getters/Setters/Constructors
        #endregion

        #region My Methods

        //public void DoBeforeTask() {
        //    Debug.Log("Called from inside Do Before Task Method.");
        //}

        //public void DoTask() {
        //    Debug.Log("Called from inside Do Task Method.");
        //}

        //public void DoAfterTask() {
        //    Debug.Log("Called from inside Do After Method.");
        //}
        #endregion

        #region Unity Methods
        void OnEnable() {

        }

        void Start() {
            //task = AutomatedTaskBuilder.Start()
            //       .DoTask(delegate { Debug.Log("Jumping."); })
            //       .WithDelayBefore(5f)
            //       .DoTaskBeforeStarting(delegate { Debug.Log("Checking if grounded."); })
            //       .DoTaskAfterFinishing(delegate { Debug.Log("Incrementing jump count."); })
            //       .WithDelayAfter(3f)
            //       .CreateTask();

            //Debug.Log(task.ToString());

            //task = AutomatedTaskBuilder.Start()
            //        .DoTask(DoTask)
            //        .WithDelayBefore(5f)
            //        .DoTaskBeforeStarting(DoBeforeTask)
            //        .DoTaskAfterFinishing(DoAfterTask)
            //        .WithDelayAfter(3f)
            //        .CreateTask();

            //Debug.Log(task.ToString());
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