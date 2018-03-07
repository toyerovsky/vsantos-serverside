﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using Serverside.Core;
using Serverside.Core.Interfaces;

namespace Serverside.Entities.Common.Atm.Models
{
    [Serializable]
    public class AtmModel : IXmlObject
    {
        public int Id { get; set; }
        public FullPosition Position { get; set; }
        public string CreatorForumName { get; set; }
        public string FilePath { get; set; }
    }
}