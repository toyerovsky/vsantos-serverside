/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.Linq;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Core.Tools;

namespace VRP.Serverside.Core.Telephone
{
    public static class TelephoneHelper
    {
        public static int GetNextFreeTelephoneNumber()
        {
            using (ItemsRepository repository = new ItemsRepository())
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