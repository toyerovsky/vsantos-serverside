/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using Serverside.Core.Interfaces;

namespace Serverside.Core.Animations.Models
{
    [Serializable]
    public class Animation : IXmlObject
    {
        public string Name { get; set; }
        public string AnimDictionary { get; set; }
        public string AnimName { get; set; }
        public string FilePath { get; set; }
        public string CreatorForumName { get; set; }
    }
}