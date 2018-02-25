/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using GTANetworkAPI;
using Serverside.Constant;
using Serverside.Core;
using Serverside.Core.Enums;
using Serverside.Core.Extensions;
using Serverside.Core.Scripts;
using Serverside.Entities.Base;
using Serverside.Entities.Common.Corners.EventArgs;
using Serverside.Entities.Core;
using Serverside.Entities.Core.Item;

namespace Serverside.Entities.Common.Corners
{
    public class CornerPedEntity : PedEntity
    {
        public int BotId { get; set; }
        public DrugType DrugType { get; set; }
        public decimal MoneyCount { get; set; }
        public string Greeting { get; set; }
        public string GoodFarewell { get; set; }
        public string BadFarewell { get; set; }

        private AccountEntity Seller { get; }
        private List<FullPosition> NextPositions { get; }

        public delegate void EndTransactionEventHandler(object sender, EndTransactionEventArgs e);
        public event EndTransactionEventHandler OnTransactionEnd;

        private List<decimal> LowerMoneyCounts
        {
            get
            {
                var lowerMoneyCounts = new List<decimal>();
                for (int i = 0; i < MoneyCount; i++)
                    lowerMoneyCounts.Add(i);

                return lowerMoneyCounts;
            }
        }
        private List<decimal> MostlyGoodMoneyCounts
        {
            get
            {
                var mostlyGoodMoneyCounts = new List<decimal>();
                for (int i = Convert.ToInt32(MoneyCount) + 1; i < MoneyCount + 21; i++)
                    mostlyGoodMoneyCounts.Add(i);

                return mostlyGoodMoneyCounts;
            }
        }

        public CornerPedEntity(string name, PedHash pedHash, FullPosition spawnPosition, List<FullPosition> nextPositions, DrugType drugType, decimal moneyCount, string greeting, string goodFarewell, string badFarewell, AccountEntity seller, int botId) : base(name, pedHash, spawnPosition)
        {
            BotId = botId;
            NextPositions = nextPositions;
            DrugType = drugType;
            MoneyCount = moneyCount;
            Greeting = greeting;
            GoodFarewell = goodFarewell;
            BadFarewell = badFarewell;
            Seller = seller;
        }

        public void IntializeProcess()
        {
            GoAllPoints(false);
        }

