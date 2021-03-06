﻿using Elasticsearch.Net.Specification.IndexLifecycleManagementApi;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.CompilerServices;
using TravBotSharp.Files.Models.AccModels;
using TravBotSharp.Files.Tasks;

namespace TravBotSharp.Files.Helpers
{
    public static class IoHelperCore
    {
        public static string AccountsPath => Path.Combine(TbsPath(), "accounts.txt");
        public static string CachePath => Path.Combine(TbsPath(), "cache");

        public static void AddBuildTasksFromFile(Account acc, Village vill, string location)
        {
            List<BuildingTask> tasks = null;
            try
            {
                using (StreamReader sr = new StreamReader(location))
                {
                    tasks = JsonConvert.DeserializeObject<List<BuildingTask>>(sr.ReadToEnd());
                }
            }
            catch (Exception e) { return; } // User canceled

            foreach (var task in tasks)
            {
                BuildingHelper.AddBuildingTask(acc, vill, task);
            }
            BuildingHelper.RemoveCompletedTasks(vill, acc);
        }

        public static string TbsPath()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            return Path.Combine(appData, "TravBotSharp");
        }

        /// <summary>
        /// Removes the cache folders that were created by Selenium driver, since they take a lot of space (70MB+)
        /// </summary>
        /// <param name="acc">Account</param>
        public static void RemoveCache(Account acc)
        {
            var userFolder = IoHelperCore.GetCacheFolder(acc.AccInfo.Nickname, acc.AccInfo.ServerUrl, "");

            var removeFolders = Directory
                .GetDirectories(CachePath + "\\")
                .Where(x => x.Replace(CachePath + "\\", "").StartsWith(userFolder))
                .ToArray();

            for (int i = 0; i < removeFolders.Count(); i++)
            {
                Directory.Delete(removeFolders[i], true);
            }
        }

        /// <summary>
        /// Removes the protocol (http/https) text from the url
        /// </summary>
        /// <param name="url">Url</param>
        /// <returns>Shortened url</returns>
        public static string UrlRemoveHttp(string url)
        {
            return url.Replace("https://", "").Replace("http://", "");
        }

        /// <summary>
        /// Read accounts from the accounts.txt file
        /// </summary>
        /// <returns>Accounts saved in the file</returns>
        public static List<Account> ReadAccounts()
        {
            var accounts = new List<Account>();
            try
            {
                // Open the text file using a stream reader.
                var folder = IoHelperCore.TbsPath();
                System.IO.Directory.CreateDirectory(folder);

                using (StreamReader sr = new StreamReader(IoHelperCore.AccountsPath))
                {
                    accounts = JsonConvert.DeserializeObject<List<Account>>(sr.ReadToEnd());
                }
                if (accounts == null) accounts = new List<Account>();
            }
            catch (IOException e)
            {

                Console.WriteLine("Can't read accounts.txt, Exception thrown: " + e.Message);
            }
            return accounts;
        }

        /// <summary>
        /// Cache folder selenium will use for this account
        /// </summary>
        /// <param name="username">Username</param>
        /// <param name="server">Server url</param>
        /// <param name="proxy">Proxy ip</param>
        /// <returns></returns>
        internal static string GetCacheFolder(string username, string server, string proxy)
        {
            return $"{username}_{IoHelperCore.UrlRemoveHttp(server)}_{proxy}";
        }
        /// <summary>
        /// Quit (stop) the program. This will logout (close drivers) from all accounts and save them into the file
        /// </summary>
        /// <param name="accounts"></param>
        public static void Quit(List<Account> accounts)
        {
            foreach (var acc in accounts)
            {
                Logout(acc);
            }
            using (StreamWriter sw = new StreamWriter(AccountsPath))
            {
                sw.Write(JsonConvert.SerializeObject(accounts));
            }
        }
        /// <summary>
        /// Login into account and initialize everything
        /// </summary>
        /// <param name="acc">Account</param>
        public static void LoginAccount(Account acc)
        {
            if (acc.Wb == null)
            { // If Agent doesn't exist yet
                acc.Tasks = new List<BotTask>();
                acc.Wb = new WebBrowserInfo();
                acc.Wb.InitSelenium(acc);
                acc.TaskTimer = new TaskTimer(acc);

                AccountHelper.StartAccountTasks(acc);
            }
        }
        /// <summary>
        /// Logout from the account. Closes web driver.
        /// </summary>
        /// <param name="acc"></param>
        public static void Logout(Account acc)
        {
            if (acc.TaskTimer != null)
            {
                acc.TaskTimer.Stop();
                acc.TaskTimer = null;
            }
            if (acc.Wb != null)
            {
                acc.Wb.Close();
                acc.Wb = null;
            }
            acc.Tasks = null; //TODO: somehow save tasks, JSON cant parse/stringify abstract classes :(
        }
    }
}
