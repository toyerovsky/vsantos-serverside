/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Timers;
using GTANetworkAPI;
using VRP.Core.Database.Models;
using VRP.Core.Enums;
using VRP.Core.Repositories;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Core.Scripts;
using VRP.Serverside.Entities.Base;
using VRP.Serverside.Interfaces;

namespace VRP.Serverside.Entities.Core.Building
{
    public class BuildingEntity : GameEntity, IDbEntity<BuildingModel>, IOfferable
    {
        public BuildingModel DbModel { get; set; }

        public ColShape InteriorDoorsColshape { get; set; }
        public ColShape ExteriorDoorsColshape { get; set; }
        public Marker BuildingMarker { get; set; }

        public List<AccountEntity> PlayersInBuilding { get; set; } = new List<AccountEntity>();
        public bool DoorsLocked { get; set; } = true;

        public BuildingEntity(BuildingModel dbModel)
        {
            DbModel = dbModel;
            EntityHelper.Add(this);
        }

        public void Save()
        {
        }

        public override void Spawn()
        {
            InteriorDoorsColshape = NAPI.ColShape.CreateCylinderColShape(
                new Vector3(DbModel.InternalPickupPositionX, DbModel.InternalPickupPositionY, DbModel.InternalPickupPositionZ), 1, 3);
            InteriorDoorsColshape.Dimension = (uint)DbModel.InternalDimension;

            Vector3 externalPosition = new Vector3(DbModel.ExternalPickupPositionX,
                DbModel.ExternalPickupPositionY, DbModel.ExternalPickupPositionZ);

            ExteriorDoorsColshape = NAPI.ColShape.CreateCylinderColShape(externalPosition, 1, 3);
            ExteriorDoorsColshape.Dimension = (uint)DbModel.InternalDimension;

            Color color = DbModel.Cost.HasValue ? new Color(106, 154, 40, 255) : new Color(255, 255, 0, 255);

            //Jeśli budynek jest na sprzedaż marker jest zielony jeśli nie żółty
            BuildingMarker = NAPI.Marker.CreateMarker(2, externalPosition, new Vector3(0f, 0f, 0f),
                new Vector3(180f, 0f, 0f), 0.5f, color);

            InteriorDoorsColshape.OnEntityEnterColShape += (s, e) =>
            {
                if (NAPI.Entity.GetEntityType(e).Equals(EntityType.Player))
                {
                    Client player = NAPI.Player.GetPlayerFromHandle(e);
                    //args[0] jako true określa że gracz jest na zewnątrz budynku
                    //args[1] to informacje o budynku
                    player.TriggerEvent("DrawBuildingComponents", false);
                    player.SetData("CurrentDoors", this);

                    //Doors target jest potrzebne ponieważ Colshape nie ma pola Position... 
                    //dodatkowo przydaje się aby sprawdzać po stronie klienta czy gracz jest w jakimś budynku
                    //Gracz jest obecnie w zasięgu jakichś drzwi

                    player.SetSharedData("DoorsTarget", new Vector3(
                        DbModel.ExternalPickupPositionX, DbModel.ExternalPickupPositionY, DbModel.ExternalPickupPositionZ));
                }
            };

            InteriorDoorsColshape.OnEntityExitColShape += (s, e) =>
            {
                if (NAPI.Entity.GetEntityType(e).Equals(EntityType.Player))
                {
                    Client player = NAPI.Player.GetPlayerFromHandle(e);
                    player.TriggerEvent("DisposeBuildingComponents");
                    player.ResetData("CurrentDoors");
                    player.ResetSharedData("DoorsTarget");
                }
            };

            ExteriorDoorsColshape.OnEntityEnterColShape += (s, e) =>
            {
                //Jeśli podchodzi od zewnątrz rysujemy panel informacji
                if (NAPI.Entity.GetEntityType(e).Equals(EntityType.Player))
                {
                    Client player = NAPI.Player.GetPlayerFromHandle(e);
                    //args[0] jako true rysuje panel informacji
                    player.TriggerEvent("DrawBuildingComponents", true, new List<string>
                    {
                        DbModel.Name,
                        DbModel.Description,
                        DbModel.EnterCharge.HasValue ? DbModel.EnterCharge.ToString() : "",
                        DbModel.Cost.HasValue ? DbModel.Cost.ToString() : ""
                    });

                    player.SetData("CurrentDoors", this);
                    player.SetSharedData("DoorsTarget", new Vector3(DbModel.InternalPickupPositionX, DbModel.InternalPickupPositionY, DbModel.InternalPickupPositionZ));
                }
            };

            ExteriorDoorsColshape.OnEntityExitColShape += (s, e) =>
            {
                if (NAPI.Entity.GetEntityType(e).Equals(EntityType.Player))
                {
                    Client player = NAPI.Player.GetPlayerFromHandle(e);
                    player.TriggerEvent("DisposeBuildingComponents");
                    player.ResetData("CurrentDoors");
                    player.ResetSharedData("DoorsTarget");
                }
            };
        }

        public void Buy(Client sender)
        {
            if (!DbModel.Cost.HasValue)
            {
                sender.SendWarning("Ten budynek nie jest na sprzedaż");
                return;
            }

            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;

            if (!character.HasMoney(DbModel.Cost.Value))
            {
                sender.SendError("Nie posiadasz wystarczającej ilości gotówki");
                return;
            }

            BuildingMarker.Color = new Color(255, 255, 0);

            character.RemoveMoney(DbModel.Cost.Value);
            sender.TriggerEvent(Constant.RemoteEvents.RemoteEvents.CharacterShowShardRequested, "Zakupiono budynek", 5000);
            NAPI.Player.PlaySoundFrontEnd(sender, "BASE_JUMP_PASSED", "HUD_AWARDS");

            DbModel.Character = character.DbModel;
            DbModel.Cost = null;
            Save();
        }

