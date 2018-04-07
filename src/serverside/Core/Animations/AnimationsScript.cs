/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.IO;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Animations.Models;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Core.Animations
{
    public class AnimationsScript : Script
    {
        [RemoteEvent(RemoteEvents.OnPlayerAddAnim)]
        public void OnPlayerAddAnimHandler(Client sender, string eventName, params object[] arguments)
        {
            //args[0] Polska nazwa animacji
            //args[1] Słownik animacji z GTA:N
            //args[2] Nazwa animacji z GTA:N

            XmlHelper.AddXmlObject(new Animation
            {
                Name = arguments[0].ToString(),
                AnimDictionary = arguments[1].ToString(),
                AnimName = arguments[2].ToString(),
            }, Path.Combine(Utils.XmlDirectory + "Animations"), arguments[2].ToString());

                sender.SendInfo("Animacja została dodana pomyślnie.");
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