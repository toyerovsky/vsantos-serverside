/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using Serverside.Core.Serialization;
using Serverside.Entities.Common.Corners.Models;

namespace Serverside.Entities.Common.Corners.Helpers
{
    public class CornerBotHelper
    {
        public static bool TryGetCornerBotIds(List<string> botIds, out List<int> correctBotIds)
        {
            correctBotIds = new List<int>();
            List<int> ids = XmlHelper.GetXmlObjects<CornerBotModel>(Constant.ServerInfo.XmlDirectory +
                                                              @"CornerBots\").Select(x => x.BotId).ToList();            
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