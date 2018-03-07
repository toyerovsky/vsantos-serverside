/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using Serverside.Entities.Core;

namespace Serverside.Entities.Interfaces
{
    public interface IOfferable
    {
        void Offer(CharacterEntity seller, CharacterEntity getter, decimal money);
    }
}