/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;

namespace Serverside.Entities.Interfaces
{
    public interface IGameEntity : IDisposable
    {
        void Spawn();
    }
}