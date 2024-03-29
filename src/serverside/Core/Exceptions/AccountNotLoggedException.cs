﻿/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Runtime.Serialization;

namespace VRP.Serverside.Core.Exceptions
{
    [Serializable]
    public class AccountNotLoggedException : Exception
    {
        public AccountNotLoggedException()
        {
        }

        public AccountNotLoggedException(string message) : base(message)
        {
        }

        public AccountNotLoggedException(string message, Exception inner) : base(message, inner)
        {
        }

        protected AccountNotLoggedException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}