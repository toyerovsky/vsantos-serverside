/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

namespace VRP.Core.Database.Models
{
    public class CrimeBotModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public virtual AccountModel Creator { get; set; }
        public virtual GroupModel GroupModel { get; set; }
        public virtual string Vehicle { get; set; }
        public string Model { get; set; }

        public decimal? PistolCost { get; set; }
        public int? PistolCount { get; set; }
        public int? PistolDefaultCount { get; set; }

        public decimal? PistolMk2Cost { get; set; }
        public int? PistolMk2Count { get; set; }
        public int? PistolMk2DefaultCount { get; set; }

        public decimal? CombatPistolCost { get; set; }
        public int? CombatPistolCount { get; set; }
        public int? CombatPistolDefaultCount { get; set; }

        public decimal? Pistol50Cost { get; set; }
        public int? Pistol50Count { get; set; }
        public int? Pistol50DefaultCount { get; set; }

        public decimal? SnsPistolCost { get; set; }
        public int? SnsPistolCount { get; set; }
        public int? SnsPistolDefaultCount { get; set; }

        public decimal? HeavyPistolCost { get; set; }
        public int? HeavyPistolCount { get; set; }
        public int? HeavyPistolDefaultCount { get; set; }

        public decimal? RevolverCost { get; set; }
        public int? RevolverCount { get; set; }
        public int? RevolverDefaultCount { get; set; }

        public decimal? MicroSmgCost { get; set; }
        public int? MicroSmgCount { get; set; }
        public int? MicroSmgDefaultCount { get; set; }

        public decimal? SmgCost { get; set; }
        public int? SmgCount { get; set; }
        public int? SmgDefaultCount { get; set; }

        public decimal? SmgMk2Cost { get; set; }
        public int? SmgMk2Count { get; set; }
        public int? SmgMk2DefaultCount { get; set; }

        public decimal? MiniSmgCost { get; set; }
        public int? MiniSmgCount { get; set; }
        public int? MiniSmgDefaultCount { get; set; }

        public decimal? AssaultRifleCost { get; set; }
        public int? AssaultRifleCount { get; set; }
        public int? AssaultRifleDefaultCount { get; set; }

        public decimal? AssaultRifleMk2Cost { get; set; }
        public int? AssaultRifleMk2Count { get; set; }
        public int? AssaultRifleMk2DefaultCount { get; set; }

        public decimal? SniperRifleCost { get; set; }
        public int? SniperRifleCount { get; set; }
        public int? SniperRifleDefaultCount { get; set; }

        public decimal? DoubleBarrelShotgunCost { get; set; }
        public int? DoubleBarrelShotgunCount { get; set; }
        public int? DoubleBarrelShotgunDefaultCount { get; set; }

        public decimal? PumpShotgunCost { get; set; }
        public int? PumpShotgunCount { get; set; }
        public int? PumpShotgunDefaultCount { get; set; }

        public decimal? SawnoffShotgunCost { get; set; }
        public int? SawnoffShotgunCount { get; set; }
        public int? SawnoffShotgunDefaultCount { get; set; }

        public decimal? PistolMagazineCost { get; set; }
        public int? PistolMagazineCount { get; set; }
        public int? PistolMagazineDefaultCount { get; set; }

        public decimal? PistolMk2MagazineCost { get; set; }
        public int? PistolMk2MagazineCount { get; set; }
        public int? PistolMk2RMagazineDefaultCount { get; set; }

        public decimal? CombatPistolMagazineCost { get; set; }
        public int? CombatPistolMagazineCount { get; set; }
        public int? CombatPistolMagazineDefaultCount { get; set; }

        public decimal? Pistol50MagazineCost { get; set; }
        public int? Pistol50MagazineCount { get; set; }
        public int? Pistol50MagazineDefaultCount { get; set; }

        public decimal? SnsPistolMagazineCost { get; set; }
        public int? SnsPistolMagazineCount { get; set; }
        public int? SnsPistolMagazineDefaultCount { get; set; }

        public decimal? HeavyPistolMagazineCost { get; set; }
        public int? HeavyPistolMagazineCount { get; set; }
        public int? HeavyPistolMagazineDefaultCount { get; set; }

        public decimal? RevolverMagazineCost { get; set; }
        public int? RevolverMagazineCount { get; set; }
        public int? RevolverMagazineDefaultCount { get; set; }

        public decimal? MicroSmgMagazineCost { get; set; }
        public int? MicroSmgMagazineCount { get; set; }
        public int? MicroSmgMagazineDefaultCount { get; set; }

        public decimal? SmgMagazineCost { get; set; }
        public int? SmgMagazineCount { get; set; }
        public int? SmgMagazineDefaultCount { get; set; }

        public decimal? SmgMk2MagazineCost { get; set; }
        public int? SmgMk2MagazineCount { get; set; }
        public int? SmgMk2MagazineDefaultCount { get; set; }

        public decimal? MiniSmgMagazineCost { get; set; }
        public int? MiniSmgMagazineCount { get; set; }
        public int? MiniSmgMagazineDefaultCount { get; set; }

        public decimal? AssaultRifleMagazineCost { get; set; }
        public int? AssaultRifleMagazineCount { get; set; }
        public int? AssaultRifleMagazineDefaultCount { get; set; }

        public decimal? AssaultRifleMk2MagazineCost { get; set; }
        public int? AssaultRifleMk2MagazineCount { get; set; }
        public int? AssaultRifleMk2MagazineDefaultCount { get; set; }

        public decimal? SniperRifleMagazineCost { get; set; }
        public int? SniperRifleMagazineCount { get; set; }
        public int? SniperRifleMagazineDefaultCount { get; set; }

        public decimal? DoubleBarrelShotgunMagazineCost { get; set; }
        public int? DoubleBarrelShotgunMagazineCount { get; set; }
        public int? DoubleBarrelShotgunMagazineDefaultCount { get; set; }

        public decimal? PumpShotgunMagazineCost { get; set; }
        public int? PumpShotgunMagazineCount { get; set; }
        public int? PumpShotgunMagazineDefaultCount { get; set; }

        public decimal? SawnoffShotgunMagazineCost { get; set; }
        public int? SawnoffShotgunMagazineCount { get; set; }
        public int? SawnoffShotgunMagazineDefaultCount { get; set; }

        public decimal? MarijuanaCost { get; set; }
        public int? MarijuanaCount { get; set; }
        public int? MarijuanaDefaultCount { get; set; }

        public decimal? LsdCost { get; set; }
        public int? LsdCount { get; set; }
        public int? LsdDefaultCount { get; set; }

        public decimal? ExcstasyCost { get; set; }
        public int? ExcstasyCount { get; set; }
        public int? ExcstasyDefaultCount { get; set; }

        public decimal? AmphetamineCost { get; set; }
        public int? AmphetamineCount { get; set; }
        public int? AmphetamineDefaultCount { get; set; }

        public decimal? MetaamphetamineCost { get; set; }
        public int? MetaamphetamineCount { get; set; }
        public int? MetaamphetamineDefaultCount { get; set; }

        public decimal? CrackCost { get; set; }
        public int? CrackCount { get; set; }
        public int? CrackDefaultCount { get; set; }

        public decimal? CocaineCost { get; set; }
        public int? CocaineCount { get; set; }
        public int? CocaineDefaultCount { get; set; }

        public decimal? HasishCost { get; set; }
        public int? HasishCount { get; set; }
        public int? HasishDefaultCount { get; set; }

        public decimal? HeroinCost { get; set; }
        public int? HeroinCount { get; set; }
        public int? HeroinDefaultCount { get; set; }
    }
}