        #region Interakcja z botem
        private void CornerPlayerSaidHandler(object sender, SaidEventArgs e)
        {
            //Brak TransactionLevel bot przychodzi i pyta o narkotyk
            //TransactionLevel == 1 PedEntity czeka na cenę
            //TransactionLevel == 2 Podana cena była za wysoka i bot wynegocjował zgodnie z tym co może dać, czeka aż gracz powie tak lub poda cenę

            //Jeśli gracz powie że nie ma
            if ((!BotHandle.HasData("TransactionLevel") || BotHandle.GetData("TransactionLevel") == 2) && e.Player == Seller.Client && (e.ChatMessageType == ChatMessageType.Normal || e.ChatMessageType == ChatMessageType.Quiet || e.ChatMessageType == ChatMessageType.Loud) && Messages.NoMessagesList.Any(e.Message.Contains))
            {
                GoAllPoints(true);
            }
            else if (BotHandle.HasData("TransactionLevel") && BotHandle.GetData("TransactionLevel") == 1 && e.Player == Seller.Client && (e.ChatMessageType == ChatMessageType.Normal || e.ChatMessageType == ChatMessageType.Quiet || e.ChatMessageType == ChatMessageType.Loud) && !e.Message.All(char.IsDigit))
            {
                //Jeśli gracz nie napisze zadnej liczby
                Seller.Client.Notify("Aby podać cenę kupującemu NPC musisz używać liczb np. 70.");
            }
            //Jeśli gracz powie tak
            else if (!BotHandle.HasData("TransactionLevel") && e.Player == Seller.Client && (e.ChatMessageType == ChatMessageType.Normal || e.ChatMessageType == ChatMessageType.Quiet || e.ChatMessageType == ChatMessageType.Loud) && Messages.YesMessagesList.Any(e.Message.Contains))
            {
                BotHandle.SetData("TransactionLevel", 1);
                SendMessageToNerbyPlayers("Ile za to cudo?", ChatMessageType.Normal);
            }
            //Jeśli gracz poda za wysoką cenę, ale w granicach rozsądku
            else if (BotHandle.HasData("TransactionLevel") && BotHandle.GetData("TransactionLevel") == 1 && e.Player == Seller.Client && (e.ChatMessageType == ChatMessageType.Normal || e.ChatMessageType == ChatMessageType.Quiet || e.ChatMessageType == ChatMessageType.Loud) && MostlyGoodMoneyCounts.Any(Convert.ToDecimal(e.Message).Equals))
            {
                SendMessageToNerbyPlayers($"Co powiesz na ${MoneyCount}?", ChatMessageType.Normal);
                BotHandle.SetData("TransactionLevel", 2);
            }
            //Jeśli gracz poda właściwą lub niższą cenę
            else if (BotHandle.HasData("TransactionLevel") && BotHandle.GetData("TransactionLevel") == 1 && e.Player == Seller.Client && (e.ChatMessageType == ChatMessageType.Normal || e.ChatMessageType == ChatMessageType.Quiet || e.ChatMessageType == ChatMessageType.Loud) && (e.Message.Contains(MoneyCount.ToString(CultureInfo.InvariantCulture)) || LowerMoneyCounts.Any(Convert.ToDecimal(e.Message).Equals)))
            {
                //Sprawdzamy czy gracz posiada dany narkotyk
                if (Seller.CharacterEntity.DbModel.Items.Any(i => i.ItemType == ItemType.Drug && i.FirstParameter == (int)DrugType))
                {
                    EndTransaction(LowerMoneyCounts.Any(Convert.ToDecimal(e.Message).Equals) ? LowerMoneyCounts.First(Convert.ToDecimal(e.Message).Equals) : MoneyCount);
                }
                //Jeśli gracz nie ma narkotyku
                else
                {
                    SendFailMessage();
                }
                GoAllPoints(true);
            }
            //Po negocjacji
            else if (BotHandle.HasData("TransactionLevel") && BotHandle.GetData("TransactionLevel") == 2 && e.Player == Seller.Client && (e.ChatMessageType == ChatMessageType.Normal || e.ChatMessageType == ChatMessageType.Quiet || e.ChatMessageType == ChatMessageType.Loud) && (Messages.YesMessagesList.Any(e.Message.Contains) || e.Message.Contains(MoneyCount.ToString(CultureInfo.InvariantCulture)) || LowerMoneyCounts.Any(Convert.ToDecimal(e.Message).Equals)))
            {
                //Jeśli gracz zgodzi się na cenę bota
                //Sprawdzamy czy gracz posiada dany narkotyk
                if (Seller.CharacterEntity.DbModel.Items.Any(i => i.ItemType == ItemType.Drug && i.FirstParameter == (int)DrugType))
                {
                    if (!e.Message.All(char.IsDigit)) EndTransaction(MoneyCount);
                    else EndTransaction(LowerMoneyCounts.Any(Convert.ToDecimal(e.Message).Equals) ? LowerMoneyCounts.First(Convert.ToDecimal(e.Message).Equals) : MoneyCount);
                }
                //Jeśli gracz nie ma narkotyku po negocjacji
                else
                {
                    SendFailMessage();
                }
                GoAllPoints(true);
            }
            else
            {
                SendMessageToNerbyPlayers(BadFarewell, ChatMessageType.Normal);
                GoAllPoints(true);
            }
        }
        #endregion

        private void SendFailMessage()
        {
            ChatScript.SendMessageToNearbyPlayers(Seller.Client, "szuka narkotyku w swoim otoczeniu, błądzi wzrokiem.", ChatMessageType.ServerMe);
            SendMessageToNerbyPlayers($"popatrzył się na {Seller.CharacterEntity.FormatName} jak na debila.", ChatMessageType.ServerMe);
        }

        private void EndTransaction(decimal money)
        {
            SendMessageToNerbyPlayers("wystawia dyskretnie dłoń z gotówką i odbiera narkotyk.", ChatMessageType.Me);
            SendMessageToNerbyPlayers(GoodFarewell, ChatMessageType.Normal);

            Seller.CharacterEntity.DbModel.Items.Remove(
                Seller.CharacterEntity.DbModel.Items.First(x => x.ItemType == ItemType.Drug && x.FirstParameter == (int)DrugType));
            Seller.CharacterEntity.Save();
            Seller.Client.AddMoney(money);

            GoAllPoints(true);
        }

        private void GoAllPoints(bool end)
        {
            if (end)
            {
                ChatScript.OnPlayerSaid -= CornerPlayerSaidHandler;
                EndTransactionEventHandler handler = OnTransactionEnd;
                EndTransactionEventArgs eventArgs = new EndTransactionEventArgs(false);
                handler?.Invoke(this, eventArgs);
                return;
            }

            foreach (var pos in NextPositions)
            {
                GoToPoint(pos.Position);
                while (true)
                {
                    if (BotHandle.Position == pos.Position) break;
                    Task.Delay(100).Wait();
                }
            }
            SendMessageToNerbyPlayers(Greeting, ChatMessageType.Normal);
            ChatScript.OnPlayerSaid += CornerPlayerSaidHandler;
        }

        public override void Spawn()
        {
            base.Spawn();
            IntializeProcess();
        }

        public override void Dispose()
        {
            GoToPoint(SpawnPosition.Position);
            Task.Delay(1000).Wait();
            base.Dispose();
        }
    }
}
