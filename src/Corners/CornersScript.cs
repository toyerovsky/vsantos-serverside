/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using Serverside.Core;
using Serverside.Core.Extensions;
using Serverside.Core.Serialization.Xml;
using Serverside.Corners.Helpers;
using Serverside.Corners.Models;

namespace Serverside.Corners
{
    public class CornersScript : Script
    {
        private List<Corner> Corners { get; set; } = new List<Corner>();

        public CornersScript()
        {
            Event.OnResourceStart += OnResourceStart;
        }

        private void OnResourceStart()
        {
            foreach (var corner in XmlHelper.GetXmlObjects<CornerModel>(Path.Combine(Constant.ServerInfo.XmlDirectory, "Corners")))
            {
                Corners.Add(new Corner(Event, corner));
            }
        }

        #region ADMIN COMMANDS
        [Command("dodajrog", "~y~UŻYJ: ~w~ /dodajrog [id npc np: 1, 2, 8, 4, 5]", GreedyArg = true)]
        public void AddCorner(Client sender, string ids)
        {
            var botIds = ids.Split(',').ToList();
            List<int> correctBotIds;

            //Sprawdzamy czy gracz podał prawidłowe ID NPC
            if (!CornerBotHelper.TryGetCornerBotIds(botIds, out correctBotIds))
            {
                sender.Notify("Podano dane w nieprawidłowym formacie. Lub podany NPC nie istnieje.");
                return;
            }

            sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\".");
            sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");

            FullPosition position = null;
            List<FullPosition> botPositions = new List<FullPosition>();

            Event.OnChatMessage += Handler;

            void Handler(Client o, string message, CancelEventArgs cancel)
            {
                if (position == null && o == sender && message == "tu")
                {
                    cancel.Cancel = true;
                    position = new FullPosition
                    {
                        Position = new Vector3
                        {
                            X = sender.Position.X,
                            Y = sender.Position.Y,
                            Z = sender.Position.Z - 0.5f
                        },

                        Rotation = new Vector3
                        {
                            X = sender.Rotation.X,
                            Y = sender.Rotation.Y,
                            Z = sender.Rotation.Z
                        }
                    };

                    sender.Notify("Wyznacz trasę npc na rogu. Ustaw się w danym punkcie i wpisz \"poz\".");
                    sender.Notify("Aby zacząć od nowa wpisz \"reset\"");
                    sender.Notify("Aby usunąć ostatnią pozycję wpisz \"usun\"");
                }
                else if (position != null && o == sender && message == "/poz")
                {
                    cancel.Cancel = true;
                    botPositions.Add(new FullPosition
                    {
                        Position = new Vector3
                        {
                            X = sender.Position.X,
                            Y = sender.Position.Y,
                            Z = sender.Position.Z
                        },

                        Rotation = new Vector3
                        {
                            X = sender.Rotation.X,
                            Y = sender.Rotation.Y,
                            Z = sender.Rotation.Z
                        }
                    });

                    sender.Notify($"Obecna liczba punktów: {botPositions.Count}. Aby zakończyć wpisz \"zakoncz\"");
                    sender.Notify("Aby zacząć od nowa wpisz /reset");
                }
                else if (position != null && botPositions.Count != 0 && o == sender && message == "zakoncz")
                {
                    CornerModel corner = new CornerModel
                    {
                        CreatorForumName = o.GetAccountEntity().DbModel.Name,
                        Position = position,
                        CornerBots = XmlHelper.GetXmlObjects<CornerBotModel>(Constant.ServerInfo.XmlDirectory + @"CornerBots\").Where(e => correctBotIds.Contains(e.BotId)).ToList(),
                        BotPositions = botPositions
                    };
                    //Dodajemy nowy plik .xml
                    XmlHelper.AddXmlObject(corner, Constant.ServerInfo.XmlDirectory + @"Corners\");
                    Corners.Add(new Corner(Event, corner));

                    sender.Notify("Dodawanie rogu zakończyło się ~h~~g~pomyślnie.");
                    Event.OnChatMessage -= Handler;
                }
                else if (botPositions.Count != 0 && position != null && o == sender && message == "usun")
                {
                    botPositions.RemoveAt(botPositions.Count);
                }
                else if (botPositions.Count != 0 && position != null && o == sender && message == "reset")
                {
                    position = null;
                    botPositions = new List<FullPosition>();
                    sender.Notify("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\".");
                    sender.Notify("...użyj /diag aby poznać swoją obecną pozycję.");
                }
            };
        }

        [Command("usunrog")]
        public void DeleteCorner(Client sender)
        {
            var corner = Corners.OrderBy(a => a.Data.Position.Position.DistanceTo(sender.Position)).First();
            if (XmlHelper.TryDeleteXmlObject(corner.Data.FilePath))
            {
                sender.Notify("Usuwanie rogu zakończyło się ~h~~g~pomyślnie.");
                Corners.Remove(corner);
                corner.Dispose();
            }
            else
            {
                sender.Notify("Usuwanie rogu zakończyło się ~h~~r~niepomyślnie.");
            }
        }
        #endregion
    }
}
