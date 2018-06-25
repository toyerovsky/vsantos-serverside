/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */
 
using VRP.DAL.Database.Models.Item;

namespace VRP.Serverside.Entities.Core.Item
{
    internal class Tuning : ItemEntity
    {
        /// <summary>
        /// param 1 to typ tuningu, pozostałe parametry zależą od typu tuningu
        /// param 2 dla 1 (speed) 2 parametr engineMultipilier, 3 torque
        /// param 3 dla 2 (brakes) 2 parametr to moc hamowania
        /// param 4 określa czy tuning jest zamontowany
        /// </summary>
        /// <param name="itemModel"></param>
        public Tuning(ItemModel itemModel) : base(itemModel) { }

        public override string UseInfo
        {
            get
            {
                if (DbModel.FirstParameter.HasValue && (TuningType)DbModel.FirstParameter == TuningType.Speed)
                {
                    return $"Tuning: {DbModel.Name} zwiększa prędkość maksymalną o: {DbModel.SecondParameter} procent, oraz zwiększa moment obrotowy o: {DbModel.ThirdParameter} procent.";
                }
                return $"Tuning: {DbModel.Name} zwiększa moc hamulcy o: {DbModel.SecondParameter} procent.";
            }
        }
    }
}