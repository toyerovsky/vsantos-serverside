using System;
using System.Collections.Generic;
using System.Text;

namespace VRP.Core.Enums
{
    [Flags]
    public enum GroupRights
    {
        DepositWithdrawMoney = 1 << 0,
        Doors = 1 << 1,
        Recrutation = 1 << 2,
        Chat = 1 << 3,
        OfferFromWarehouse = 1 << 4,
        OrderToWareouse = 1 << 5,
        First = 1 << 6,
        Second = 1 << 7,
        Third = 1 << 8,
        Fourth = 1 << 9,
        Fifth = 1 << 10,
        Sixth = 1 << 11,
        Seventh = 1 << 12,
        Eight = 1 << 13,
        Ninth = 1 << 14
    }
}
