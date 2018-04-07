/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Scripts;
using VRP.Serverside.Core.Telephone;
using VRP.Serverside.Economy.Groups.Base;
using VRP.Serverside.Entities.Peds.CrimeBot.Models;
using NAPI = GTANetworkAPI.NAPI;

namespace VRP.Serverside.Entities.Core.Item.Scripts
{
    public class ItemsScript : Script
    {
        private ItemEntityFactory _itemFactory { get; } = new ItemEntityFactory();

        private void API_OnClientEventTrigger(Client sender, string eventName, params object[] args)
        {
            if (eventName == "SelectedItem")
            {
                int index = Convert.ToInt32(args[0]);
                sender.SetData("SelectedItem", index);
                sender.TriggerEvent("SelectOptionItem", index);
            }
            else if (eventName == "UseItem")
            {
                CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
                int index = Convert.ToInt32(sender.GetData("SelectedItem"));

                ItemEntity item = _itemFactory.Create(character.DbModel.Items.ToList()[index]);
                item.UseItem(character);
            }
            else if (eventName == "InformationsItem")
            {
                AccountEntity player = sender.GetAccountEntity();

                int index = Convert.ToInt32(sender.GetData("SelectedItem"));
                List<ItemModel> userItems = player.CharacterEntity.DbModel.Items.ToList();

                ItemEntity item = _itemFactory.Create(userItems[index]);
                sender.SendInfo(item.ItemInfo);

            }
            else if (eventName == "UsingInformationsItem")
            {
                AccountEntity player = sender.GetAccountEntity();

                int index = Convert.ToInt32(sender.GetData("SelectedItem"));
                List<ItemModel> userItems = player.CharacterEntity.DbModel.Items.ToList();

                ItemEntity item = _itemFactory.Create(userItems[index]);
                sender.SendInfo(item.UseInfo);

            }
            else if (eventName == "BackToItemList")
            {
                AccountEntity player = sender.GetAccountEntity();
                string itemsJson = JsonConvert.SerializeObject(player.CharacterEntity.DbModel.Items.ToList());
                sender.TriggerEvent("ShowItems", itemsJson);
            }
            //args[0] to numer na jaki dzwoni
            else if (eventName == "OnPlayerTelephoneCall")
            {
                AccountEntity player = sender.GetAccountEntity();

                if (Convert.ToInt32(args[0]) == 555 && player.CharacterEntity.OnDutyGroup is CrimeGroup group)
                {
                    if (group.CanPlayerCallCrimeBot(player))
                    {
                        List<string> names = XmlHelper.GetXmlObjects<CrimeBotPosition>(Path.Combine(Utils.XmlDirectory, "CrimeBotPositions"))
                            .Select(n => n.Name).ToList();
                        sender.TriggerEvent("OnPlayerCalledCrimeBot", names);
                        return;
                    }
                }

                if (sender.GetAccountEntity().CharacterEntity.CurrentCellphone.CurrentCall != null)
                {
                    sender.Notify("Obecnie prowadzisz rozmowę telefoniczną. Zakończ ją klawiszem END.", NotificationType.Error);
                    return;
                }
                // FixMe animka dzwonienia przez telefon
                //NAPI.playPlayerAnimation(senderPlayer.Client, (int)(AnimationFlags.AllowPlayerControl | AnimationFlags.Loop),
                //    "cellphone@first_person", "cellphone_call_listen_base");

                int number = Convert.ToInt32(args[0]);
                if (EntityHelper.GetAccounts().Any(p => p.CharacterEntity.CurrentCellphone.Number == number))
                {
                    CharacterEntity getterCharacter = EntityHelper.GetAccounts()
                        .Single(p => p.CharacterEntity.CurrentCellphone.Number == number).CharacterEntity;

                    if (getterCharacter.CurrentCellphone.CurrentCall != null)
                    {
                        sender.SendChatMessage("~#ffdb00~",
                            "Wybrany abonent prowadzi obecnie rozmowę, spróbuj później.");
                        return;
                    }

                    CharacterEntity senderCharacter = sender.GetAccountEntity().CharacterEntity;

                    TelephoneCall call = new TelephoneCall(senderCharacter, getterCharacter);
                    sender.GetAccountEntity().CharacterEntity.CurrentCellphone.CurrentCall = call;

                    call.Timer.Elapsed += (o, eventArgs) =>
                    {
                        sender.SendChatMessage("~#ffdb00~",
                            "Wybrany abonent ma wyłączony telefon, bądź znajduje się poza zasięgiem, spróbuj później.");
                        call.Dispose();
                    };
                }
                else
                {
                    sender.SendChatMessage("~#ffdb00~",
                        "Wybrany abonent ma wyłączony telefon, bądź znajduje się poza zasięgiem, spróbuj później.");
                }
            }
            else if (eventName == "OnPlayerTelephoneTurnoff")
            {
                TelephoneCall call = sender.GetAccountEntity().CharacterEntity.CurrentCellphone.CurrentCall;
                call?.Dispose();

                //TODO: Wyłączanie telefonu

            }
            //Rządanie otworzenia okienka telefonu
            else if (eventName == "OnPlayerPullCellphoneRequest")
            {
                Cellphone cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;
                sender.TriggerEvent("OnPlayerPulledCellphone", cellphone.Name,
                    JsonConvert.SerializeObject(cellphone.Contacts),
                    JsonConvert.SerializeObject(cellphone.Messages));
            }
            //Odebranie rozmowy
            else if (eventName == "OnPlayerCellphonePickUp")
            {
                Cellphone cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;
                TelephoneCall telephoneCall = cellphone.CurrentCall;


                if (telephoneCall.Getter.CurrentCellphone.CurrentCall != null)
                {
                    telephoneCall.Getter.SendWarning("Aby odebrać musisz zakończyć bieżące połączenie.");
                    return;
                }

                //NAPI.playPlayerAnimation(telephoneCall.Getter.Client, (int)(AnimationFlags.AllowPlayerControl),
                //    "cellphone@first_person", "cellphone_call_listen_base");

                telephoneCall.Open();

                telephoneCall.Getter.SendInfo("Odebrano telefon, aby zakończyć rozmowę naciśnij klawisz END.");
                telephoneCall.Sender.SendInfo("Rozmówca odebrał telefon, aby zakończyć rozmowę naciśnij klawisz END.");
            }
            else if (eventName == "OnPlayerCellphoneEnd")
            {
                Cellphone cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;

                if (cellphone?.CurrentCall != null)
                {
                    cellphone.CurrentCall.Sender.SendInfo("Rozmowa zakończona.");
                    cellphone.CurrentCall.Getter.SendInfo("Rozmowa zakończona.");

                    cellphone.CurrentCall = null;
                }
            }
            //args[0] to numer kontaktu args[1] to nazwa 
            else if (eventName == "OnPlayerTelephoneContactAdded")
            {
                Cellphone cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;
                if (cellphone == null)
                {
                    sender.Notify("Musisz mieć włączony telefon.", NotificationType.Error);
                    return;
                }

                int number = Convert.ToInt32(args[0]);
                string name = args[1].ToString();

                TelephoneContactModel telephoneContactModel = new TelephoneContactModel
                {
                    Name = name,
                    Number = number,
                    Id = cellphone.Id
                };
                cellphone.Contacts.Add(telephoneContactModel);
            }
        }

        #region Komendy
        [Command("p")]
        public void ShowItemsList(Client sender)
        {
            NAPI.ClientEvent.TriggerClientEvent(sender, "ShowItems", JsonConvert.SerializeObject(sender.GetAccountEntity().CharacterEntity.DbModel.Items.ToList().Select(x => new
            {
                x.Name
            })));
        }
        #endregion
    }
}