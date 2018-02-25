/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using Serverside.Core;
using Serverside.Core.Interfaces;

namespace Serverside.Entities.Common.Corners.Models
{
    [Serializable]
    public class CornerModel : IXmlObject
    {
        public List<CornerBotModel> CornerBots { get; set; }
        public FullPosition Position { get; set; }
        public List<FullPosition> BotPositions { get; set; }
        public string CreatorForumName { get; set; }
        public string FilePath { get; set; }
    }
}