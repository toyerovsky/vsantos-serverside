/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using GTANetworkAPI;
using Newtonsoft.Json;
using VRP.Core;
using VRP.Core.Enums;
using VRP.DAL.Database;
using VRP.DAL.Enums;
using VRP.DAL.Repositories;
using VRP.Serverside.Core.Extensions;

namespace VRP.Serverside.Entities.Core.Building
{
    public class BuildingScript : Script
    {
        private void API_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            switch (eventName)
            {
                case "PassDoors":
                    BuildingEntity.PassDoors(sender);
                    break;
                case "KnockDoors":
                    BuildingEntity.Knock(sender);
                    break;
                case "BuyBuilding":
                    if (!sender.HasData("CurrentDoors")) return;
                    BuildingEntity buildingEntity = sender.GetData("CurrentDoors");
                    buildingEntity.Buy(sender);
                    break;
                case "AddBuilding":
                    {
                        /* Arguments
                         * args[0] string Name 
                         * args[1] decimal cost
                         * args[2] string interiorName
                         * args[3] bool spawnPossible
                         */

                        if (Constant.Items.DefaultInteriors.All(i => i.Name != (string)arguments[2]) ||
                            !sender.HasData("AdminDoorPosition")) return;

                        Vector3 internalPosition = Constant.Items.DefaultInteriors
                            .First(i => i.Name == (string)arguments[2]).InternalPosition;
                        Vector3 externalPosition = sender.GetData("AdminDoorPosition");

                        BuildingEntity building = BuildingEntity.Create(
                            sender.GetAccountEntity().DbModel,
                            (string)arguments[0],
                            Convert.ToDecimal(arguments[1]),
                            internalPosition, externalPosition,
                            (bool)arguments[3]);

                        building.Save();

                        sender.SendInfo("Dodawanie budynku zakończyło się pomyślnie.");
                        sender.Position = externalPosition;
                        sender.Dimension = (uint)Dimension.Global;
                        break;
                    }
                case "EdingBuildingInfo":
                    {
                        /* Arguments
                         * args[0] Name 
                         * args[1] description
                         * args[2] enterCharge
                         */

                        BuildingEntity building = sender.HasData("CurrentDoors")
                            ? sender.GetData("CurrentDoors")
                            : sender.GetAccountEntity().CharacterEntity.CurrentBuilding;
                        building.DbModel.Name = arguments[0].ToString();
                        building.DbModel.Description = arguments[1].ToString();
                        if (decimal.TryParse(arguments[2].ToString(), out decimal result))
                            building.DbModel.EnterCharge = result;
                        building.Save();
                        break;
                    }
            }
        }

        #region PLAYER COMMANDS

        [Command("drzwizamknij")]
        public void ChangeBuildingLockState(Client sender)
        {
            if (!sender.HasData("CurrentDoors"))
            {
                sender.SendError("Nie znajdujesz się przy drzwiach.");
                return;
            }

            BuildingEntity building = sender.GetData("CurrentDoors");

            //TODO: Dodanie zeby pracownicy mogli otwierać budynki grupowe zgodnie z uprawnieniami
            if (building.DbModel.Character == null ||
                building.DbModel.Character.Id != sender.GetAccountEntity().CharacterEntity.DbModel.Id)
            {
                sender.SendError("Nie jesteś właścicielem tego budynku.");
                return;
            }

            string text = building.DoorsLocked ? "otwarte" : "zamknięte";
            sender.SendInfo($"Drzwi zostały {text}");
            building.DoorsLocked = !building.DoorsLocked;
        }

