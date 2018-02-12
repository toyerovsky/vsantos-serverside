/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

namespace Serverside.Entities.Core.Item
{
    public enum ItemType
    {
        Food,
        Weapon,
        WeaponClip,
        Mask,
        Drug,
        Dice,
        Watch,
        Cloth,
        Transmitter,
        Alcohol,
        Cellphone,
        Tuning
    }

    public enum TuningType
    {
        Speed,
        Brakes
    }

    public enum DrugType
    {
        Marihuana,
        Lsd,
        Ekstazy,
        Amfetamina,
        Metaamfetamina,
        Crack,
        Kokaina,
        Haszysz,
        Heroina
    }

    public enum CellphoneVisibleType
    {
        iFriut,
        Casual
    }
}
