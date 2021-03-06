﻿using System.Collections.Generic;
using TravBotSharp.Files.Models.ResourceModels;
using static TravBotSharp.Files.Helpers.Classificator;

namespace TravBotSharp.Files.TravianData
{
    /// <summary>
    /// Class for dealing with localization. TODO: add new languages, save language into the acc/access model
    /// </summary>
    public static class Localizations
    {
        private static Dictionary<Language, List<string>> buildings = new Dictionary<Language, List<string>>()
        {
            { Language.English, new List<string> { "site", "woodcutter", "clay pit", "iron mine", "cropland", "sawmill", "brickyard", "iron foundry", "grain mill", "bakery", "warehouse", "granary", "blacksmith", "smithy", "tournament square", "main building", "rally point", "marketplace", "embassy", "barracks", "stable", "workshop", "academy", "cranny", "town hall", "residence", "palace", "treasury", "trade office", "great barracks", "great stable", "city wall", "earth wall", "palisade", "stonemason", "brewery", "trapper", "hero's mansion", "great warehouse", "great granary", "wonder of the world", "horse drinking trough", "water ditch", "natarian wall", "stone wall", "makeshift wall", "command center", "waterworks" } }
        };
        private static Dictionary<Language, List<string>> merchants = new Dictionary<Language, List<string>>()
        {
             { Language.English, new List<string> { "returning merchants:", "incoming merchants:", "ongoing merchants:" } }
        };

        public static TransitType MercahntDirectionFromString(string str)
        {
            var strs = merchants[Language.English];
            var index = strs.IndexOf(str.Trim().ToLower());
            return (TransitType)index;
        }
        public static BuildingEnum BuildingFromString(string str/*, Language lang*/)
        {
            var strs = buildings[Language.English];
            var index = strs.IndexOf(str.Trim().ToLower());
            return (BuildingEnum)index;
        }

        public enum Language
        {
            English
        }

    }
}
