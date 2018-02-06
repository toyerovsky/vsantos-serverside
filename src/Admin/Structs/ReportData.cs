/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Serverside.Admin.Enums;
using Serverside.Entities.Core;

namespace Serverside.Admin.Structs
{
    public struct ReportData
    {
        public AccountEntity Sender { get; set; }
        public AccountEntity Accused { get; set; }
        public string Content { get; set; }
        public ReportType Type { get; set; }
    }
}