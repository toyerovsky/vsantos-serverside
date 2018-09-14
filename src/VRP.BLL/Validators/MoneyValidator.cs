/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.BLL.Interfaces;

namespace VRP.BLL.Validators
{
    public class MoneyValidator : IValidator<decimal>
    {
        public bool IsValid(decimal value)
        {
            return value >= 0m;
        }
    }
}