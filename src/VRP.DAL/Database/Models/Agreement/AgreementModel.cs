/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace VRP.DAL.Database.Models.Agreement
{
    public class AgreementModel
    {
        public int Id { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        public bool AutoRenewal { get; set; }

        // foreign keys
        [ForeignKey("LeaseModel")]
        public int? LeaseId { get; set; }
        
        // navigation properties
        public virtual LeaseModel LeaseModel { get; set; }
    }
}
