/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using GTANetworkAPI;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core.WheelMenu
{
    public class WheelMenuItem
    {
        public string Name { get; }
        private CharacterEntity Sender { get; }
        private object Target { get; }
        private Action<CharacterEntity, object> WheelAction { get; }

        public WheelMenuItem(string name, CharacterEntity sender, object target, Action<CharacterEntity, object> wheelAction)
        {
            Name = name;
            Sender = sender;
            Target = target;
            WheelAction = wheelAction;
        }

        public void Use()
        {
            WheelAction.Invoke(Sender, Target);   
        }
    }
}