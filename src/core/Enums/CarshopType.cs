/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.ComponentModel;

namespace VRP.Core.Enums
{
    [Flags]
    public enum CarshopType
    {
        Empty = 0,
        [Description("biedny")]
        Poor = 1 << 1,
        [Description("sredni")]
        Medium = 1 << 2,
        [Description("bogaty")]
        Luxury = 1 << 3,
    }
}