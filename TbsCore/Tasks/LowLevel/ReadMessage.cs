﻿using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System.Linq;
using System.Threading.Tasks;
using TravBotSharp.Files.Helpers;
using TravBotSharp.Files.Models.AccModels;


namespace TravBotSharp.Files.Tasks.LowLevel
{
    public class ReadMessage : BotTask
    {
        public override async Task<TaskRes> Execute(HtmlDocument htmlDoc, ChromeDriver wb, Files.Models.AccModels.Account acc)
        {
            await acc.Wb.Navigate($"{acc.AccInfo.ServerUrl}/messages.php");

            var msg = htmlDoc.DocumentNode.Descendants("img").Where(x => x.HasClass("messageStatusUnread")).FirstOrDefault();
            if (msg != null)
            {
                var url = msg.ParentNode.GetAttributeValue("href", "").Replace("amp;", "");
                await acc.Wb.Navigate(acc.AccInfo.ServerUrl + "/" + url);
            }
            return TaskRes.Executed;
        }
    }
}
