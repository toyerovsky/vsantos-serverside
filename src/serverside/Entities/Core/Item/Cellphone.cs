/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using VRP.Core.Database.Models;
using VRP.Core.Database.Models.Item;
using VRP.Core.Database.Models.Telephone;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Serverside.Core.Scripts;
using VRP.Serverside.Core.Telephone;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Cellphone : ItemEntity
    {
        public int PossibleContactsToSave => DbModel.FirstParameter.Value;
        public int PossibleMessagesToSave => DbModel.SecondParameter.Value;
        public int Number => DbModel.ThirdParameter.Value;
        public CellphoneVisibleType VisibleType => (CellphoneVisibleType)DbModel.FourthParameter.Value;

        public TelephoneCall CurrentCall { get; set; }

        public ObservableCollection<TelephoneContactModel> Contacts { get; }
        public ObservableCollection<TelephoneMessageModel> Messages { get; }

        /// <summary>
        /// 1 parametr to liczba kontaktów do zapisania, 
        /// 2 to liczba sms możliwych do zapisania, 
        /// 3 to numer telefonu
        /// 4 to wygląd
        /// </summary>
        /// <param name="itemModel"></param>
        public Cellphone(ItemModel itemModel) : base(itemModel)
        {
            using (TelephoneMessagesRepository repository = new TelephoneMessagesRepository())
                Messages = new ObservableCollection<TelephoneMessageModel>(repository.GetAll().Where(m => m.Cellphone.Id == Id));


            using (TelephoneContactsRepository repository = new TelephoneContactsRepository())
                Contacts = new ObservableCollection<TelephoneContactModel>(repository.GetAll().Where(m => m.Cellphone.Id == Id));

            Messages.CollectionChanged += Messages_CollectionChanged;
            Contacts.CollectionChanged += Contacts_CollectionChanged;
        }

        public override void UseItem(CharacterEntity character)
        {
            if (character.ItemsInUse.Any(item => ReferenceEquals(item, this)))
            {
                character.ItemsInUse.Remove(this);
                Save();
                ChatScript.SendMessageToNearbyPlayers(character, $"wyłącza {DbModel.Name}", ChatMessageType.ServerMe);
                character.SendInfo($"Telefon {DbModel.Name} został wyłączony.");
            }
            else if (character.ItemsInUse.All(item => !(item is Cellphone)))
            {
                character.ItemsInUse.Add(this);
                ChatScript.SendMessageToNearbyPlayers(character, $"włącza {DbModel.Name}", ChatMessageType.ServerMe);
                character.SendInfo($"Telefon {DbModel.Name} został włączony naciśnij klawisz END, aby go używać.");
            }
        }

        public override string UseInfo => $"Telefon: {DbModel.Name} może przechowywać: {DbModel.FirstParameter} kontaktów, oraz {DbModel.SecondParameter} wiadomości SMS.";

        private void Messages_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (TelephoneMessagesRepository repository = new TelephoneMessagesRepository())
            {
                foreach (TelephoneMessageModel message in e.NewItems.Cast<TelephoneMessageModel>().Where(m => !e.OldItems.Contains(m)))
                    repository.Insert(message);

                foreach (TelephoneMessageModel message in e.OldItems.Cast<TelephoneMessageModel>().Where(m => !e.NewItems.Contains(m)))
                    repository.Delete(message.Id);

                repository.Save();
            }
        }

        private void Contacts_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            using (TelephoneContactsRepository repository = new TelephoneContactsRepository())
            {
                foreach (TelephoneContactModel contact in e.NewItems.Cast<TelephoneContactModel>().Where(m => !e.OldItems.Contains(m)))
                    repository.Insert(contact);

                foreach (TelephoneContactModel contact in e.OldItems.Cast<TelephoneContactModel>().Where(m => !e.NewItems.Contains(m)))
                    repository.Delete(contact.Id);

                repository.Save();
            }
        }
    }
}