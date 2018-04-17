/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using VRP.Core.Enums;
using VRP.Core.Extensions;
using VRP.Core.Serialization;
using VRP.Core.Tools;
using VRP.Serverside.Core.Extensions;
using VRP.Serverside.Entities.Common.Carshop.Models;
using VRP.Serverside.Entities.Core;
using VRP.Serverside.Entities.Core.Vehicle;
using FullPosition = VRP.Serverside.Core.FullPosition;
using VRP.Serverside.Constant.RemoteEvents;

namespace VRP.Serverside.Entities.Common.Carshop
{
    public class CarshopScript : Script
    {
        public static List<CarshopVehicleModel> Vehicles { get; set; } = new List<CarshopVehicleModel>();
        public static List<CarshopEntity> Carshops { get; set; } = new List<CarshopEntity>();

        [ServerEvent(Event.ResourceStart)]
        private void OnResourceStart()
        {
            Vehicles =
                XmlHelper.GetXmlObjects<CarshopVehicleModel>(
                    Path.Combine(Utils.XmlDirectory, "CarshopVehicles"));

            //Vehicles.AddRange(new List<CarshopVehicleModel>
            //{
            //    //Używane 
            //    //Rahapsody
            //    //Blista2

            //    //Do zrobienia
            //    //CogCabrio
            //    //Oracle2
            //    //Sentinel2
            //    //Windsor2

            //    //Kompaktowe
            //    new CarshopVehicleModel("Blista", VehicleHash.Blista, VehicleClass.Compact, new decimal(17999), new List<CarshopType>() {CarshopType.Poor, CarshopType.Medium}),
            //    new CarshopVehicleModel("Brioso", VehicleHash.Brioso, VehicleClass.Compact, new decimal(21999), new List<CarshopType>() {CarshopType.Poor, CarshopType.Medium}),
            //    new CarshopVehicleModel("Dilettante", VehicleHash.Dilettante, VehicleClass.Compact, new decimal(12000), new List<CarshopType>() {CarshopType.Poor, CarshopType.Medium}),
            //    new CarshopVehicleModel("Issi", VehicleHash.Issi2, VehicleClass.Compact, new decimal(25000), new List<CarshopType>() {CarshopType.Poor, CarshopType.Medium}),
            //    new CarshopVehicleModel("Panto", VehicleHash.Panto, VehicleClass.Compact, new decimal(10000), new List<CarshopType>() {CarshopType.Poor, CarshopType.Medium}),
            //    new CarshopVehicleModel("Prairie", VehicleHash.Prairie, VehicleClass.Compact, new decimal(22000), new List<CarshopType>() {CarshopType.Poor, CarshopType.Medium}),

            //    //Coupe
            //    new CarshopVehicleModel("Exemplar", VehicleHash.Exemplar, VehicleClass.Coupe, new decimal(190000), new List<CarshopType>() {CarshopType.Luxury}),
            //    new CarshopVehicleModel("F620", VehicleHash.F620, VehicleClass.Coupe, new decimal(151999), new List<CarshopType>() {CarshopType.Luxury}),
            //    new CarshopVehicleModel("Felon", VehicleHash.Felon2, VehicleClass.Coupe, new decimal(158999), new List<CarshopType>() {CarshopType.Luxury}),
            //    new CarshopVehicleModel("Jackal", VehicleHash.Jackal, VehicleClass.Coupe, new decimal(31000), new List<CarshopType>() {CarshopType.Medium}),
            //    new CarshopVehicleModel("Oracle", VehicleHash.Oracle, VehicleClass.Coupe, new decimal(21000), new List<CarshopType>() {CarshopType.Medium}),
            //    new CarshopVehicleModel("Sentinel", VehicleHash.Sentinel, VehicleClass.Coupe, new decimal(40000), new List<CarshopType>() {CarshopType.Medium}),
            //    new CarshopVehicleModel("Windsor", VehicleHash.Windsor, VehicleClass.Coupe, new decimal(220000), new List<CarshopType>() {CarshopType.Medium}),
            //    new CarshopVehicleModel("Zion", VehicleHash.Zion, VehicleClass.Coupe, new decimal(32000), new List<CarshopType>() {CarshopType.Medium}),

            //    //SUV
            //    new CarshopVehicleModel("BeeJay XL", VehicleHash.BJXL, VehicleClass.Suv, new decimal(16999), new List<CarshopType>() {CarshopType.Poor, CarshopType.Medium}),
            //    new CarshopVehicleModel("Baller I", VehicleHash.Baller, VehicleClass.Suv, new decimal(14999), new List<CarshopType>() {CarshopType.Poor, CarshopType.Medium}),
            //    new CarshopVehicleModel("Baller II", VehicleHash.Baller2, VehicleClass.Suv, new decimal(51000), new List<CarshopType>() {CarshopType.Medium}),
            //    new CarshopVehicleModel("Cavalcade I", VehicleHash.Cavalcade, VehicleClass.Suv, new decimal(26000), new List<CarshopType>() {CarshopType.Poor, CarshopType.Medium}),
            //    new CarshopVehicleModel("Cavalcade II", VehicleHash.Cavalcade2, VehicleClass.Suv, new decimal(71000), new List<CarshopType>() {CarshopType.Medium, CarshopType.Luxury}),
            //    new CarshopVehicleModel("Dubsta", VehicleHash.Dubsta2, VehicleClass.Suv, new decimal(56999), new List<CarshopType>() {CarshopType.Medium}),
            //    new CarshopVehicleModel("FQ2", VehicleHash.FQ2, VehicleClass.Suv, new decimal(52000), new List<CarshopType>() {CarshopType.Medium}),
            //    new CarshopVehicleModel("Granger", VehicleHash.Granger, VehicleClass.Suv, new decimal(61799), new List<CarshopType>() {CarshopType.Medium}),
            //    new CarshopVehicleModel("Gresley", VehicleHash.Gresley, VehicleClass.Suv, new decimal(56999), new List<CarshopType>() { CarshopType.Medium }),
            //    new CarshopVehicleModel("Habanero", VehicleHash.Gresley, VehicleClass.Suv, new decimal(68799), new List<CarshopType>() { CarshopType.Medium }),
            //    new CarshopVehicleModel("Huntley S", VehicleHash.Huntley, VehicleClass.Suv, new decimal(72699), new List<CarshopType>() { CarshopType.Medium }),
            //    new CarshopVehicleModel("Landstalker", VehicleHash.Landstalker, VehicleClass.Suv, new decimal(41899), new List<CarshopType>() { CarshopType.Medium }),
            //    new CarshopVehicleModel("Patriot", VehicleHash.Patriot, VehicleClass.Suv, new decimal(78999), new List<CarshopType>() { CarshopType.Medium }),
            //    new CarshopVehicleModel("Radius", VehicleHash.Radi, VehicleClass.Suv, new decimal(36999), new List<CarshopType>() { CarshopType.Medium }),
            //    new CarshopVehicleModel("Rocoto", VehicleHash.Rocoto, VehicleClass.Suv, new decimal(98000), new List<CarshopType>() { CarshopType.Medium }),
            //    new CarshopVehicleModel("Seminole", VehicleHash.Seminole, VehicleClass.Suv, new decimal(64999), new List<CarshopType>() { CarshopType.Medium }),
            //    new CarshopVehicleModel("Serrano", VehicleHash.Serrano, VehicleClass.Suv, new decimal(28999), new List<CarshopType>() { CarshopType.Medium }),
            //    new CarshopVehicleModel("XLS", VehicleHash.XLS, VehicleClass.Suv, new decimal(54999), new List<CarshopType>() {CarshopType.Medium}),

            //    //Sedany
            //});

            //Tworzenie salonow dla każdego pliku XML
            XmlHelper.GetXmlObjects<CarshopModel>(Path.Combine(Utils.XmlDirectory, "Carshops"))
                .ForEach(carshopModel =>
                {
                    CarshopEntity carshop = new CarshopEntity(carshopModel);
                    carshop.Spawn();
                    Carshops.Add(carshop);
                });
        }

