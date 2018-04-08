/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.Core.Tools
{
    public static class ValidationHelper
    {
        public static bool IsMoneyValid(decimal moneyToCheck) => moneyToCheck >= 0m;

        public static bool IsGroupSlotValid(string groupSlot) =>
            byte.TryParse(groupSlot, out byte slot) && IsGroupSlotValid(slot);
       
        public static bool IsCellphoneNumberValid(string number)
        {
            return int.TryParse(number, out int converted) && converted.ToString().Length > 0 && converted.ToString().Length <= 6;
        }

        public static bool IsGroupSlotValid(byte slot) => slot <= 3 && slot >= 1;
    }
}