/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Serverside.Core.Interfaces;

namespace Serverside.Autonomic.Market.Models
{
    [Serializable]
    public class Market : IXmlObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3 Center { get; set; }
        public float Radius { get; set; }
        public List<MarketItem> Items { get; set; }
        public string CreatorForumName { get; set; }
        public string FilePath { get; set; }
    }
}