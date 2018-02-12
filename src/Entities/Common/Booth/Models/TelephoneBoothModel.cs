/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using Serverside.Core;
using Serverside.Core.Interfaces;

namespace Serverside.Entities.Common.Booth.Models
{
    [Serializable]
    public class TelephoneBoothModel : IXmlObject
    {
        public int Number { get; set; }
        public decimal Cost { get; set; }
        public FullPosition Position { get; set; }
        public string CreatorForumName { get; set; }
        public string FilePath { get; set; }
    }
}