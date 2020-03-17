// Created by h1ddengames
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;
using ReorderableListAttribute = NaughtyAttributes.ReorderableListAttribute;

namespace h1ddengames {
    public class AutomatedTaskRunner : MonoBehaviour {
        #region Exposed Fields
        [ReorderableList, SerializeField] List<AutomatedTask> automatedTasks = new List<AutomatedTask>();

        [SerializeField] private bool tasksShouldRun = true;
        [SerializeField] private bool loopTasks = true;
        [SerializeField] private int amountOfTimesToReunTasks = 2;

        public int index = 0;
        #endregion

        #region Private Fields
        public List<AutomatedTask> AutomatedTasks { get => automatedTasks; set => automatedTasks = value; }
        public bool TasksShouldRun { get => tasksShouldRun; set => tasksShouldRun = value; }
        public bool LoopTasks { get => loopTasks; set => loopTasks = value; }
        public int AmountOfTimesToReunTasks { get => amountOfTimesToReunTasks; set => amountOfTimesToReunTasks = value; }
        #endregion

        #region Getters/Setters/Constructors
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
}