        [RemoteEvent(RemoteEvents.OnPlayerBoughtVehicle)]
        public void OnPlayerBoughtVehicleHandler(Client sender, params object[] arguments)
        {
            //arguments[0] to nazwa pojazdu

            if (!Enum.TryParse(arguments[0].ToString(), out VehicleHash vehicleHash)) return;

            CarshopVehicleModel vehicle = Vehicles.First(v => v.Name == arguments[0].ToString());

            CharacterEntity character = sender.GetAccountEntity().CharacterEntity;
            if (character.HasMoney(vehicle.Cost))
            {
                character.RemoveMoney(vehicle.Cost);

                VehicleEntity.Create(new FullPosition(new Vector3(-50, -1680, 29.5), new Vector3(0, 0, 0)),
                    vehicleHash, "", 0, null, ApiExtensions.GetRandomColor(), ApiExtensions.GetRandomColor(), 0f, 0f, sender.GetAccountEntity().CharacterEntity.DbModel);
                sender.SendInfo($"Pojazd {vehicleHash.ToString()} zakupiony pomyślnie.");
            }
            else
            {
                sender.SendError("Nie posiadasz wystarczającej ilości gotówki.");
            }
        }

        [Command("dodajautosalon", "~y~ UŻYJ ~w~ /dodajautosalon [model] [koszt] [typ salonu]")]
        public void AddVehicleToCarshop(Client sender, VehicleHash hash, VehicleClass vehicleClass, decimal cost, string type, string type2 = "Empty")
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.AdministratorGry)
            {
                sender.SendWarning("Nie posiadasz uprawnień do tworzenia pojazdu w salonie.");
                return;
            }

