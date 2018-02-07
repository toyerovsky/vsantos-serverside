/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkInternals;
using Serverside.Core.Database.Models;

namespace Serverside.Items
{
    internal class Tuning : Item
    {
        /// <summary>
        /// 1 to typ tuningu, pozostałe parametry zależą od typu tuningu
        /// dla 1 (speed) 2 parametr engineMultipilier, 3 torque
        /// dla 2 (brakes) 2 parametr to moc hamowania
        /// </summary>
        /// <param name="events"></param>
        /// <param name="itemModel"></param>
        public Tuning(EventClass events, ItemModel itemModel) : base(events, itemModel) { }

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