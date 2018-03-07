/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Core.Interfaces;

namespace VRP.Serverside.Entities.Common.BusStop.Models
{
    [Serializable]
    public class BusStopModel : IXmlObject
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public Vector3 Center { get; set; }
        public string CreatorForumName { get; set; }
        public string FilePath { get; set; }
    }
}