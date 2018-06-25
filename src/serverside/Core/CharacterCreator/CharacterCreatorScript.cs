/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using System;
using System.Globalization;
using GTANetworkAPI;
using VRP.DAL.Database.Models.Character;
using VRP.Serverside.Constant;

using VRP.Serverside.Core.Extensions;

namespace VRP.Serverside.Core.CharacterCreator
{
    public class CharacterCreatorScript : Script
    {

        #region Subskrypcja zdarzenia, i dopasowanie zmienianego obiektu
        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            CharacterCreator characterCreator = sender.GetAccountEntity().CharacterEntity.CharacterCreator;
            if (characterCreator == null) return;

            switch (eventName)
            {
                case "OnCreatorFatherChanged":
                    characterCreator.FatherId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorMotherChanged":
                    characterCreator.MotherId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorShapeMixChanged":
                    characterCreator.ShapeMix =
                        float.Parse(arguments[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    break;
                case "OnCreatorSkinMixChanged":
                    characterCreator.SkinMix =
                        float.Parse(arguments[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    break;
                case "OnCreatorHairChanged":
                    characterCreator.HairId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorHairTextureChanged":
                    characterCreator.HairTexture = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorHairColorChanged":
                    characterCreator.HairColor = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorMakeupChanged":
                    characterCreator.MakeupId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorFirstMakeupColorChanged":
                    characterCreator.FirstMakeupColor = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorSecondMakeupColorChanged":
                    characterCreator.SecondMakeupColor = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorMakeupOpacityChanged":
                    characterCreator.MakeupOpacity =
                        float.Parse(arguments[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    break;
                case "OnCreatorEyeBrowsChanged":
                    characterCreator.EyeBrowsId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorFirstEyeBrowsColorChanged":
                    characterCreator.FirstEyeBrowsColor = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorSecondEyeBrowsColorChanged":
                    characterCreator.SecondEyeBrowsColor = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorEyeBrowsOpacityChanged":
                    characterCreator.EyeBrowsOpacity =
                        float.Parse(arguments[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    break;
                case "OnCreatorLipstickChanged":
                    characterCreator.LipstickId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorFirstLipstickColorChanged":
                    characterCreator.FirstLipstickColor = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorSecondLipstickColorChanged":
                    characterCreator.SecondLipstickColor = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorLipstickOpacityChanged":
                    characterCreator.LipstickOpacity =
                        float.Parse(arguments[0].ToString(), CultureInfo.InvariantCulture.NumberFormat);
                    break;
                case "OnCreatorFeetsChanged":
                    characterCreator.FeetsId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorFeetsTextureChanged":
                    characterCreator.FeetsTexture = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorLegsChanged":
                    characterCreator.LegsId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorLegsTextureChanged":
                    characterCreator.LegsTexture = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorTopChanged":
                    characterCreator.TopId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorTopTextureChanged":
                    characterCreator.TopTexture = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorHatChanged":
                    characterCreator.HatId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorHatTextureChanged":
                    characterCreator.HatTexture = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorGlassesChanged":
                    characterCreator.GlassesId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorGlassesTextureChanged":
                    characterCreator.GlassesTexture = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorEarsChanged":
                    characterCreator.EarsId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorEarsTextureChanged":
                    characterCreator.EarsTexture = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorAccessoryChanged":
                    characterCreator.AccessoryId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorAccessoryTextureChanged":
                    characterCreator.AccessoryTexture = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorTorsoChanged":
                    characterCreator.TorsoId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorUndershirtChanged":
                    characterCreator.UndershirtId = Convert.ToByte(arguments[0]);
                    break;
                case "OnCreatorUndershirtTextureChanged":
                    characterCreator.UndershirtTexture = Convert.ToByte(arguments[0]);
                    break;
            }
        }

        #endregion

        [Command("kreator")]
        public void Creator(Client sender)
        {
            NAPI.Player.SetPlayerSkin(sender, PedHash.FreemodeMale01);

            CharacterModel dbModel = sender.GetAccountEntity().CharacterEntity.DbModel;

            sender.TriggerEvent("OnPlayerCreateCharacter", Clothes.Mens, Clothes.Womans,
                dbModel.Gender ? Clothes.MensHairId : Clothes.WomansHairId, dbModel.Gender ? Clothes.MenFeet : Clothes.WomanFeets,
                dbModel.Gender ? Clothes.MenLegs : Clothes.WomanLegs, dbModel.Gender ? Clothes.MenLegs : Clothes.WomanLegs,
                dbModel.Gender ? Clothes.MenHats : Clothes.WomanHats, dbModel.Gender ? Clothes.MenGlasses : Clothes.WomanGlasses,
                dbModel.Gender ? Clothes.MenEars : Clothes.WomanEars, dbModel.Gender ? Clothes.MenAccesories : Clothes.WomanAccesories,
                $"{dbModel.Name} {dbModel.Surname}");


            sender.GetAccountEntity().CharacterEntity.CharacterCreator = new CharacterCreator(sender.GetAccountEntity().CharacterEntity)
            {
                FatherId = 0,
                MotherId = 0
            };
        }
    }
}