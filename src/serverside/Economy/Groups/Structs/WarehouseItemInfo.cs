/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Database.Models.Warehouse;

namespace VRP.Serverside.Economy.Groups.Structs
{
    public struct WarehouseItemInfo
    {
        public GroupWarehouseItemModel ItemModelInfo { get; set; }
        public int Count { get; set; }
    }
}