// Created by h1ddengames
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;

namespace h1ddengames {
    public class AutomatedTask : MonoBehaviour {
        #region Exposed Fields
        [SerializeField] private int id;
        [SerializeField] private UnityEvent task;
        [SerializeField] private float delayBeforeTask;
        [SerializeField] private float delayAfterTask;
        [SerializeField] private bool hasInvokedTask = false;
        [SerializeField] private bool hasFinishedDelayAfterTask = false;
        #endregion

        #region Private Fields
        private float delayBeforeTaskBackup;
        private float delayAfterTaskBackup;
        #endregion

        #region Getters/Setters/Constructors
        public AutomatedTask(int id, UnityEvent task, float delayBefore, float delayAfter) {
            Id = id;
            Task = task;
            DelayBeforeTask = delayBefore;
            DelayAfterTask = delayAfter;
            HasInvokedTask = false;
            HasFinishedDelayAfterTask = false;
        }

        public int Id { get => id; set => id = value; }
        public UnityEvent Task { get => task; set => task = value; }
        public float DelayBeforeTask { get => delayBeforeTask; set => delayBeforeTask = value; }
        public float DelayAfterTask { get => delayAfterTask; set => delayAfterTask = value; }
        public bool HasInvokedTask { get => hasInvokedTask; set => hasInvokedTask = value; }
        public bool HasFinishedDelayAfterTask { get => hasFinishedDelayAfterTask; set => hasFinishedDelayAfterTask = value; }
        #endregion

        #region My Methods
        public void Reset() {
            DelayBeforeTask = delayBeforeTaskBackup;
            DelayAfterTask = delayAfterTaskBackup;
            HasInvokedTask = false;
            HasFinishedDelayAfterTask = false;
        }
        #endregion

        #region Unity Methods
        void OnEnable() {
            delayBeforeTaskBackup = DelayBeforeTask;
            delayAfterTaskBackup = DelayAfterTask;
        }
        #endregion
    }
}