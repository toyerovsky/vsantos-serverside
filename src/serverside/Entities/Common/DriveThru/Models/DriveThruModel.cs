/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Core.Interfaces;

namespace VRP.Serverside.Entities.Common.DriveThru.Models
{
    [Serializable]
    public class DriveThruModel : IXmlObject
    {
        public int Id { get; set; }
        public Vector3 Position { get; set; }
        public string FilePath { get; set; }
        public string CreatorForumName { get; set; }
    }
}