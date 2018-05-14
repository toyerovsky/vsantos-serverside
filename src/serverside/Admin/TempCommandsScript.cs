/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using GTANetworkAPI;
using VRP.Serverside.Constant.RemoteEvents;
using VRP.Serverside.Core.Extensions;

namespace VRP.Serverside.Admin
{
    public class TempCommandsScript : Script
    {
        // Mugshot bedzie dodany jako przedmiot do zamówienia przez PD, komenda tylko do screena na facebook
        [Command("dmug", "~y~ UŻYJ ~w~ /dmug [tytuł], [topText], [midText], [bottomText]", GreedyArg = true)]
        public void ShowMugshot(Client sender, string text)
        {
            string[] parameters = text.Split(",");

            if (parameters.Length != 4)
            {
                sender.SendError("Wprowadzono dane w nieprawidłowym formacie.");
                return;
            }

            if (!sender.HasData("mugshot"))
            {
                sender.SetData("mugshot", true);
                sender.TriggerEvent(RemoteEvents.PlayerMugshotRequested, parameters[0], parameters[1], parameters[2], parameters[3]);
            }
            else
            {
                sender.ResetData("mugshot");
                sender.TriggerEvent(RemoteEvents.PlayerMugshotDestroyRequested);
            }
        }
    }
}