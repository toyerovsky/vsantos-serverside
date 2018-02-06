/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using GTANetworkAPI;
using Serverside.Constant.Structs;
using Serverside.Items;

namespace Serverside.Constant
{
    public static class ConstantItems
    {
        /// <summary>
        /// Klucz to słownik, wartosc to animacja
        /// </summary>
        public static Dictionary<string, string> Animations
        {
            get
            {
                return File.ReadLines(ServerInfo.WorkingDirectory + @"\Files\dict.txt").Select(
                    x => x.Split(',')).ToDictionary(
                        s => s[0], s => s[1]);
            }
        }

        public static Tuple<string, ItemType> GetCrimeBotItemName(string name)
        {
            if (name.Contains("Revolver"))
                return new Tuple<string, ItemType>("Magazynek Colt Anaconda", ItemType.WeaponClip);
            else if (name.Contains("AssaultRifleMagazine"))
                return new Tuple<string, ItemType>("Magazynek AK-47", ItemType.WeaponClip);
            else if (name.Contains("SNSPistolMagazine"))
                return new Tuple<string, ItemType>("Magazynek Colt Junior 25", ItemType.WeaponClip);
            else if (name.Contains("Pistol50Magazine"))
                return new Tuple<string, ItemType>("Magazynek Colt M45A1", ItemType.WeaponClip);
            else if (name.Contains("CarbineRifleMagazine"))
                return new Tuple<string, ItemType>("Magazynek Colt M4", ItemType.WeaponClip);
            else if (name.Contains("DoubleBarrelShotgunMagazine"))
                return new Tuple<string, ItemType>("Magazynek AYA 12g", ItemType.WeaponClip);
            else if (name.Contains("MachinePistolMagazine"))
                return new Tuple<string, ItemType>("Magazynek TEC-9", ItemType.WeaponClip);
            else if (name.Contains("HeavyPistolMagazine"))
                return new Tuple<string, ItemType>("Magazynek Desert Eagle", ItemType.WeaponClip);
            else if (name.Contains("SniperRifleMagazine"))
                return new Tuple<string, ItemType>("Magazynek Barrett M98B", ItemType.WeaponClip);
            else if (name.Contains("VintagePistolMagazine"))
                return new Tuple<string, ItemType>("Magazynek FN 1922", ItemType.WeaponClip);
            else if (name.Contains("CombatPistolMagazine"))
                return new Tuple<string, ItemType>("Magazynek Walther P99", ItemType.WeaponClip);
            else if (name.Contains("CompactRifleMagazine"))
                return new Tuple<string, ItemType>("Magazynek AKMSU", ItemType.WeaponClip);
            else if (name.Contains("SawnoffShotgunMagazine"))
                return new Tuple<string, ItemType>("Magazynek Mossberg 500", ItemType.WeaponClip);
            else if (name.Contains("MicroSMGMagazine"))
                return new Tuple<string, ItemType>("Magazynek Mini-UZI", ItemType.WeaponClip);
            else if (name.Contains("PistolMagazine"))
                return new Tuple<string, ItemType>("Magazynek Colt M1911", ItemType.WeaponClip);
            else if (name.Contains("PumpShotgunMagazine"))
                return new Tuple<string, ItemType>("Magazynek Mossberg 590", ItemType.WeaponClip);
            else if (name.Contains("MiniSMGMagazine"))
                return new Tuple<string, ItemType>("Magazynek Skorpion vz 61", ItemType.WeaponClip);
            else if (name.Contains("Revolver"))
                return new Tuple<string, ItemType>("Colt Anaconda", ItemType.Weapon);
            else if (name.Contains("AssaultRifle"))
                return new Tuple<string, ItemType>("AK-47", ItemType.Weapon);
            else if (name.Contains("SNSPistol"))
                return new Tuple<string, ItemType>("Colt Junior 25", ItemType.Weapon);
            else if (name.Contains("Pistol50"))
                return new Tuple<string, ItemType>("Colt M45A1", ItemType.Weapon);
            else if (name.Contains("CarbineRifle"))
                return new Tuple<string, ItemType>("Colt M4", ItemType.Weapon);
            else if (name.Contains("DoubleBarrelShotgun"))
                return new Tuple<string, ItemType>("AYA 12g", ItemType.Weapon);
            else if (name.Contains("MachinePistol"))
                return new Tuple<string, ItemType>("TEC-9", ItemType.Weapon);
            else if (name.Contains("HeavyPistol"))
                return new Tuple<string, ItemType>("Desert Eagle", ItemType.Weapon);
            else if (name.Contains("SniperRifle"))
                return new Tuple<string, ItemType>("Barrett M98B", ItemType.Weapon);
            else if (name.Contains("VintagePistol"))
                return new Tuple<string, ItemType>("FN 1922", ItemType.Weapon);
            else if (name.Contains("CombatPistol"))
                return new Tuple<string, ItemType>("Walther P99", ItemType.Weapon);
            else if (name.Contains("CompactRifle"))
                return new Tuple<string, ItemType>("AKMSU", ItemType.Weapon);
            else if (name.Contains("SawnoffShotgun"))
                return new Tuple<string, ItemType>("Mossberg 500", ItemType.Weapon);
            else if (name.Contains("MicroSMG"))
                return new Tuple<string, ItemType>("Mini-UZI", ItemType.Weapon);
            else if (name.Contains("Pistol"))
                return new Tuple<string, ItemType>("Colt M1911", ItemType.Weapon);
            else if (name.Contains("PumpShotgun"))
                return new Tuple<string, ItemType>("Mossberg 590", ItemType.Weapon);
            else if (name.Contains("MiniSMG"))
                return new Tuple<string, ItemType>("Skorpion vz 61", ItemType.Weapon);
            else if (name.Contains("Golfclub"))
                return new Tuple<string, ItemType>("Kij golfowy", ItemType.Weapon);
            else if (name.Contains("Bat"))
                return new Tuple<string, ItemType>("Kij baseballowy", ItemType.Weapon);
            else if (name.Contains("Hammer"))
                return new Tuple<string, ItemType>("Młotek", ItemType.Weapon);
            else if (name.Contains("Crowbar"))
                return new Tuple<string, ItemType>("Łom", ItemType.Weapon);
            else if (name.Contains("Marijuana"))
                return new Tuple<string, ItemType>("Marihuana", ItemType.Weapon);
            else if (name.Contains("Lsd"))
                return new Tuple<string, ItemType>("LSD", ItemType.Drug);
            else if (name.Contains("Excstasy"))
                return new Tuple<string, ItemType>("Ekstazy", ItemType.Drug);
            else if (name.Contains("Amphetamine"))
                return new Tuple<string, ItemType>("Amfetamina", ItemType.Drug);
            else if (name.Contains("Metaamphetamine"))
                return new Tuple<string, ItemType>("Metaamfetamina", ItemType.Drug);
            else if (name.Contains("Crack"))
                return new Tuple<string, ItemType>("Crack", ItemType.Drug);
            else if (name.Contains("Cocaine"))
                return new Tuple<string, ItemType>("Kokaina", ItemType.Drug);
            else if (name.Contains("Hasish"))
                return new Tuple<string, ItemType>("Haszysz", ItemType.Drug);
            else if (name.Contains("Heroin"))
                return new Tuple<string, ItemType>("Heroina", ItemType.Drug);

            return new Tuple<string, ItemType>("BRAK NAZWY", ItemType.Weapon);
        }

