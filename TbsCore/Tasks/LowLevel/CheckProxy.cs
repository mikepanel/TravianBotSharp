﻿using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System.Threading.Tasks;
using TravBotSharp.Files.Helpers;
using TravBotSharp.Files.Models.AccModels;

namespace TravBotSharp.Files.Tasks.LowLevel
{
    public class CheckProxy : BotTask
    {
        public override async Task<TaskRes> Execute(HtmlDocument htmlDoc, ChromeDriver wb, Files.Models.AccModels.Account acc)
        {
            await acc.Wb.Navigate("https://api.ipify.org/");

            var ip = htmlDoc.DocumentNode.InnerText;

            var currentProxy = acc.Access.GetCurrentAccess().Proxy;
            if (!string.IsNullOrEmpty(currentProxy) &&
                ip.Trim() != currentProxy.Trim())
            {
                // Proxy error!
                Utils.log.Information($"Failed proxy {currentProxy} for account {acc.AccInfo.Nickname}! Trying to get new proxy.");
                if(acc.Access.AllAccess.Count > 1)
                {
                    // Try another access.
                    acc.Wb.Close();

                    await Task.Delay(AccountHelper.Delay());

                    acc.Wb.InitSelenium(acc);
                    return TaskRes.Executed;
                }

            }

            return TaskRes.Executed;
        }
    }
}
