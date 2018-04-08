/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
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
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Entities.Core.Item.Scripts
{
    public class ItemsScript : Script
    {
        private ItemEntityFactory _itemFactory { get; } = new ItemEntityFactory();

        [RemoteEvent(RemoteEvents.SelectedItem)]
        public void SelectedItemHandler(Client sender, params object[] args)
        {
            int index = Convert.ToInt32(args[0]);
            sender.SetData("SelectedItem", index);
            sender.TriggerEvent("SelectOptionItem", index);
        }

        [RemoteEvent(RemoteEvents.UseItem)]
        public void UseItemHandler(Client sender, params object[] args)
        {
            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            int index = Convert.ToInt32(sender.GetData("SelectedItem"));

            ItemEntity item = _itemFactory.Create(character.DbModel.Items.ToList()[index]);
            item.UseItem(character);
        }

        [RemoteEvent(RemoteEvents.InformationsItem)]
        public void InformationsItemHandler(Client sender, params object[] args)
        {
            AccountEntity player = sender.GetAccountEntity();

            int index = Convert.ToInt32(sender.GetData("SelectedItem"));
            List<ItemModel> userItems = player.CharacterEntity.DbModel.Items.ToList();

            ItemEntity item = _itemFactory.Create(userItems[index]);
            sender.SendInfo(item.ItemInfo);
        }

        [RemoteEvent(RemoteEvents.UsingInformationsItem)]
        public void UsingInformationsItemHandler(Client sender, params object[] args)
        {
            AccountEntity player = sender.GetAccountEntity();

            int index = Convert.ToInt32(sender.GetData("SelectedItem"));
            List<ItemModel> userItems = player.CharacterEntity.DbModel.Items.ToList();

            ItemEntity item = _itemFactory.Create(userItems[index]);
            sender.SendInfo(item.UseInfo);
        }

        [RemoteEvent(RemoteEvents.BackToItemList)]
        public void BackToItemListHandler(Client sender, params object[] args)
        {
            AccountEntity player = sender.GetAccountEntity();
            string itemsJson = JsonConvert.SerializeObject(player.CharacterEntity.DbModel.Items.ToList());
            sender.TriggerEvent("ShowItems", itemsJson);
        }

        [RemoteEvent(RemoteEvents.OnPlayerTelephoneCall)]
        public void OnPlayerTelephoneCallHandler(Client sender, params object[] args)
        {

            //args[0] to numer na jaki dzwoni
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
                sender.SendInfo("Obecnie prowadzisz rozmowę telefoniczną. Zakończ ją klawiszem END.");
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

        [RemoteEvent(RemoteEvents.OnPlayerTelephoneTurnoff)]
        public void OnPlayerTelephoneTurnoffHandler(Client sender, params object[] args)
        {
            TelephoneCall call = sender.GetAccountEntity().CharacterEntity.CurrentCellphone.CurrentCall;
            call?.Dispose();

            //TODO: Wyłączanie telefonu
        }

        //Rządanie otworzenia okienka telefonu
        [RemoteEvent(RemoteEvents.OnPlayerPullCellphoneRequest)]
        public void OnPlayerPullCellphoneRequestHandler(Client sender, params object[] args)
        {
            Cellphone cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;
            sender.TriggerEvent("OnPlayerPulledCellphone", cellphone.Name,
                JsonConvert.SerializeObject(cellphone.Contacts),
                JsonConvert.SerializeObject(cellphone.Messages));
        }
        //Odebranie rozmowy
        [RemoteEvent(RemoteEvents.OnPlayerCellphonePickUp)]
        public void OnPlayerCellphonePickUpHandler(Client sender, params object[] args)
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

        [RemoteEvent(RemoteEvents.OnPlayerCellphoneEnd)]
        public void OnPlayerCellphoneEndHandler(Client sender, params object[] args)
        {
            Cellphone cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;

            if (cellphone?.CurrentCall != null)
            {
                cellphone.CurrentCall.Sender.SendInfo("Rozmowa zakończona.");
                cellphone.CurrentCall.Getter.SendInfo("Rozmowa zakończona.");

                cellphone.CurrentCall = null;
            }
        }

        [RemoteEvent(RemoteEvents.OnPlayerTelephoneContactAdded)]
        public void OnPlayerTelephoneContactAddedHandler(Client sender, params object[] args)
        {
            //args[0] to numer kontaktu args[1] to nazwa 
            Cellphone cellphone = sender.GetAccountEntity().CharacterEntity.CurrentCellphone;
            if (cellphone == null)
            {
                sender.SendError("Musisz mieć włączony telefon.");
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