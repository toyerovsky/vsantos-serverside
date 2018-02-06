/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Core;
using Serverside.Core.Extensions;

namespace Serverside.Employer
{
    public sealed class EmployerBot : Bot
    {
        public EmployerBot(EventClass events, string name, PedHash pedHash, FullPosition position) : base(events, name, pedHash, position)
        {
        }

        public override void Intialize()
        {
            base.Intialize();
            ColShape employerColShape = NAPI.ColShape.CreateCylinderColShape(BotHandle.Position, 3f, 2f);

            employerColShape.OnEntityEnterColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    var sender = NAPI.Player.GetPlayerFromHandle(entity);
                    NAPI.ClientEvent.TriggerClientEvent(sender, "OnPlayerEnteredEmployer", sender.GetAccountEntity().CharacterEntity.DbModel.MoneyJob.ToString());
                }
            };

            employerColShape.OnEntityExitColShape += (shape, entity) =>
            {
                if (NAPI.Entity.GetEntityType(entity) == EntityType.Player)
                {
                    var sender = NAPI.Player.GetPlayerFromHandle(entity);
                    NAPI.ClientEvent.TriggerClientEvent(sender, "OnPlayerExitEmployer");
                }
            };
        }
    }
}