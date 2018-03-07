/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Base;
using FullPosition = VRP.Serverside.Core.FullPosition;

namespace VRP.Serverside.Entities.Peds.Employer
{
    public sealed class EmployerPedEntity : PedEntity
    {
        public EmployerPedEntity(string name, PedHash pedHash, FullPosition position) : base(name, pedHash, position)
        {
        }

        public override void Spawn()
        {
            base.Spawn();
            ColShape employerColShape = NAPI.ColShape.CreateCylinderColShape(BotHandle.Position, 3f, 2f);

            employerColShape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    Client sender = NAPI.Player.GetPlayerFromHandle(entity);
                    NAPI.ClientEvent.TriggerClientEvent(sender, "OnPlayerEnteredEmployer", sender.GetAccountEntity().CharacterEntity.DbModel.MoneyJob.ToString());
                }
            };

            employerColShape.OnEntityExitColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    Client sender = NAPI.Player.GetPlayerFromHandle(entity);
                    NAPI.ClientEvent.TriggerClientEvent(sender, "OnPlayerExitEmployer");
                }
            };
        }
    }
}