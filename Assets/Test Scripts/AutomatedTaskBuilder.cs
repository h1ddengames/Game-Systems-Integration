// Created by h1ddengames
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using SFB;
using NaughtyAttributes;
using RotaryHeart.Lib.SerializableDictionary;
using System.Text.RegularExpressions;

namespace h1ddengames {
    [Serializable]
    public class AutomatedTaskBuilder : DefineDelayBefore, DefinePreTask, DefineTask, DefinePostTask, DefineDelayAfter, BuildTask {
        #region Exposed Fields
        protected AutomatedTask automatedTaskDefinition = ScriptableObject.CreateInstance<AutomatedTask>();
        #endregion

        #region Private Fields
        #endregion

        #region Getters/Setters/Constructors
        private AutomatedTaskBuilder() { }
        #endregion

        #region My Methods
        public static AutomatedTaskBuilder DefineTask() {
            return new AutomatedTaskBuilder();
        }

        public DefinePreTask WithDelayBefore(float time) {
            automatedTaskDefinition.delayBeforeStartingAutomation = time;
            return this;
        }

        public DefineTask DoBefore(Action task) {
            return DoBefore(task, task.Method.Name);
        }

        public DefineTask DoBefore(Action task, string taskName) {
            automatedTaskDefinition.taskToDoBeforeAutomating = task;

            if (!string.IsNullOrEmpty(taskName)) {
                if (Regex.IsMatch(taskName, "<*>")) {
                    automatedTaskDefinition.taskToDoBeforeName = "Inline delegate where task name was not given";
                } else {
                    automatedTaskDefinition.taskToDoBeforeName = taskName;
                }
            }

            return this;
        }

        public DefinePostTask DoTask(Action task) {
            return DoTask(task, task.Method.Name);
        }

        public DefinePostTask DoTask(Action task, string taskName) {
            if (!string.IsNullOrEmpty(taskName)) {
                if (Regex.IsMatch(taskName, "<*>")) {
                    automatedTaskDefinition.taskToDoName = "Inline delegate where task name was not given";
                } else {
                    automatedTaskDefinition.taskToDoName = taskName;
                }
            }

            return this;
        }

        public DefineDelayAfter DoPost(Action task) {
            return DoPost(task, task.Method.Name);
        }

        public DefineDelayAfter DoPost(Action task, string taskName) {
            if (!string.IsNullOrEmpty(taskName)) {
                if (Regex.IsMatch(taskName, "<*>")) {
                    automatedTaskDefinition.taskToDoAfterName = "Inline delegate where task name was not given";
                } else {
                    automatedTaskDefinition.taskToDoAfterName = taskName;
                }
            }

            return this;
        }

        public BuildTask WithDelayAfter(float time) {
            automatedTaskDefinition.delayAfterFinishingAutomation = time;
            return this;
        }

        public AutomatedTask Build() {
            return automatedTaskDefinition;
        }
        #endregion

        #region Helper Methods
        #endregion
    }

    public interface DefineDelayBefore {
        DefinePreTask WithDelayBefore(float time);
    }

    public interface DefinePreTask {
        DefineTask DoBefore(Action task);
        DefineTask DoBefore(Action task, string taskName);
    }

    public interface DefineTask {
        DefinePostTask DoTask(Action task);
        DefinePostTask DoTask(Action task, string taskName);
    }

    public interface DefinePostTask {
        DefineDelayAfter DoPost(Action task);
        DefineDelayAfter DoPost(Action task, string taskName);
    }

    public interface DefineDelayAfter {
        BuildTask WithDelayAfter(float time);
    }

    public interface BuildTask {
        AutomatedTask Build();
    }
}