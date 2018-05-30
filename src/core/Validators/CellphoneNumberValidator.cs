/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Interfaces;

namespace VRP.Core.Validators
{
    public class CellphoneNumberValidator : IValidator<string>
    {
        public bool IsValid(string value)
        {
            return int.TryParse(value, out int converted) 
                   && converted.ToString().Length > 0 
                   && converted.ToString().Length <= 6;
        }
    }
}