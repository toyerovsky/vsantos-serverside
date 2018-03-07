/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Linq;
using GTANetworkAPI;
using Serverside.Core.Animations.Models;
using Serverside.Core.Extensions;
using Serverside.Core.Serialization;

namespace Serverside.Core.Animations
{
    public class AnimationsScript : Script
    {
        private void OnClientEventTriggerHandler(Client sender, string eventName, params object[] arguments)
        {
            //args[0] Polska nazwa animacji
            //args[1] Słownik animacji z GTA:N
            //args[2] Nazwa animacji z GTA:N
            if (eventName == "OnPlayerAddAnim")
            {
                XmlHelper.AddXmlObject(new Animation
                {
                    Name = arguments[0].ToString(),
                    AnimDictionary = arguments[1].ToString(),
                    AnimName = arguments[2].ToString(),
                }, Constant.ServerInfo.XmlDirectory + @"Animations\", arguments[2].ToString());

                sender.Notify("Animacja została dodana pomyślnie.");
            }
        }

        #region Komendy administracji
        [Command("dodajanim")]
        public void AddAnim(Client player)
        {
            //Wyświetlamy menu w którym administrator może grupować animacje
            //Przesyłamy dostępne animacje
            player.TriggerEvent("ShowAdminAnimMenu", Constant.Items.Animations.Select(anim => $"{anim.Key},{anim.Value}").ToList());
        }
        #endregion

    }
}