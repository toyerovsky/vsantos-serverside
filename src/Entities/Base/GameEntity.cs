/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkInternals;
using Serverside.Entities.Interfaces;

namespace Serverside.Entities.Base
{
    public abstract class GameEntity : IGameEntity
    {
        protected EventClass Events { get; }

        protected GameEntity(EventClass events)
        {
            Events = events;
        }

        public virtual void Spawn()
        {
        }

        public abstract void Dispose();
    }
}