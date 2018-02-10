/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using Serverside.Core.Database;
using Serverside.Core.Enums;

namespace Serverside.Core
{
    public static class Tools
    {
        /// <summary>
        /// Metoda zwraca następny wymiar który nie jest zajmowany przez budynek lub serwer
        /// </summary>
        /// <returns></returns>
        public static uint GetNextFreeDimension()
        {
            using (var ctx = RolePlayContextFactory.NewContext())
            {
                //Do pierwszego budynku tak trzeba zrobić
                if (!ctx.Buildings.Any()) return 1;
                uint last = ctx.Buildings.OrderByDescending(x => x.InternalDimension).Select(x => x.InternalDimension).First();
                do
                    ++last; while (Enum.GetValues(typeof(Dimension)).Cast<uint>().Any(x => x == last));
                return last;
            }
        }
    }
}