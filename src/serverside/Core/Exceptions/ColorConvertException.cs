/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Runtime.Serialization;

namespace VRP.Serverside.Core.Exceptions
{
    [Serializable]
    public class ColorConvertException : Exception
    {
        public ColorConvertException()
        {
        }

        public ColorConvertException(string message) : base(message)
        {
        }

        public ColorConvertException(string message, Exception inner) : base(message, inner)
        {
        }

        protected ColorConvertException(SerializationInfo info, StreamingContext context) : base(info, context)
        {
        }
    }
}