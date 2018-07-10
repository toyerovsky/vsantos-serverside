/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */


using VRP.DAL.Database.Models.Misc;

namespace VRP.DAL.Database.Models.TelephoneBooth
{
    public class TelephoneBoothModel
    {
        public int Id { get; set; }
        public int Number { get; set; }
        public decimal Cost { get; set; }
        public int? CreatorId { get; set; }

        // navigation properties
        public virtual ZoneModel ZoneModel { get; set; }
    }
}