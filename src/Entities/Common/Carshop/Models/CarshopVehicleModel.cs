﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using GTANetworkAPI;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Interfaces;

namespace Serverside.Entities.Common.Carshop.Models
{
    [Serializable]
    public class CarshopVehicleModel : IXmlObject
    {
        public string Name { get; set; }
        public VehicleHash Hash { get; set; }
        public VehicleClass Category { get; set; }
        public decimal Cost { get; set; }
        public string FilePath { get; set; }
        public string CreatorForumName { get; set; }
        public CarshopType CarshopTypes { get; set; }

        public CarshopVehicleModel(string name, VehicleHash hash, VehicleClass category, decimal cost, CarshopType type)
        {
            Name = name;
            Hash = hash;
            Category = category;
            Cost = cost;
            CarshopTypes = type;
        }

        public CarshopVehicleModel() { }
    }
}