/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using GTANetworkAPI;
using Serverside.Constant;

namespace Serverside.Core
{
    public sealed class LoaderScript : Script
    {
        public LoaderScript()
        {
            Event.OnResourceStart += Event_OnResourceStart;
        }

        private void Event_OnResourceStart()
        {
            Tools.ConsoleOutput($"[{nameof(LoaderScript)}] {Messages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
        }
    }
}