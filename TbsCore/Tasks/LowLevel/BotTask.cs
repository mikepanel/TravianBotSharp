﻿using OpenQA.Selenium.Chrome;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using TravBotSharp.Files.Models.AccModels;

namespace TravBotSharp.Files.Tasks
{
    public abstract class BotTask
    {
        /// <summary>
        /// If of the village to execute the task. If null, don't change village before executing the task
        /// </summary>
        public Village vill { get; set; }

        /// <summary>
        /// Stage in which task is currently in.
        /// </summary>
        public TaskStage Stage { get; set; }

        /// <summary>
        /// When to execute the task
        /// </summary>
        public DateTime ExecuteAt { get; set; }

        /// <summary>
        /// When we want to re-execute a continuous task (build/demolish building, improve unit etc.)
        /// </summary>
        public DateTime? NextExecute { get; set; }

        /// <summary>
        /// After each execution, if return bool is true, there has to be a new browser load event. Bot will wait for that event.
        /// If there is no browser load event (just parsing some data eg. GetMapSize, return false and browser will navigate to dorf1/2.
        /// </summary>
        /// <param name="htmlDoc">The html doc of the webpage</param>
        /// <param name="wb">Selenium driver</param>
        /// <param name="acc">Account</param>
        /// <returns>TaskRes</returns>
        public abstract Task<TaskRes> Execute(HtmlAgilityPack.HtmlDocument htmlDoc, ChromeDriver wb, Account acc); //execute the task

        /// <summary>
        /// Counts how long this task was being executed. Used for timeouts (after 20sec, re-execute the task)
        /// </summary>
        public int DurationCounter { get; set; }

        /// <summary>
        /// Counts how many times we retried executing the task. After 3rd try, stop retrying. Something is clearly wrong
        /// Used in TaskExecutor and TaskTimer
        /// </summary>
        public int RetryCounter { get; set; }

        /// <summary>
        /// How high of a priority does this task have.
        /// Tasks like attacking and deffending (waves) have highest priority and should as such be executed first
        /// </summary>
        public TaskPriority Priority { get; set; }

        /// <summary>
        /// List of actions to take after execution, when browser loads new page, check for some data (html)
        /// </summary>
        public List<Action<HtmlAgilityPack.HtmlDocument, Account>> PostTaskCheck { get; set; }

        public enum TaskRes
        {
            Executed,
            Retry
        }
        public enum TaskStage
        {
            Start,
            Executing,
        }
        public enum TaskPriority
        {
            Low = 0,
            Medium,
            High
        }
        public string ErrorMessage { get; set; }
    }
}
