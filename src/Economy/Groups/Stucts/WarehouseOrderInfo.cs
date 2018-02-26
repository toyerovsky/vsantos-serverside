/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Serverside.Core.Database.Models;
using Serverside.Entities.Core;

namespace Serverside.Groups.Stucts
{
    public struct WarehouseOrderInfo
    {
        public AccountEntity CurrentCourier { get; set; }
        public GroupWarehouseOrderModel Data { get; set; }
    }
}