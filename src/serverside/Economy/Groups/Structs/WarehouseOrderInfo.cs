/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using VRP.Core.Database.Models;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Groups.Structs
{
    public struct WarehouseOrderInfo
    {
        public AccountEntity CurrentCourier { get; set; }
        public GroupWarehouseOrderModel Data { get; set; }
    }
}