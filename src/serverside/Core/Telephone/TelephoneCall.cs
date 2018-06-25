/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.DAL.Database.Models.Telephone;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Scripts;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Core.Telephone
{
    public class TelephoneCall : IDisposable
    {
        public CharacterEntity Sender { get; set; }
        public CharacterEntity Getter { get; set; }

        public int BoothNumber { get; }

        public bool Accepted { get; set; }

        public Timer Timer { get; }

        private bool _currentlyTalking;
        public bool CurrentlyTalking
        {
            get => _currentlyTalking;
            set
            {
                _currentlyTalking = value;
                if (value)
                {
                    Sender.ResetSharedData("CellphoneRinging");
                    Getter.ResetSharedData("CellphoneRinging");
                    Sender.SetSharedData("CellphoneTalking", true);
                    Getter.SetSharedData("CellphoneTalking", true);
                }
                else
                {
                    Sender.ResetSharedData("CellphoneTalking");
                    Getter.ResetSharedData("CellphoneTalking");
                    Sender.ResetSharedData("CellphoneRinging");
                    Getter.ResetSharedData("CellphoneRinging");
                }
            }
        }

        //Połączenie z komórki
        public TelephoneCall(CharacterEntity sender, CharacterEntity getter)
        {
            //gracz który dokonuje połączenia
            Sender = sender;
            //Gracz który odbiera połączenie
            Getter = getter;

            Accepted = false;

            //W tym miejscu jeśli gracz nie odbierze telefonu w 10 sekund to przerywa rozmowę.
            Timer = new Timer(10000);
            Timer.Start();

            ChatScript.OnPlayerSaid += RPChat_PlayerSaid;
            Getter.SetSharedData("CellphoneRinging", true);
            ChatScript.SendMessageToNearbyPlayers(Getter, $"dzwoni telefon {Getter.FormatName}", ChatMessageType.Do);

            ObservableCollection<TelephoneContactModel> contacts = Getter.CurrentCellphone.Contacts;

            string name = contacts.Any(c => c.Number == Sender.CurrentCellphone.Number)
                ? contacts.First(c => c.Number == Sender.CurrentCellphone.Number).Name
                : Sender.CurrentCellphone.Number.ToString();

            NAPI.Chat.SendChatMessageToPlayer(Getter.AccountEntity.Client, "~#ffdb00~",
                $"Połączenie przychodzące od: {name}, naciśnij klawisz END aby, akceptować połączenie.");

            // FixMe dzwonek telefonu
        }

        //Połączenie z budki
        public TelephoneCall(CharacterEntity sender, CharacterEntity getter, int number)
        {
            //gracz który dokonuje połączenia
            Sender = sender;
            //Gracz który odbiera połączenie
            Getter = getter;

            BoothNumber = number;
            Accepted = false;

            Timer = new Timer(20000);
            Timer.Start();

            ChatScript.OnPlayerSaid += RPChat_PlayerSaid;
            Getter.SetSharedData("CellphoneRinging", true);
            ChatScript.SendMessageToNearbyPlayers(Getter, $"dzwoni telefon {Getter.FormatName}", ChatMessageType.Do);

            ObservableCollection<TelephoneContactModel> contacts = Getter.CurrentCellphone.Contacts;

            string name = contacts.Any(c => c.Number == Sender.CurrentCellphone.Number)
                ? contacts.First(c => c.Number == Sender.CurrentCellphone.Number).Name
                : number.ToString();

            NAPI.Chat.SendChatMessageToPlayer(Getter.AccountEntity.Client, "~#ffdb00~",
                $"Połączenie przychodzące od: {name}, naciśnij klawisz END aby, akceptować połączenie.");

        }

        public void Open()
        {
            Accepted = true;
            CurrentlyTalking = true;
            Timer.Stop();
        }

        public void Dispose()
        {
            Accepted = false;
            CurrentlyTalking = false;

            ChatScript.OnPlayerSaid -= RPChat_PlayerSaid;
            Timer.Dispose();

            NAPI.Player.StopPlayerAnimation(Sender.AccountEntity.Client);
            NAPI.Player.StopPlayerAnimation(Getter.AccountEntity.Client);
        }

        private void RPChat_PlayerSaid(object s, SaidEventArgs e)
        {
            if (Accepted && e.Character == Sender)
            {
                ChatScript.SendMessageToPlayer(Getter, e.Message, ChatMessageType.Phone);
            }
            else if (Accepted && e.Character == Getter)
            {
                ChatScript.SendMessageToPlayer(Sender, e.Message, ChatMessageType.Phone);
            }
        }
    }
}