        public static Tuple<WeaponHash, int?> GetWeaponData(string friendlyName)
        {
            if (friendlyName.Contains("Colt Anaconda"))
                return new Tuple<WeaponHash, int?>(WeaponHash.Revolver, 12);
            else if (friendlyName.Contains("AK-47"))
                return new Tuple<WeaponHash, int?>(WeaponHash.AssaultRifle, 60);
            else if (friendlyName.Contains("Colt Junior 25"))
                return new Tuple<WeaponHash, int?>(WeaponHash.SNSPistol, 12);
            else if (friendlyName.Contains("Colt M45A1"))
                return new Tuple<WeaponHash, int?>(WeaponHash.Pistol50, 18);
            else if (friendlyName.Contains("Colt M4"))
                return new Tuple<WeaponHash, int?>(WeaponHash.CarbineRifle, 30);
            else if (friendlyName.Contains("AYA 12g"))
                return new Tuple<WeaponHash, int?>(WeaponHash.DoubleBarrelShotgun, 4);
            else if (friendlyName.Contains("TEC-9"))
                return new Tuple<WeaponHash, int?>(WeaponHash.MachinePistol, 54);
            else if (friendlyName.Contains("Desert Eagle"))
                return new Tuple<WeaponHash, int?>(WeaponHash.HeavyPistol, 14);
            else if (friendlyName.Contains("Barrett M98B"))
                return new Tuple<WeaponHash, int?>(WeaponHash.SniperRifle, 10);
            else if (friendlyName.Contains("FN 1922"))
                return new Tuple<WeaponHash, int?>(WeaponHash.VintagePistol, 20);
            else if (friendlyName.Contains("Walther P99"))
                return new Tuple<WeaponHash, int?>(WeaponHash.CombatPistol, 18);
            else if (friendlyName.Contains("AKMSU"))
                return new Tuple<WeaponHash, int?>(WeaponHash.CompactRifle, 60);
            else if (friendlyName.Contains("Mossberg 500"))
                return new Tuple<WeaponHash, int?>(WeaponHash.SawnOffShotgun, 16);
            else if (friendlyName.Contains("Mini-UZI"))
                return new Tuple<WeaponHash, int?>(WeaponHash.MicroSMG, 16);
            else if (friendlyName.Contains("Colt M1911"))
                return new Tuple<WeaponHash, int?>(WeaponHash.Pistol, 18);
            else if (friendlyName.Contains("Mossberg 590"))
                return new Tuple<WeaponHash, int?>(WeaponHash.PumpShotgun, 16);
            else if (friendlyName.Contains("Skorpion vz 61"))
                return new Tuple<WeaponHash, int?>(WeaponHash.MiniSMG, 20);
            return new Tuple<WeaponHash, int?>(WeaponHash.BattleAxe, null);
        }

