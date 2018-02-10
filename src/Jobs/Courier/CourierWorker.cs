/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkInternals;
using Serverside.Entities.Core;
using Serverside.Jobs.Base;

namespace Serverside.Jobs.Courier
{
    public class CourierWorker : JobWorkerController
    {
        public CourierWorker(EventClass events, AccountEntity player, JobVehicleEntity jobVehicle) : base(events, player, jobVehicle)
        {
        }
    }
}