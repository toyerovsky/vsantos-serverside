/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;

namespace Serverside.Core.Telephone
{
    public class TelephoneCall : IDisposable
    {
        public Client Sender { get; set; }
        public Client Getter { get; set; }

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
        public TelephoneCall(Client sender, Client getter)
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
            ChatScript.SendMessageToNearbyPlayers(Getter, $"dzwoni telefon {Getter.Name}", ChatMessageType.Do);

            var senderCellphone = Sender.GetAccountEntity().CharacterEntity.CurrentCellphone;
            var getterCellphone = getter.GetAccountEntity().CharacterEntity.CurrentCellphone;
            var contacts = getterCellphone.Contacts;

            NAPI.Chat.SendChatMessageToPlayer(Getter, "~#ffdb00~", contacts.Any(c => c.Number == senderCellphone.Number) ? $"Połączenie przychodzące od: {contacts.First(c => c.Number == senderCellphone.Number).Name}, naciśnij klawisz END aby, akceptować połączenie."
                : $"Połączenie przychodzące od: {senderCellphone.Number}, naciśnij klawisz END aby, akceptować połączenie.");
        }

        //Połączenie z budki
        public TelephoneCall(Client sender, Client getter, int number)
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
            ChatScript.SendMessageToNearbyPlayers(Getter, $"dzwoni telefon {Getter.Name}", ChatMessageType.Do);

            var telephone = Getter.GetAccountEntity().CharacterEntity.CurrentCellphone;
            var contacts = telephone.Contacts;

            NAPI.Chat.SendChatMessageToPlayer(Getter, "~#ffdb00~", contacts.Any(c => c.Number == number) ? $"Połączenie przychodzące od: {contacts.First(c => c.Number == number).Name}, naciśnij klawisz END aby, akceptować połączenie."
                : $"Połączenie przychodzące od: {number}, naciśnij klawisz END aby, akceptować połączenie.");

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

            NAPI.Player.StopPlayerAnimation(Sender);
            NAPI.Player.StopPlayerAnimation(Getter);
        }

        private void RPChat_PlayerSaid(object s, SaidEventArgs e)
        {
            if (Accepted && e.Player == Sender)
            {
                ChatScript.SendMessageToPlayer(Getter, e.Message, ChatMessageType.Phone);
            }
            else if (Accepted && e.Player == Getter)
            {
                ChatScript.SendMessageToPlayer(Sender, e.Message, ChatMessageType.Phone);
            }
        }
    }
}