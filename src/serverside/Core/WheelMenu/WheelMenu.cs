/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Core.WheelMenu
{
    public class WheelMenu : IDisposable
    {
        public List<WheelMenuItem> WheelMenuItems { get; }
        private Client Sender { get; }

        public WheelMenu(List<WheelMenuItem> wheelMenuItems, Client sender)
        {
            WheelMenuItems = wheelMenuItems;
            Sender = sender;
            Show();
        }

        private void Show()
        {
            Sender.TriggerEvent(RemoteEvents.ShowWheelMenu,
                JsonConvert.SerializeObject(WheelMenuItems.Select(a => new
                {
                    name = a.Name
                })));
        }

        public void Dispose()
        {
            Sender.ResetData("WheelMenu");
        }
    }
}