/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;

namespace Serverside.Core.Extensions
{
    [Flags]
    public enum AnimationFlags
    {
        Loop = 1 << 0,
        StopOnLastFrame = 1 << 1,
        OnlyAnimateUpperBody = 1 << 4,
        AllowPlayerControl = 1 << 5,
        Cancellable = 1 << 7
    }

    public enum VehicleClass
    {
        Compact = 0,
        Sedans = 1,
        Suv = 2,
        Coupe = 3,
        Muscle = 4,
        SportClassics = 5,
        Sports = 6,
        Super = 7,
        Motorcycles = 8,
        Offroad = 9,
        Industrial = 10,
        Utility = 11,
        Vans = 12,
        Cycle = 13,
        Boat = 14,
        Heli = 15,
        Planes = 16,
        Service = 17,
        Emergency = 18,
        Military = 19,
        Commercial = 20,
        Train = 21,
        Trailer = 22
    }

    public enum Wheel
    {
        FrontLeft,
        FrontRight,
        MiddleLeft,
        MiddleRight,
        RearLeft,
        RearRight,
        BikeFront,
        BikeRear
    }

    public enum Doors
    {
        FrontLeftDoor,
        FrontRightDoor,
        BackLeftDoor,
        BackRightDoor,
        Hood,
        Trunk,
        Back,
        Back2,
    }
}
