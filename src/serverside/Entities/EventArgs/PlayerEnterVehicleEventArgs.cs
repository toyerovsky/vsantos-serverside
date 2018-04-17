using VRP.Serverside.Entities.Core;
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