/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Enums;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Admin.Structs
{
    public struct ReportData
    {
        public AccountEntity Sender { get; set; }
        public AccountEntity Accused { get; set; }
        public string Content { get; set; }
        public ReportType Type { get; set; }
    }
}