        public void PutItemToBuilding(Client player, ItemModel itemModel)
        {
            DbModel.Items.Add(itemModel);
            itemModel.Building = DbModel;
            itemModel.Character = null;
            itemModel.Vehicle = null;
            Save();
        }

        public void Offer(CharacterEntity seller, CharacterEntity getter, decimal money)
        {
            throw new NotImplementedException();
        }

        public override void Dispose()
        {
            //Jeśli budynek zostanie zwolniony to teleportujemy graczy na zewnątrz
            foreach (AccountEntity p in PlayersInBuilding)
            {
                if (p.Client.HasData("CurrentDoors") && p.Client.GetData("CurrentDoors") == this)
                {
                    p.Client.ResetData("CurrentDoors");
                    p.Client.TriggerEvent("DisposeBuildingComponents");
                }

                if (p.CharacterEntity.CurrentBuilding == this)
                {
                    p.Client.Dimension = (uint)Dimension.Global;
                    p.Client.Position = p.CharacterEntity.CurrentBuilding.BuildingMarker.Position;
                    p.Client.SendWarning("Budynek w którym się znajdowałeś został usunięty.");
                    p.CharacterEntity.CurrentBuilding = null;
                }
            }
            NAPI.ColShape.DeleteColShape(InteriorDoorsColshape);
            NAPI.ColShape.DeleteColShape(ExteriorDoorsColshape);
            NAPI.Entity.DeleteEntity(BuildingMarker);
        }

        #region STATIC

        public static void PassDoors(Client sender)
        {
            if (!sender.HasData("CurrentDoors") || !sender.HasSharedData("DoorsTarget")) return;

            BuildingEntity buildingEntity = (BuildingEntity)sender.GetData("CurrentDoors");

            if (buildingEntity.DoorsLocked)
            {
                sender.SendWarning("Drzwi są zamknięte.");
                return;
            }

            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            if (buildingEntity.DbModel.EnterCharge.HasValue &&
                !character.HasMoney(buildingEntity.DbModel.EnterCharge.Value))
            {
                sender.SendError("Nie posiadasz wystarczającej ilości gotówki");
                return;
            }

            if (buildingEntity.DbModel.EnterCharge.HasValue)
                character.RemoveMoney(buildingEntity.DbModel.EnterCharge.Value);

            if (buildingEntity.PlayersInBuilding.Contains(sender.GetAccountEntity()))
            {
                sender.Position = (Vector3)sender.GetSharedData("DoorsTarget");
                sender.Dimension = 0;
                buildingEntity.PlayersInBuilding.Remove(sender.GetAccountEntity());

                character.CurrentBuilding = null;
            }
            else
            {
                sender.Position = (Vector3)sender.GetSharedData("DoorsTarget");
                sender.Dimension = ((BuildingEntity)sender.GetData("CurrentDoors")).InteriorDoorsColshape
                    .Dimension;
                buildingEntity.PlayersInBuilding.Add(sender.GetAccountEntity());
                character.CurrentBuilding = sender.GetData("CurrentDoors");
            }
        }

        private bool _spamProtector;
        public static void Knock(Client player)
        {
            if (!player.HasData("CurrentDoors")) return;
            BuildingEntity building = (BuildingEntity)player.GetData("CurrentDoors");
            if (building._spamProtector) return;

            building._spamProtector = true;
            CharacterEntity character = player.GetAccountEntity().CharacterEntity;
            ChatScript.SendMessageToNearbyPlayers(character, "unosi dłoń i puka do drzwi budynku", ChatMessageType.ServerMe);
            ChatScript.SendMessageToSpecifiedPlayers(character, building.PlayersInBuilding.Select(x => x.CharacterEntity), "Słychać pukanie do drzwi.", ChatMessageType.ServerDo);

            // Ochrona przed spamem w pukaniu do drzwi
            Timer timer = new Timer(4000);
            timer.Start();
            timer.Elapsed += (o, e) =>
            {
                building._spamProtector = false;
                timer.Stop();
                timer.Dispose();
            };
        }

        public static void LoadBuildings()
        {
            using (BuildingsRepository repository = new BuildingsRepository())
                foreach (BuildingModel building in repository.GetAll())
                {
                    new BuildingEntity(building);
                }
        }

        public static BuildingEntity Create(AccountModel creator, string name, decimal cost, Vector3 internalPosition, Vector3 externalPosition, bool spawnPossible, CharacterModel characterModel = null, GroupModel groupModel = null)
        {
            BuildingModel buildingModel = new BuildingModel()
            {
                Name = name,
                Cost = cost,
                CurrentObjectCount = 0,
                Description = "",
                EnterCharge = null,
                InternalDimension = Utils.GetNextFreeDimension(),
                InternalPickupPositionX = internalPosition.X,
                InternalPickupPositionY = internalPosition.Y,
                InternalPickupPositionZ = internalPosition.Z,
                ExternalPickupPositionX = externalPosition.X,
                ExternalPickupPositionY = externalPosition.Y,
                ExternalPickupPositionZ = externalPosition.Z,
                Items = new List<ItemModel>(),
                SpawnPossible = spawnPossible,
                Creator = creator,
                HasCctv = false,
                HasSafe = false,
                MaxObjectCount = 10,
                Character = characterModel,
                Group = groupModel
            };

            using (BuildingsRepository repository = new BuildingsRepository())
            {
                repository.Insert(buildingModel);
                repository.Save();
            }
            return new BuildingEntity(buildingModel);
        }

        #endregion
    }
}