﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using Serverside.Core.Interfaces;

namespace Serverside.Core.ServerInfo.Models
{
    public class ServerInfoModel : IXmlObject
    {
        public string FilePath { get; set; }
        public string CreatorForumName { get; set; }

        public DateTime JobsResetTime { get; set; }
        public DateTime CrimeBotResetTime { get; set; }
        public DateTime GroupsPayDayTime { get; set; }
    }
}