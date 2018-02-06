/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.ComponentModel.DataAnnotations;

namespace Serverside.Core.Database.Models
{
    public class GroupWarehouseOrderModel
    {
        [Key]
        public long Id { get; set; }
        public virtual GroupModel Getter { get; set; }

        //TODO: Zmienić na konkretne typy
        public string OrderItemsJson { get; set; }
        public string ShipmentLog { get; set; }
    }
}