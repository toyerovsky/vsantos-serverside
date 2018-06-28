﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;

namespace VRP.DAL.Enums
{
    [Flags]
    public enum GroupRights
    {
        None = 0,
        First = 1 << 0,
        Second = 1 << 1,
        Third = 1 << 2,
        Fourth = 1 << 3,
        Fifth = 1 << 4,
        Sixth = 1 << 5,
        Seventh = 1 << 6,
        Eight = 1 << 7,
        Ninth = 1 << 8,
        DepositWithdrawMoney = 1 << 9,
        Doors = 1 << 10,
        Recrutation = 1 << 11,
        Chat = 1 << 12,
        OfferFromWarehouse = 1 << 13,
        OrderToWareouse = 1 << 14,
        AllBasic = DepositWithdrawMoney | Doors | Recrutation | Chat | OfferFromWarehouse | OrderToWareouse,
    }
}