        [Command("drzwi", "~y~UŻYJ ~w~ /drzwi")]
        public void ManageBuilding(Client sender, long id = -1)
        {
            //Dla administracji
            if (id != -1)
            {
                BuildingEntity buildindController = EntityHelper.GetBuilding(id);
                if (buildindController != null)
                {
                    List<string> adminInfo = new List<string>
                    {
                        buildindController.DbModel.Name,
                        buildindController.DbModel.Description,
                        buildindController.DbModel.EnterCharge.ToString()
                    };

                    sender.SetData("CurrentDoors", buildindController);
                    sender.TriggerEvent("ShowBuildingManagePanel", adminInfo);
                    return;
                }

                sender.SendError("Budynek o podanym Id nie istnieje.");
                return;
            }

            //CurrentBuilding jest po to żeby gracz mógł zarządzać budynkiem ze środka oraz do ofert
            if (sender.GetAccountEntity().CharacterEntity.CurrentBuilding != null ||
                sender.HasData("CurrentDoors"))
            {
                BuildingEntity building = sender.HasData("CurrentDoors")
                    ? sender.GetData("CurrentDoors")
                    : sender.GetAccountEntity().CharacterEntity.CurrentBuilding;

                //TODO: Dodanie, żeby właściciel grupy mógł zarządzać budynkiem grupowym
                if (building.DbModel.Character == null || building.DbModel.Character.Id !=
                    sender.GetAccountEntity().CharacterEntity.DbModel.Id)
                {
                    sender.SendError("Nie jesteś właścicielem tego budynku.");
                    return;
                }

                List<string> info = new List<string>
                {
                    building.DbModel.Name,
                    building.DbModel.Description,
                    building.DbModel.EnterCharge.ToString()
                };

                sender.TriggerEvent("ShowBuildingManagePanel", info);

            }
            else
            {
                sender.SendInfo(
                    "Aby otworzyć panel zarządzania budynkiem musisz znajdować się w markerze bądź środku budynku.");
            }
        }

        #endregion

        #region ADMIN COMMANDS

        [Command("usundrzwi", "~y~UŻYJ ~w~ /usundrzwi (id)")]
        public void DeleteBuilding(Client sender, long id = -1)
        {
            if (!sender.HasRank(ServerRank.AdministratorRozgrywki2))
            {
                sender.SendWarning("Nie posiadasz uprawnień do usuwania drzwi.");
                return;
            }

            if (id == -1 && !sender.HasData("CurrentDoors"))
            {
                sender.SendError("Aby usunąć budynek musisz podać jego Id, lub znajdować się w jego drzwiach.");
            }

            RoleplayContext ctx = Singletons.RoleplayContextFactory.Create();
            using (BuildingsRepository repository = new BuildingsRepository(ctx))
            {
                if (sender.HasData("CurrentDoors"))
                {
                    BuildingEntity building = (BuildingEntity)sender.GetData("CurrentDoors");
                    building.Dispose();
                    repository.Delete(building.DbModel.Id);
                    repository.Save();

                    return;
                }

                if (id != -1 && EntityHelper.GetBuilding(id) != null)
                {
                    BuildingEntity building = EntityHelper.GetBuilding(id);
                    building.Dispose();

                    repository.Delete(building.DbModel.Id);
                    repository.Save();
                }
            }
            sender.SendError("Podany budynek nie istnieje.");
        }


        [Command("dodajdrzwi")]
        public void CreateBuilding(Client sender)
        {
            if (!sender.HasRank(ServerRank.AdministratorRozgrywki4))
            {
                sender.SendWarning("Nie posiadasz uprawnień do tworzenia drzwi.");
                return;
            }

            sender.SendInfo("Ustaw się w pozycji markera, a następnie wpisz /tu użyj ctrl + alt + shift + d aby poznać swoją obecną pozycję.");

            void Handler(Client o, string message)
            {
                if (o == sender && message == "/tu")
                {
                    if (EntityHelper.GetBuildings().Any(b => b.BuildingMarker.Position.DistanceTo(o.Position) < 5))
                    {
                        sender.SendInfo("W bliskim otoczeniu tego budynku znajduje się już inny budynek.");
                        return;
                    }

                    o.SetData("AdminDoorPosition", o.Position);
                    sender.TriggerEvent("ShowAdminBuildingMenu", JsonConvert.SerializeObject(Constant.Items.DefaultInteriors));
                    sender.SendInfo("Dodawanie budynku zakończyło się pomyślnie.");
                }
            }
        }
        #endregion
    }
}