/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System;
using System.Globalization;
using GTANetworkAPI;
using Serverside.Constant;
using Serverside.Core;
using Serverside.Core.Extensions;

namespace Serverside.CharacterCreator
{
    public class CharacterCreatorScript : Script
    {
        public CharacterCreatorScript()
        {
            Event.OnResourceStart += API_onResourceStart;
        }

        private void API_onResourceStart()
        {
            Tools.ConsoleOutput($"[{nameof(CharacterCreatorScript)}] {ConstantMessages.ResourceStartMessage}", ConsoleColor.DarkMagenta);
        }

        #region Subskrypcja zdarzenia, i dopasowanie zmienianego obiektu
        private void Event_OnClientEventTrigger(Client sender, string eventName, params object[] arguments)
        {
            var characterCreator = sender.GetAccountEntity().CharacterEntity.CharacterCreator;
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

            var dbModel = sender.GetAccountEntity().CharacterEntity.DbModel;

            sender.TriggerEvent("OnPlayerCreateCharacter", ConstantClothes.Mens, ConstantClothes.Womans,
                dbModel.Gender ? ConstantClothes.MensHairId : ConstantClothes.WomansHairId, dbModel.Gender ? ConstantClothes.MenFeet : ConstantClothes.WomanFeets,
                dbModel.Gender ? ConstantClothes.MenLegs : ConstantClothes.WomanLegs, dbModel.Gender ? ConstantClothes.MenLegs : ConstantClothes.WomanLegs,
                dbModel.Gender ? ConstantClothes.MenHats : ConstantClothes.WomanHats, dbModel.Gender ? ConstantClothes.MenGlasses : ConstantClothes.WomanGlasses,
                dbModel.Gender ? ConstantClothes.MenEars : ConstantClothes.WomanEars, dbModel.Gender ? ConstantClothes.MenAccesories : ConstantClothes.WomanAccesories,
                $"{dbModel.Name} {dbModel.Surname}");


            sender.GetAccountEntity().CharacterEntity.CharacterCreator = new CharacterCreator(sender.GetAccountEntity().CharacterEntity)
            {
                FatherId = 0,
                MotherId = 0
            };
        }
    }
}