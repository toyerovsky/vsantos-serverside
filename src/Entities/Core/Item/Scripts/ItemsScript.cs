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
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;
using Serverside.Core.Serialization.Xml;
using Serverside.Core.Telephone;
using Serverside.Entities.Peds.CrimeBot.Models;
using Serverside.Groups.Base;
using NAPI = GTANetworkAPI.NAPI;

namespace Serverside.Entities.Core.Item.Scripts
{
    public class ItemsScript : Script
    {
        private Item CreateItem(ItemModel itemModel)
        {
            var itemType = (ItemType)itemModel.ItemType;
            switch (itemType)
            {
                case ItemType.Food: return new Food(Event, itemModel);
                case ItemType.Weapon: return new Weapon(Event, itemModel);
                case ItemType.WeaponClip: return new WeaponClip(Event, itemModel);
                case ItemType.Mask: return new Mask(Event, itemModel);
                case ItemType.Drug: return new Drug(Event, itemModel);
                case ItemType.Dice: return new Dice(Event, itemModel);
                case ItemType.Watch: return new Watch(Event, itemModel);
                case ItemType.Cloth: return new Cloth(Event, itemModel);
                case ItemType.Transmitter: return new Transmitter(Event, itemModel);
                case ItemType.Cellphone: return new Cellphone(Event, itemModel);
                case ItemType.Tuning: return new Tuning(Event, itemModel);

                default:
                    throw new NotSupportedException($"Podany typ przedmiotu {itemType} nie jest obsługiwany.");
            }
        }

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
                var player = sender.GetAccountEntity();
                int index = Convert.ToInt32(sender.GetData("SelectedItem"));

                Item item = CreateItem(player.CharacterEntity.DbModel.Items.ToList()[index]);
                item.UseItem(player);
            }
            else if (eventName == "InformationsItem")
            {
                var player = sender.GetAccountEntity();

                int index = Convert.ToInt32(sender.GetData("SelectedItem"));
                List<ItemModel> userItems = player.CharacterEntity.DbModel.Items.ToList();

                Item item = CreateItem(userItems[index]);
                ChatScript.SendMessageToPlayer(sender, item.ItemInfo, ChatMessageType.ServerInfo);

            }
            else if (eventName == "UsingInformationsItem")
            {
                var player = sender.GetAccountEntity();

                int index = Convert.ToInt32(sender.GetData("SelectedItem"));
                List<ItemModel> userItems = player.CharacterEntity.DbModel.Items.ToList();

                Item item = CreateItem(userItems[index]);
                ChatScript.SendMessageToPlayer(sender, item.UseInfo, ChatMessageType.ServerInfo);

            }
            else if (eventName == "BackToItemList")
            {
                var player = sender.GetAccountEntity();
                string itemsJson = JsonConvert.SerializeObject(player.CharacterEntity.DbModel.Items.ToList());
                sender.TriggerEvent("ShowItems", itemsJson);
            }
            //args[0] to numer na jaki dzwoni
            else if (eventName == "OnPlayerTelephoneCall")
            {
                var player = sender.GetAccountEntity();

                if (Convert.ToInt32(args[0]) == 555 && player.CharacterEntity.OnDutyGroup is CrimeGroup group)
                {
                    if (group.CanPlayerCallCrimeBot(player))
                    {
                        List<string> names = XmlHelper.GetXmlObjects<CrimeBotPosition>($@"{Constant.ServerInfo.XmlDirectory}CrimeBotPositions\").Select(n => n.Name).ToList();
                        sender.TriggerEvent("OnPlayerCalledCrimeBot", names);
                        return;
                    }
                }

                if (sender.GetAccountEntity().CharacterEntity.CurrentCellphone.CurrentCall != null)
                {
                    sender.Notify("Obecnie prowadzisz rozmowę telefoniczną. Zakończ ją klawiszem END.");
                    return;
                }
                ////animka dzwonienia przez telefon
                //NAPI.playPlayerAnimation(senderPlayer.Client, (int)(AnimationFlags.AllowPlayerControl | AnimationFlags.Loop),
                //    "cellphone@first_person", "cellphone_call_listen_base");

                var number = Convert.ToInt32(args[0]);
                if (EntityManager.GetAccounts().Any(p => p.Value.CharacterEntity.CurrentCellphone.Number == number))
                {
                    var getter = EntityManager.GetAccounts()
                        .Single(p => p.Value.CharacterEntity.CurrentCellphone.Number == number).Value;

                    if (getter.CharacterEntity.CurrentCellphone.CurrentCall != null)
                    {
                        sender.SendChatMessage("~#ffdb00~",
                            "Wybrany abonent prowadzi obecnie rozmowę, spróbuj później.");
                        return;
                    }

                    var call = new TelephoneCall(sender, getter.Client);
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
                var call = sender.GetAccountEntity().CharacterEntity.CurrentCellphone.CurrentCall;
                call?.Dispose();

                //TODO: Wyłączanie telefonu

            }
            //Rządanie otworzenia okienka telefonu
            else if (eventName == "OnPlayerPullCellphoneRequest")
            {
                var cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;
                sender.TriggerEvent("OnPlayerPulledCellphone", cellphone.Name,
                    JsonConvert.SerializeObject(cellphone.Contacts),
                    JsonConvert.SerializeObject(cellphone.Messages));
            }
            //Odebranie rozmowy
            else if (eventName == "OnPlayerCellphonePickUp")
            {
                var cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;
                TelephoneCall telephoneCall = cellphone.CurrentCall;

                if (telephoneCall.Getter.GetAccountEntity().CharacterEntity.CurrentCellphone.CurrentCall != null)
                {
                    telephoneCall.Getter.SendChatMessage("~#ffdb00~",
                        "Aby odebrać musisz zakończyć bieżące połączenie.");
                    return;
                }

                //NAPI.playPlayerAnimation(telephoneCall.Getter.Client, (int)(AnimationFlags.AllowPlayerControl),
                //    "cellphone@first_person", "cellphone_call_listen_base");

                telephoneCall.Open();

                telephoneCall.Getter.SendChatMessage("~#ffdb00~",
                    "Odebrano telefon, aby zakończyć rozmowę naciśnij klawisz END.");
                telephoneCall.Sender.SendChatMessage("~#ffdb00~",
                    "Rozmówca odebrał telefon, aby zakończyć rozmowę naciśnij klawisz END.");
            }
            else if (eventName == "OnPlayerCellphoneEnd")
            {
                var controller = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;

                if (controller?.CurrentCall != null)
                {
                    sender.GetAccountEntity().CharacterEntity.CurrentCellphone.CurrentCall = null;

                    NAPI.Chat.SendChatMessageToPlayer(controller.CurrentCall.Sender, "~#ffdb00~",
                        "Rozmowa zakończona.");
                    NAPI.Chat.SendChatMessageToPlayer(controller.CurrentCall.Getter, "~#ffdb00~",
                        "Rozmowa zakończona.");
                }
            }
            //args[0] to numer kontaktu args[1] to nazwa 
            else if (eventName == "OnPlayerTelephoneContactAdded")
            {
                var cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;
                if (cellphone == null)
                {
                    sender.Notify("Musisz mieć włączony telefon.");
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