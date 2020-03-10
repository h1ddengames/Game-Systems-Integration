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
            automatedTaskDefinition.taskToDoBeforeAutomating = task;
            automatedTaskDefinition.taskToDoBeforeName = task.Method.Name;
            return this;
        }

        public DefineTask DoBefore(Action task, string taskName) {
            automatedTaskDefinition.taskToDoBeforeAutomating = task;
            automatedTaskDefinition.taskToDoBeforeName = taskName + " (Inline delegate)";
            return this;
        }

        public DefinePostTask DoTask(Action task) {
            automatedTaskDefinition.taskToAutomate = task;
            automatedTaskDefinition.taskToDoName = task.Method.Name;
            return this;
        }

        public DefinePostTask DoTask(Action task, string taskName) {
            automatedTaskDefinition.taskToAutomate = task;
            automatedTaskDefinition.taskToDoName = taskName;
            return this;
        }

        public DefineDelayAfter DoPost(Action task) {
            automatedTaskDefinition.taskToDoAfterAutomating = task;
            automatedTaskDefinition.taskToDoAfterName = task.Method.Name;
            return this;
        }

        public DefineDelayAfter DoPost(Action task, string taskName) {
            automatedTaskDefinition.taskToDoAfterAutomating = task;
            automatedTaskDefinition.taskToDoAfterName = taskName;
            return this;
        }

        public BuildTask WithDelayAfter(float time) {
            automatedTaskDefinition.delayAfterFinishingAutomation = time;
            return this;
        }

        public AutomatedTask Build() {
            if (string.IsNullOrEmpty(automatedTaskDefinition.taskToDoBeforeName) || automatedTaskDefinition.taskToDoBeforeName.Contains("<Start>")) {
                automatedTaskDefinition.taskToDoBeforeName = "Task name not given";
            }

            if (string.IsNullOrEmpty(automatedTaskDefinition.taskToDoName) || automatedTaskDefinition.taskToDoName.Contains("<Start>")) {
                automatedTaskDefinition.taskToDoName = "Task name not given";
            }

            if (string.IsNullOrEmpty(automatedTaskDefinition.taskToDoAfterName) || automatedTaskDefinition.taskToDoAfterName.Contains("<Start>")) {
                automatedTaskDefinition.taskToDoAfterName = "Task name not given";
            }
            return automatedTaskDefinition;
        }

        //public ITaskDelayBeforeAutomating DoTask(Action task) {
        //    automatedTask.taskToAutomate = task;
        //    return this;
        //}

        //public IDoTaskBeforeStart WithDelayBefore(float delayTime) {
        //    automatedTask.delayBeforeStartingAutomation = delayTime;
        //    return this;
        //}

        //public IDoTaskAfterFinish DoTaskBeforeStarting(Action task) {
        //    automatedTask.taskToDoBeforeAutomating = task;
        //    return this;
        //}

        //public ITaskDelayAfterAutomating DoTaskAfterFinishing(Action task) {
        //    automatedTask.taskToDoAfterAutomating = task;
        //    return this;
        //}

        //public ITaskDefinitionFinished WithDelayAfter(float delayTime) {
        //    automatedTask.delayAfterFinishingAutomation = delayTime;
        //    return this;
        //}

        //public AutomatedTask CreateTask() {
        //    return automatedTask;
        //}
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

    //public interface TaskDefinition {
    //    ITaskDelayBeforeAutomating DoTask(Action task);
    //}

    //public interface ITaskDelayBeforeAutomating {
    //    IDoTaskBeforeStart WithDelayBefore(float delayTime);
    //}

    //public interface IDoTaskBeforeStart {
    //    IDoTaskAfterFinish DoTaskBeforeStarting(Action task);
    //}

    //public interface IDoTaskAfterFinish {
    //    ITaskDelayAfterAutomating DoTaskAfterFinishing(Action task);
    //}

    //public interface ITaskDelayAfterAutomating {
    //    ITaskDefinitionFinished WithDelayAfter(float delayTime);
    //}

    //public interface ITaskDefinitionFinished {
    //    AutomatedTask CreateTask();
    //}
}