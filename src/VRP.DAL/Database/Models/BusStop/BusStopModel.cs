/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.ComponentModel.DataAnnotations.Schema;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Misc;

namespace VRP.DAL.Database.Models.BusStop
{
    public class BusStopModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        // foreign keys
        [ForeignKey("Creator")]
        public int CreatorId { get; set; }
        [ForeignKey("BusStopZone")]
        public int BusStopZoneId { get; set; }

        // navigation properties
        public virtual AccountModel Creator { get; set; }
        public virtual ZoneModel BusStopZone { get; set; }
    }
}