            if (Vehicles.Any(v => v.Hash == hash))
            {
                sender.SendError("Podany pojazd jest już dodany.");
                return;
            }

            if (!ValidationHelper.IsMoneyValid(cost))
            {
                sender.SendError("Wprowadzona kwota gotówki jest nieprawidłowa.");
                return;
            }

            CarshopType endType = CarshopType.Empty;
            CarshopType endType2 = CarshopType.Empty;
            var carshopTypes = ((CarshopType[])Enum.GetValues(typeof(CarshopType))).ToList();

            if (carshopTypes.All(carshopType => carshopType.GetDescription() != type && carshopType.GetDescription() != type2)) ;
            {
                sender.SendError("Wprowadzony typ salonu jest nieprawidłowy.");
            }

            foreach (CarshopType item in carshopTypes)
            {
                if (item.GetDescription() == type)
                    endType = item;
                if (item.GetDescription() == type2)
                    endType2 = item;
            }

            if (endType2 != CarshopType.Empty) endType = endType | endType2;

            CarshopVehicleModel vehicle =
                new CarshopVehicleModel(hash.ToString(), hash, vehicleClass, cost, endType)
                {
                    CreatorForumName = sender.GetAccountEntity().DbModel.Name
                };

            XmlHelper.AddXmlObject(vehicle, Path.Combine(Utils.XmlDirectory, "CarshopVehicles"), vehicle.Name);
        }

        [Command("dodajsalon", "~y~ UŻYJ ~w~ /dodajsalon [nazwa] [typ]")]
        public void AddCarshop(Client sender, string name, CarshopType type)
        {
            AccountEntity player = sender.GetAccountEntity();

            if (player.DbModel.ServerRank < ServerRank.AdministratorGry)
            {
                sender.SendWarning("Nie posiadasz uprawnień do tworzenia salonu samochodowego.");
                return;
            }

            sender.SendInfo("Ustaw się w wybranej pozycji, a następnie wpisz \"tu\". użyj ctrl + alt + shift + d aby poznać swoją obecną pozycję.");

            player.HereHandler += client =>
            {
                CarshopModel data = new CarshopModel
                {
                    CreatorForumName = client.GetAccountEntity().DbModel.Name,
                    Position = client.Position,
                    Type = type,
                };
                data.CreatorForumName = client.GetAccountEntity().DbModel.Name;

                XmlHelper.AddXmlObject(data, Path.Combine(Utils.XmlDirectory, "Carshops"));
                Carshops.Add(new CarshopEntity(data));
                sender.SendInfo("Dodawanie salonu zakończyło się pomyślnie.");
            };
        }

        [Command("usunsalon", "~y~ UŻYJ ~w~ /usunsalon")]
        public void DeleteCarshop(Client sender)
        {
            if (sender.GetAccountEntity().DbModel.ServerRank < ServerRank.AdministratorGry)
            {
                sender.SendWarning("Nie posiadasz uprawnień do usuwania salonu samochodowego.");
                return;
            }

            if (Carshops.Count == 0)
            {
                sender.SendWarning("Nie znaleziono salonu pojazdów który można usunąć.");
                return;
            }

            CarshopEntity carshop = Carshops.First(x => x.ColShape.IsPointWithin(sender.Position));
            if (XmlHelper.TryDeleteXmlObject(carshop.Data.FilePath))
            {
                sender.SendInfo("Usuwanie salonu zakończyło się pomyślnie");
                Carshops.Remove(carshop);
                carshop.Dispose();
            }
            else
            {
                sender.SendError("Usuwanie salonu zakończyło się niepomyślnie.");
            }
        }
    }
}
