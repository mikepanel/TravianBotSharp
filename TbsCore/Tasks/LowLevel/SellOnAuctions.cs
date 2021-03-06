﻿using HtmlAgilityPack;
using OpenQA.Selenium.Chrome;
using System.Linq;
using System.Threading.Tasks;
using TravBotSharp.Files.Models.AccModels;

namespace TravBotSharp.Files.Tasks.LowLevel
{
    public class SellOnAuctions : BotTask
    {
        public int ItemId { get; set; }
        public override async Task<TaskRes> Execute(HtmlDocument htmlDoc, ChromeDriver wb, Files.Models.AccModels.Account acc)
        {
            await acc.Wb.Navigate($"{acc.AccInfo.ServerUrl}/hero.php?t=4&action=sell");

            var yesButton = htmlDoc.DocumentNode.Descendants().Where(x => x.Attributes.Any(a => a.Value.Contains("green ok dialogButtonOk"))).First();
            //if()
            //Make dropdown menu selectable!

            wb.ExecuteScript($"document.getElementsByClassName(\"green ok dialogButtonOk\")[0].click()");

            return TaskRes.Executed;
        }
    }
}
