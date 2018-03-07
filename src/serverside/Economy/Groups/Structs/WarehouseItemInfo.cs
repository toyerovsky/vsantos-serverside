/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using VRP.Core.Database.Models;

namespace VRP.Serverside.Economy.Groups.Structs
{
    public struct WarehouseItemInfo
    {
        public GroupWarehouseItemModel ItemModelInfo { get; set; }
        public int Count { get; set; }
    }
}