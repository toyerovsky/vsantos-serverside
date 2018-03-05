/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using Serverside.Core.Database.Models;
using Serverside.Core.Extensions;
using Serverside.Entities.Core;

namespace Serverside.Entities.Misc.Description
{
    public class DescriptionsScript : Script
    {
        private void API_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        { 
            if (eventName == "OnPlayerAddDescription")
            {
                AccountEntity player = sender.GetAccountEntity();

                if (player.CharacterEntity.DbModel.Descriptions.Count > 3)
                {
                    sender.Notify("Liczba twoich opisów nie może być większa niż 3.");
                    return;
                }
                //arguments[0] tytul, arguments[1] opis
                DescriptionModel descriptionModel = new DescriptionModel()
                {
                    Character = sender.GetAccountEntity().CharacterEntity.DbModel,
                    Title = arguments[0].ToString(),
                    Content = arguments[1].ToString()
                };

                player.CharacterEntity.DbModel.Descriptions.Add(descriptionModel);
                player.Save();

                sender.Notify("Twój opis został pomyślnie dodany.");

                sender.TriggerEvent("ShowDescriptionsCef", false);

                string descriptionsJson = JsonConvert.SerializeObject(player.CharacterEntity.DbModel.Descriptions);

                sender.TriggerEvent("ShowDescriptionsCef", true, descriptionsJson);
            }
            else if (eventName == "OnPlayerEditDescription")
            {
                if (arguments[0] != null && arguments[1] != null && arguments[2] != null)
                {
                    //arguments[0] index na liście, arguments[1] tytul, arguments[2] opis
                    AccountEntity player = sender.GetAccountEntity();

                    DescriptionModel description = player.CharacterEntity.DbModel.Descriptions.ToList()[0];

                    description.Title = arguments[1].ToString();
                    description.Content = arguments[2].ToString();

                    player.Save();
                    
                    sender.Notify("Twój opis został pomyślnie edytowany.");

                    sender.TriggerEvent("ShowDescriptionsCef", false);

                    string descriptionsClient = JsonConvert.SerializeObject(player.CharacterEntity.DbModel.Descriptions.ToList());

                    sender.TriggerEvent("ShowDescriptionsCef", true, descriptionsClient);
                }
            }
            else if (eventName == "OnPlayerDeleteDescription")
            {
                if (arguments[0] != null)
                {
                    AccountEntity player = sender.GetAccountEntity();
                    List<DescriptionModel> descriptions = player.CharacterEntity.DbModel.Descriptions.ToList(); 

                    DescriptionModel description = descriptions[Convert.ToInt32(arguments[0])];

                    player.CharacterEntity.DbModel.Descriptions.Remove(description);
                    player.Save();

                    sender.Notify("Twój opis został pomyślnie usunięty.");
                    sender.TriggerEvent("ShowDescriptionsCef", false);

                    string descriptionsClient = JsonConvert.SerializeObject(player.CharacterEntity.DbModel.Descriptions);

                    sender.TriggerEvent("ShowDescriptionsCef", true, descriptionsClient);
                }
            }
            else if (eventName == "OnPlayerSetDescription")
            {
                //args[0] to string opisu
                if (arguments[0].ToString() != "")
                {
                    CharacterEntity player = sender.GetAccountEntity().CharacterEntity;
                    player.Description.Value = arguments[0].ToString();
                }
            }
            else if (eventName == "OnPlayerResetDescription")
            {
                CharacterEntity player = sender.GetAccountEntity().CharacterEntity;
                player.Description.ResetCurrentDescription();
            }
        }

        [Command("opis")]
        public void ShowDescriptionsList(Client sender)
        {
            CharacterEntity player = sender.GetAccountEntity().CharacterEntity;
            string descriptionsClient = JsonConvert.SerializeObject(player.DbModel.Descriptions);
            sender.TriggerEvent("ShowDescriptionsCef", descriptionsClient);
        }
    }
}