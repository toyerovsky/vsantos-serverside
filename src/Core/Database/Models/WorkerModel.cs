/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

namespace Serverside.Core.Database.Models
{
    public class WorkerModel
    {
        public long Id { get; set; }

        public virtual GroupModel Group { get; set; }
        public virtual CharacterModel Character { get; set; }

        public int Salary { get; set; }
        public int DutyMinutes { get; set; }

        //Te będą dla każdej grupy
        //Uprawnienia do wypłacania pieniędzy
        public bool PaycheckRight { get; set; }

        //Uprawnienia do otwierania/zamykania drzwi budynku 
        public bool DoorsRight { get; set; }

        //Uprawnienia do zapraszania i wypraszania graczy z grupy
        public bool RecrutationRight { get; set; }

        //Uprawnienia do chatu grupowego 
        public bool ChatRight { get; set; }

        //Oferowanie z magazynu grupy   
        public bool OfferFromWarehouseRight { get; set; }

        //Zamawianie przedmiotów z magazynu grupowego
        public bool OrderFromWarehouseRight { get; set; }

        //Te będą się różniły zależnie od rodzaju grupy, np Taxi może mieć przejazd jako pierwsze prawo
        //a gastronomia coś innego
        public bool? FirstRight { get; set; }
        public bool? SecondRight { get; set; }
        public bool? ThirdRight { get; set; }
        public bool? FourthRight { get; set; }
        public bool? FifthRight { get; set; }
        public bool? SixthRight { get; set; }
        public bool? SeventhRight { get; set; }
        public bool? EightRight { get; set; }
    }
}