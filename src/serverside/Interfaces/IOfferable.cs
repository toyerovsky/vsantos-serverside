/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Interfaces
{
    public interface IOfferable
    {
        void Offer(CharacterEntity seller, CharacterEntity getter, decimal money);
    }
}