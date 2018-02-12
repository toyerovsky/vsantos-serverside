/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using Serverside.Core.Database.Models;
using Serverside.Core.Repositories;
using Serverside.Entities.Core.Item;

namespace Serverside.Core.Telephone
{
    public static class TelephoneHelper
    {
        public static int GetNextFreeTelephoneNumber()
        {
            using (ItemsRepository repository = new ItemsRepository())
            {
                var numbers = repository.GetAll()
                    .Where(i => i.ItemType == ItemType.Cellphone && i.ThirdParameter.HasValue)
                    .Select(i => i.ThirdParameter.Value);

                Random random = new Random();
                int freeTelephoneNumber = random.Next(100000);

                var numbersArray = numbers as int[] ?? numbers.ToArray();
                while (numbersArray.Any(number => number == freeTelephoneNumber))
                {
                    freeTelephoneNumber = random.Next(100000);
                }
                return freeTelephoneNumber;
            }
        }
    }
}