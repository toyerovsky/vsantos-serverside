/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using VRP.Core.Interfaces;
using FullPosition = VRP.Serverside.Core.FullPosition;

namespace VRP.Serverside.Entities.Peds.CrimeBot.Models
{
    [Serializable]
    public class CrimeBotPosition : IXmlObject
    {
        public string Name { get; set; }
        public FullPosition BotPosition { get; set; }
        public FullPosition VehiclePosition { get; set; }
        public string CreatorForumName { get; set; }
        public string FilePath { get; set; }
    }
}