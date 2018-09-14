/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.BLL.Interfaces;

namespace VRP.BLL.Validators
{
    public class GroupSlotValidator : IValidator<byte>
    {
        public bool IsValid(byte value)
        {
            return value <= 3 && value >= 1;
        }
    }
}