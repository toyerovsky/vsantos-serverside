/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Entities.Common.Corners.Models;

namespace VRP.Serverside.Entities.Common.Corners.Helpers
{
    public class CornerBotHelper
    {
        public static bool TryGetCornerBotIds(List<string> botIds, out List<int> correctBotIds)
        {
            correctBotIds = new List<int>();
            List<int> ids = XmlHelper.GetXmlObjects<CornerBotModel>(Path.Combine(Utils.XmlDirectory, "CornerBots"))
                .Select(x => x.BotId).ToList();            
            foreach (string id in botIds)
            {
                int correctId = Convert.ToInt32(id);
                if (!ids.Contains(correctId))
                {
                    return false;
                }
                correctBotIds.Add(correctId);
            }
            return true;
        }
    }
}