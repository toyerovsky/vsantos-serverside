/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using VRP.Serverside.Economy.Jobs.Base;
using VRP.Serverside.Entities.Core;

namespace VRP.Serverside.Economy.Jobs.Courier
{
    public class CourierWorker : JobWorkerController
    {
        public CourierWorker(AccountEntity player, JobVehicleEntity jobVehicle) : base(player, jobVehicle)
        {
        }
    }
}