/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System.Collections.Generic;

namespace VRP.DAL.Database.Models.Corner
{
    public class CornerModel
    {
        public int Id { get; set; }
        public int CreatorId { get; set; }
        public float PositionX { get; set; }
        public float PositionY { get; set; }
        public float PositionZ { get; set; }
        public string BotPathJson { get; set; }

        // navigation properties
        public virtual ICollection<CornerBotModel> CornerBots { get; set; }
    }
}