        public static readonly List<VehicleInfo> VehicleInfo = new List<VehicleInfo>()
        {
            new VehicleInfo(VehicleHash.Adder, 50, 5.0f),
            new VehicleInfo(VehicleHash.Sultan, 30, 1.0f),

        };

        public static readonly List<Tuple<string, float>> TuningInfo = new List<Tuple<string, float>>()
        {
            new Tuple<string, float>("ECU", 5.0f)
        };

        public static readonly Dictionary<string, PedHash> ConstantNames = new Dictionary<string, PedHash>()
        {
            {"John Galante", PedHash.Baygor},
            {"Lawrance Cagiano", PedHash.Epsilon01AFY},
            {"Randall Salvaggio", PedHash.Fatlatin01AMM},
            {"Cade Smith", PedHash.Cntrybar01SMM},
            {"Maxwell Savona", PedHash.BikeHire01},
            {"Emily Royer", PedHash.Eastsa03AFY},
            {"Daniel Berter", PedHash.Epsilon02AMY},
            {"Jonathan Becker", PedHash.FosRepCutscene},
        };

        public static readonly List<BuildingData> DefaultInteriors = new List<BuildingData>
        {
            new BuildingData("Garaż 2 auta", new Vector3(173.2903, -1003.6, -99.65707)),
            new BuildingData("Garaż 6 aut",  new Vector3(197.8153, -1002.293, -99.65749)),
            new BuildingData("Garaż 10 aut", new Vector3(229.9559, -981.7928, -99.66071)),
            new BuildingData("Tanie mieszkanie", new Vector3(261.4586, -998.8196, -99.00863)),
            new BuildingData("4 Integrity Way Apt 28", new Vector3(-18.07856, -583.6725, 79.46569)),
            new BuildingData("4 Integrity Way, Apt 30", new Vector3(-35.31277, -580.4199, 88.71221)),
            new BuildingData("Dell Perro Heights, Apt 4", new Vector3(-1468.14, -541.815, 73.4442)),
            new BuildingData("Dell Perro Heights Apt 7", new Vector3(-1477.14, -538.7499, 55.5264)),
            new BuildingData("Richard Majestic, Apt 2", new Vector3(-915.811, -379.432, 113.6748)),
            new BuildingData("Tinsel Towers, Apt 42", new Vector3(-614.86, 40.6783, 97.60007)),
            new BuildingData("EclipseTowers, Apt 3", new Vector3(-773.407, 341.766, 211.397)),
            new BuildingData("3655 Wild Oats Drive", new Vector3(-169.286, 486.4938, 137.4436)),
            new BuildingData("2044 North Conker Avenue", new Vector3(340.9412, 437.1798, 149.3925)),
            new BuildingData("2045 North Conker Avenue", new Vector3(373.023, 416.105, 145.7006)),
            new BuildingData("2862 Hillcrest Avenue", new Vector3(-676.127, 588.612, 145.1698)),
            new BuildingData("2868 Hillcrest Avenue", new Vector3(-763.107, 615.906, 144.1401)),
            new BuildingData("2874 Hillcrest Avenue", new Vector3(-857.798, 682.563, 152.6529)),
            new BuildingData("2677 Whispymound Drive", new Vector3(120.5, 549.952, 184.097)),
            new BuildingData("2133 Mad Wayne Thunder", new Vector3(-1288, 440.748, 97.69459)),
            new BuildingData("CharCreator", new Vector3(402.5164, -1002.847, -99.2587)),
            new BuildingData("Mission Carpark", new Vector3(405.9228, -954.1149, -99.6627)),
            new BuildingData("Torture Room", new Vector3(136.5146, -2203.149, 7.30914)),
            new BuildingData("Solomon's Office", new Vector3(-1005.84, -478.92, 50.02733)),
            new BuildingData("Psychiatrist's Office", new Vector3(-1908.024, -573.4244, 19.09722)),
            new BuildingData("Omega's Garage", new Vector3(2331.344, 2574.073, 46.68137)),
            new BuildingData("Movie Theatre", new Vector3(-1427.299, -245.1012, 16.8039)),
            new BuildingData("Motel", new Vector3(152.2605, -1004.471, -98.99999)),
            new BuildingData("Mandrazos Ranch", new Vector3(152.2605, 1146.954, 114.337)),
            new BuildingData("Life Invader Office", new Vector3(-1044.193, -236.9535, 37.96496)),
            new BuildingData("Lester's House", new Vector3(1273.9, -1719.305, 54.77141)),
            new BuildingData("FBI Top Floor", new Vector3(134.5835, -749.339, 258.152)),
            new BuildingData("FBI Floor 47", new Vector3(134.5835, -766.486, 234.152)),
            new BuildingData("FBI Floor 49", new Vector3(134.635, -765.831, 242.152)),
            new BuildingData("IAA Office", new Vector3(117.22, -620.938, 206.1398)),
        };

        public static List<Vector3> ServerSpawnPositions = new List<Vector3>()
        {
            
        };
    }
}