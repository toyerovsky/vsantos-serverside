/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Drawing;
using System.Linq;
using GTANetworkInternals;
using Serverside.Core.Database.Models;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Repositories;
using Serverside.Core.Scripts;
using Serverside.Core.Telephone;
using Serverside.Entities.Core;

namespace Serverside.Items
{
    internal class Cellphone : Item
    {
        public int PossibleContactsToSave { get; }
        public int PossibleMessagesToSave { get; }
        public int Number { get; }
        public CellphoneVisibleType VisibleType { get; }

        public TelephoneCall CurrentCall { get; set; }

        public ObservableCollection<TelephoneContactModel> Contacts { get; }
        public ObservableCollection<TelephoneMessageModel> Messages { get; }

        /// <summary>
        /// 1 parametr to liczba kontaktów do zapisania, 
        /// 2 to liczba sms możliwych do zapisania, 
        /// 3 to numer telefonu
        /// 4 to wygląd
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Cellphone(EventClass events, ItemModel itemModel) : base(events, itemModel)
        {
            PossibleContactsToSave = itemModel.FirstParameter.Value;
            PossibleMessagesToSave = itemModel.SecondParameter.Value;
            Number = itemModel.ThirdParameter.Value;
            VisibleType = (CellphoneVisibleType)itemModel.FourthParameter.Value;

            using (TelephoneMessagesRepository repository = new TelephoneMessagesRepository())
            {
                Messages = new ObservableCollection<TelephoneMessageModel>(repository.GetAll().Where(m => m.Cellphone.Id == Id));
            }

            using (TelephoneContactsRepository repository = new TelephoneContactsRepository())
                Contacts = new ObservableCollection<TelephoneContactModel>(repository.GetAll().Where(m => m.Cellphone.Id == Id).ToList());

            Messages.CollectionChanged += Messages_CollectionChanged;
            Contacts.CollectionChanged += Contacts_CollectionChanged;
        }

        public override void UseItem(AccountEntity player)
        {
            if (!DbModel.CurrentlyInUse)
            {
                if (!DbModel.ThirdParameter.HasValue)
                    Colorful.Console.WriteLine($"[ERROR] Do numeru telefonu o ID {DbModel.Id} został przypisany null.", Color.DarkRed);
                else
                {
                    ChatScript.SendMessageToNearbyPlayers(player.Client, $"włącza {DbModel.Name}", ChatMessageType.ServerMe);
                    player.Client.SetSharedData("CellphoneID", (int)DbModel.Id);
                    player.Client.Notify($"Telefon {DbModel.Name} został włączony naciśnij klawisz END, aby go używać.");
                    DbModel.CurrentlyInUse = true;
                    player.CharacterEntity.Save();

                    player.CharacterEntity.CurrentCellphone = this;
                }
            }
            else
            {
                ChatScript.SendMessageToNearbyPlayers(player.Client, $"wyłącza {DbModel.Name}", ChatMessageType.ServerMe);

                player.CharacterEntity.Save();
                player.CharacterEntity.CurrentCellphone = null;
                DbModel.CurrentlyInUse = false;
                player.Client.Notify($"Telefon {DbModel.Name} został wyłączony.");

            }
        }

        public override string UseInfo => $"Telefon: {DbModel.Name} może przechowywać: {DbModel.FirstParameter} kontaktów, oraz {DbModel.SecondParameter} wiadomości SMS.";

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (TelephoneMessagesRepository repository = new TelephoneMessagesRepository())
            {
                foreach (var message in e.NewItems.Cast<TelephoneMessageModel>().Where(m => !e.OldItems.Contains(m)))
                    repository.Insert(message);

                foreach (var message in e.OldItems.Cast<TelephoneMessageModel>().Where(m => !e.NewItems.Contains(m)))
                    repository.Delete(message.Id);

                repository.Save();
            }
        }

        private void Contacts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (TelephoneContactsRepository repository = new TelephoneContactsRepository())
            {
                foreach (var contact in e.NewItems.Cast<TelephoneContactModel>().Where(m => !e.OldItems.Contains(m)))
                    repository.Insert(contact);

                foreach (var contact in e.OldItems.Cast<TelephoneContactModel>().Where(m => !e.NewItems.Contains(m)))
                    repository.Delete(contact.Id);

                repository.Save();
            }
        }
    }
}