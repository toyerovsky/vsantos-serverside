/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using Serverside.Core.Database.Models;
using Serverside.Core.Repositories;
using Serverside.Items;

namespace Serverside.Core.Telephone
{
    public static class TelephoneHelper
    {
        public static int GetNextFreeTelephoneNumber()
        {
            using (ItemsRepository repository = new ItemsRepository())
            {
                var numbers = repository.GetAll().Where(i => i.ItemType == ItemType.Cellphone);

                Random r = new Random();
                int number = r.Next(100000);

                var itemModels = numbers as ItemModel[] ?? numbers.ToArray();
                while (itemModels.Any(t => t.ThirdParameter == number))
                {
                    number = r.Next(100000);
                }
                return number;
            }
        }
    }
}