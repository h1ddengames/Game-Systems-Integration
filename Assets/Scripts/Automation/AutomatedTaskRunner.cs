// Created by h1ddengames
// Attributes being used within this class require:
// https://github.com/dbrizov/NaughtyAttributes

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using NaughtyAttributes;
using ReorderableListAttribute = NaughtyAttributes.ReorderableListAttribute;

namespace h1ddengames {
    public class AutomatedTaskRunner : MonoBehaviour {
        #region Exposed Fields
        [BoxGroup("Configuration"), SerializeField] private bool tasksShouldRun = true;
        [BoxGroup("Configuration"), SerializeField] private bool loopTasks = true;
        [BoxGroup("Configuration"), SerializeField] private int amountOfTimesToReunTasks = 2;
        [BoxGroup("Tasks"), ReorderableList, SerializeField] List<AutomatedTask> automatedTasks = new List<AutomatedTask>();
        #endregion

        #region Private Fields
        public bool TasksShouldRun { get => tasksShouldRun; set => tasksShouldRun = value; }
        public bool LoopTasks { get => loopTasks; set => loopTasks = value; }
        public int AmountOfTimesToReunTasks { get => amountOfTimesToReunTasks; set => amountOfTimesToReunTasks = value; }
        public List<AutomatedTask> AutomatedTasks { get => automatedTasks; set => automatedTasks = value; }
        #endregion

        #region Getters/Setters/Constructors
        private int index = 0;
        #endregion

        #region Test Methods
        public void Task() {
            Debug.Log("Called from inside Do Task Method.");
        }

        public void Task1() {
            Debug.Log("Called from inside Do Task1 Method.");
        }

        public void Task2() {
            Debug.Log("Called from inside Do Task2 Method.");
        }

        public void Task3() {
            Debug.Log("Called from inside Do Task3 Method.");
        }

        public void TaskWithCount(int count) {
            Debug.Log("Called from inside Do Task " + count + " Method.");
        }

        public void TaskWithString(string str) {
            Debug.Log("Called from inside" + str + " Method.");
        }
        #endregion

        #region My Methods
        public bool AreAllTasksAreDone() {
            foreach (var task in AutomatedTasks) {
                if(!task.HasInvokedTask || !task.HasFinishedDelayAfterTask) {
                    // Return false if even a single HasInvokedTask 
                    // or HasFinishedDelayAfterTask is false for in any AutomatedTask
                    // contained in automatedTasks list.
                    return false;
                }
            }

            // Return true when HasInvokedTask 
            // and HasFinishedDelayAfterTask is true 
            // for every AutomatedTask.
            return true;
        }

        public void Run(AutomatedTask automatedTask) {
            // Reset index for each time that a Task has been completed.
            if (index >= AutomatedTasks.Count) {
                index = 0;
            }

            // Wait until the Delay Before timer has reached 0.
            if (automatedTask.DelayBeforeTask > 0) {
                automatedTask.DelayBeforeTask -= Time.deltaTime;
                return;
            }

            if(!automatedTask.HasInvokedTask) {
                // Execute the Task
                automatedTask.Task?.Invoke();

                // Mark the Task as completed.
                automatedTask.HasInvokedTask = true;
            }

            // Wait until the Delay After timer has reached 0.
            if (automatedTask.DelayAfterTask > 0) {
                automatedTask.DelayAfterTask -= Time.deltaTime;
                return;
            }

            // Mark that Delay After timer has reached 0.
            automatedTask.HasFinishedDelayAfterTask = true;

            // When all Tasks have finished, remove reference and stop this loop from running.
            if (AreAllTasksAreDone()) {
                if (!LoopTasks) {
                    TasksShouldRun = false;
                    AmountOfTimesToReunTasks = 0;
                } else {
                    AmountOfTimesToReunTasks--;

                    if(AmountOfTimesToReunTasks <= 0) {
                        TasksShouldRun = false;
                        LoopTasks = false;
                    }
                }

                index = 0;
                foreach (var task in AutomatedTasks) {
                    task.Reset();
                }

            } else {
                index++;
            }
        }
        
        public void ChangeTasks(List<AutomatedTask> tasks) {
            TasksShouldRun = false;
            AutomatedTasks.Clear();
            AutomatedTasks = tasks;
        }
        #endregion

        #region Unity Methods
        void OnEnable() {
            foreach(var task in AutomatedTasks) {
                task.SetBackup();
            }
        }

        void Start() {
            
        }

        void Update() {
            if(TasksShouldRun) {
                Run(AutomatedTasks[index]);
            }
        }

        void OnDisable() {

        }
        #endregion

        #region Helper Methods
        #endregion
    }

    [Serializable]
    public class AutomatedTask {
        #region Exposed Fields
        [BoxGroup("Configuration"), SerializeField] private float delayBeforeTask;
        [BoxGroup("Configuration"), SerializeField] private float delayAfterTask;
        [BoxGroup("Configuration"), SerializeField] private bool hasInvokedTask = false;
        [BoxGroup("Configuration"), SerializeField] private bool hasFinishedDelayAfterTask = false;
        [BoxGroup("Configuration"), Space(10), SerializeField] private UnityEvent task;
        #endregion

        #region Private Fields
        private float delayBeforeTaskBackup;
        private float delayAfterTaskBackup;
        #endregion

        #region Getters/Setters/Constructors
        public AutomatedTask(UnityEvent task, float delayBefore, float delayAfter) {
            Task = task;
            DelayBeforeTask = delayBefore;
            DelayAfterTask = delayAfter;
            delayBeforeTaskBackup = DelayBeforeTask;
            delayAfterTaskBackup = DelayAfterTask;
            HasInvokedTask = false;
            HasFinishedDelayAfterTask = false;
        }

        public UnityEvent Task { get => task; set => task = value; }
        public float DelayBeforeTask { get => delayBeforeTask; set => delayBeforeTask = value; }
        public float DelayAfterTask { get => delayAfterTask; set => delayAfterTask = value; }
        public bool HasInvokedTask { get => hasInvokedTask; set => hasInvokedTask = value; }
        public bool HasFinishedDelayAfterTask { get => hasFinishedDelayAfterTask; set => hasFinishedDelayAfterTask = value; }
        #endregion

        #region My Methods
        public void SetBackup() {
            delayBeforeTaskBackup = DelayBeforeTask;
            delayAfterTaskBackup = DelayAfterTask;
        }

        public void Reset() {
            DelayBeforeTask = delayBeforeTaskBackup;
            DelayAfterTask = delayAfterTaskBackup;
            HasInvokedTask = false;
            HasFinishedDelayAfterTask = false;
        }
        #endregion
    }
}