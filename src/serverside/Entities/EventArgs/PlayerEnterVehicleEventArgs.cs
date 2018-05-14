/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Serverside.Entities.Core.Vehicle;

namespace VRP.Serverside.Entities.EventArgs
{
    public class PlayerEnterVehicleEventArgs : System.EventArgs
    {
        public VehicleEntity Vehicle { get; set; }
        public sbyte SeatId { get; set; }

        public PlayerEnterVehicleEventArgs(VehicleEntity vehicle, sbyte seatId)
        {
            Vehicle = vehicle;
            SeatId = seatId;
        }
    }
}