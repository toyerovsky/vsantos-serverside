/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using VRP.Core;
using VRP.Core.Tools;
using VRP.DAL.Database;
using VRP.DAL.Enums;
using VRP.DAL.Repositories;

namespace VRP.Serverside.Core.Telephone
{
    public static class TelephoneHelper
    {
        public static int GetNextFreeTelephoneNumber()
        {
            RoleplayContext ctx = Singletons.RoleplayContextFactory.Create();
            using (ItemsRepository repository = new ItemsRepository(ctx))
            {
                IEnumerable<int> numbers = repository.GetAll()
                    .Where(i => i.ItemEntityType == ItemEntityType.Cellphone && i.ThirdParameter.HasValue)
                    .Select(i => i.ThirdParameter.Value);

                int freeTelephoneNumber = Utils.RandomRange(100000);

                int[] numbersArray = numbers as int[] ?? numbers.ToArray();
                while (numbersArray.Any(number => number == freeTelephoneNumber))
                    freeTelephoneNumber = Utils.RandomRange(100000);
                
                return freeTelephoneNumber;
            }
        }
    }
}