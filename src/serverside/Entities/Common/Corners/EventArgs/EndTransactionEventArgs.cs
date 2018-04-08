﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.Serverside.Entities.Common.Corners.EventArgs
{
    public class EndTransactionEventArgs : System.EventArgs
    {
        public bool Good { get; set; }

        public EndTransactionEventArgs(bool good)
        {
            Good = good;
        }
    }
}
