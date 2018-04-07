/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Drawing;
using VRP.Serverside.Interfaces;

namespace VRP.Serverside.Entities.Base
{
    public abstract class GameEntity : IGameEntity
    {
        public virtual void Spawn()
        {
            Colorful.Console.WriteLine($"[{nameof(GameEntity)}] [{DateTime.Now.ToShortTimeString()}] Entity spawned.", Color.ForestGreen);
        }

        public abstract void Dispose();
    }
}