/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Entities.Interfaces
{
    public interface IOfferable
    {
        void Offer(CharacterEntity seller, CharacterEntity getter, decimal money);
    }
}