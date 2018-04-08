/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using GTANetworkAPI;

namespace VRP.Serverside.Core
{
    [Serializable]
    public class FullPosition
    {
        public Vector3 Position { get; set; }
        public Vector3 Rotation { get; set; }
        public Vector3 Direction { get; set; }

        public FullPosition() { }

        public FullPosition(Vector3 position, Vector3 rotation, Vector3 direction = null)
        {
            Position = position;
            Rotation = rotation;
            Direction = direction;
        }

        public override string ToString() => $"Pozycja(X: {Position.X}, Y: {Position.Y}, Z: {Position.Z})" +
                                             $" Rotacja(X: {Rotation.X} Y: {Rotation.Y} Z: {Rotation.Z}";
        
    }
}
