/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using System.Collections.Generic;
using GTANetworkAPI;
using Serverside.Entities.Core;

namespace Serverside.Core.CharacterCreator
{
    public class CharacterCreator
    {
        private CharacterEntity CharacterEntity { get; set; }
        private Client Client => CharacterEntity.AccountEntity.Client;

        public CharacterCreator(CharacterEntity characterEntity)
        {
            CharacterEntity = characterEntity;

            if (CharacterEntity.DbModel.AccessoryId != null)
                AccessoryId = CharacterEntity.DbModel.AccessoryId.Value;
            if (CharacterEntity.DbModel.AccessoryTexture != null)
                AccessoryTexture = CharacterEntity.DbModel.AccessoryTexture.Value;
            if (CharacterEntity.DbModel.EarsId != null)
                EarsId = CharacterEntity.DbModel.EarsId.Value;
            if (CharacterEntity.DbModel.EarsTexture != null)
                EarsTexture = CharacterEntity.DbModel.EarsTexture.Value;
            if (CharacterEntity.DbModel.EyeBrowsOpacity != null)
                EyeBrowsOpacity = CharacterEntity.DbModel.EyeBrowsOpacity.Value;
            if (CharacterEntity.DbModel.EyebrowsId != null)
                EyeBrowsId = CharacterEntity.DbModel.EyebrowsId.Value;
            if (CharacterEntity.DbModel.FatherId != null)
                FatherId = CharacterEntity.DbModel.FatherId.Value;
            if (CharacterEntity.DbModel.FirstEyebrowsColor != null)
                FirstEyeBrowsColor = CharacterEntity.DbModel.FirstEyebrowsColor.Value;
            if (CharacterEntity.DbModel.SecondEyebrowsColor != null)
                SecondEyeBrowsColor = CharacterEntity.DbModel.SecondEyebrowsColor.Value;
            if (CharacterEntity.DbModel.FirstLipstickColor != null)
                FirstLipstickColor = CharacterEntity.DbModel.FirstLipstickColor.Value;
            if (CharacterEntity.DbModel.SecondLipstickColor != null)
                SecondLipstickColor = CharacterEntity.DbModel.SecondLipstickColor.Value;
            if (CharacterEntity.DbModel.FirstMakeupColor != null)
                FirstMakeupColor = CharacterEntity.DbModel.FirstMakeupColor.Value;
            if (CharacterEntity.DbModel.SecondMakeupColor != null)
                SecondMakeupColor = CharacterEntity.DbModel.SecondMakeupColor.Value;
            if (CharacterEntity.DbModel.MotherId != null)
                MotherId = CharacterEntity.DbModel.MotherId.Value;
            if (CharacterEntity.DbModel.GlassesId != null)
                GlassesId = CharacterEntity.DbModel.GlassesId.Value;
            if (CharacterEntity.DbModel.GlassesTexture != null)
                GlassesTexture = CharacterEntity.DbModel.GlassesTexture.Value;
            if (CharacterEntity.DbModel.HairId != null)
                HairId = CharacterEntity.DbModel.HairId.Value;
            if (CharacterEntity.DbModel.HairTexture != null)
                HairTexture = CharacterEntity.DbModel.HairTexture.Value;
            if (CharacterEntity.DbModel.HairColor != null)
                HairColor = CharacterEntity.DbModel.HairColor.Value;
            if (CharacterEntity.DbModel.HatId != null)
                HatId = CharacterEntity.DbModel.HatId.Value;
            if (CharacterEntity.DbModel.HatTexture != null)
                HatTexture = CharacterEntity.DbModel.HatTexture.Value;
            if (CharacterEntity.DbModel.LegsId != null)
                LegsId = CharacterEntity.DbModel.LegsId.Value;
            if (CharacterEntity.DbModel.LegsTexture != null)
                LegsTexture = CharacterEntity.DbModel.LegsTexture.Value;
            if (CharacterEntity.DbModel.LipstickOpacity != null)
                LipstickOpacity = CharacterEntity.DbModel.LipstickOpacity.Value;
            if (CharacterEntity.DbModel.MakeupId != null)
                MakeupId = CharacterEntity.DbModel.MakeupId.Value;
            if (CharacterEntity.DbModel.MakeupOpacity != null)
                MakeupOpacity = CharacterEntity.DbModel.MakeupOpacity.Value;
            if (CharacterEntity.DbModel.UndershirtId != null)
                UndershirtId = CharacterEntity.DbModel.UndershirtId.Value;
            if (CharacterEntity.DbModel.TorsoId != null)
                TorsoId = CharacterEntity.DbModel.TorsoId.Value;
            if (CharacterEntity.DbModel.TopTexture != null)
                TopTexture = CharacterEntity.DbModel.TopTexture.Value;
            if (CharacterEntity.DbModel.TopId != null)
                TopId = CharacterEntity.DbModel.TopId.Value;
            if (CharacterEntity.DbModel.ShoesTexture != null)
                FeetsTexture = CharacterEntity.DbModel.ShoesTexture.Value;
            if (CharacterEntity.DbModel.ShoesId != null)
                FeetsId = CharacterEntity.DbModel.ShoesId.Value;
            if (CharacterEntity.DbModel.ShapeMix != null)
                ShapeMix = CharacterEntity.DbModel.ShapeMix.Value;
        }

        public void Save()
        {
            CharacterEntity.DbModel.AccessoryId = AccessoryId;
            CharacterEntity.DbModel.AccessoryTexture = AccessoryTexture;
            CharacterEntity.DbModel.EarsId = EarsId;
            CharacterEntity.DbModel.EarsTexture = EarsTexture;
            CharacterEntity.DbModel.EyeBrowsOpacity = EyeBrowsOpacity;
            CharacterEntity.DbModel.EyebrowsId = EyeBrowsId;
            CharacterEntity.DbModel.FatherId = FatherId;
            CharacterEntity.DbModel.FirstEyebrowsColor = FirstEyeBrowsColor;
            CharacterEntity.DbModel.SecondEyebrowsColor = SecondEyeBrowsColor;
            CharacterEntity.DbModel.FirstLipstickColor = FirstLipstickColor;
            CharacterEntity.DbModel.SecondLipstickColor = SecondLipstickColor;
            CharacterEntity.DbModel.FirstMakeupColor = FirstMakeupColor;
            CharacterEntity.DbModel.SecondMakeupColor = SecondMakeupColor;
            CharacterEntity.DbModel.MotherId = MotherId;
            CharacterEntity.DbModel.GlassesId = GlassesId;
            CharacterEntity.DbModel.GlassesTexture = GlassesTexture;
            CharacterEntity.DbModel.HairId = HairId;
            CharacterEntity.DbModel.HairTexture = HairTexture;
            CharacterEntity.DbModel.HairColor = HairColor;
            CharacterEntity.DbModel.HatId = HatId;
            CharacterEntity.DbModel.LegsId = LegsId;
            CharacterEntity.DbModel.LegsTexture = LegsTexture;
            CharacterEntity.DbModel.LipstickOpacity = LipstickOpacity;
            CharacterEntity.DbModel.MakeupId = MakeupId;
            CharacterEntity.DbModel.MakeupOpacity = MakeupOpacity;
            CharacterEntity.DbModel.UndershirtId = UndershirtId;
            CharacterEntity.DbModel.TorsoId = TorsoId;
            CharacterEntity.DbModel.TopTexture = TopTexture;
            CharacterEntity.DbModel.TopId = TopId;
            CharacterEntity.DbModel.ShoesTexture = FeetsTexture;
            CharacterEntity.DbModel.ShoesId = FeetsId;
            CharacterEntity.DbModel.ShapeMix = ShapeMix;
            CharacterEntity.Save();
        }

        private byte _hairId;

        public byte HairId
        {
            get => _hairId;

            set
            {
                _hairId = value;
                NAPI.Player.SetPlayerClothes(Client, 2, value, 0);
            }
        }

        private byte _hairTexture;

        public byte HairTexture
        {
            get => _hairTexture;

            set
            {
                _hairTexture = value;
                NAPI.Player.SetPlayerClothes(Client, 2, _hairId, value);
            }
        }

        private byte _hairColor;

        public byte HairColor
        {
            get => _hairColor;
            set
            {
                _hairColor = value;
                NAPI.Player.SetPlayerHairColor(Client, value, value);
            }
        }

        private byte _motherId;

        public byte MotherId
        {
            get => _motherId;
            set
            {
                //SET_PED_HEAD_BLEND_DATA(Ped ped, byte shapeFirstID, byte shapeSecondID,
                //byte shapeThirdID, byte skinFirstID, byte skinSecondID, byte skinThirdID, float shapeMix,
                //float skinMix, float thirdMix, BOOL isParent)
                _motherId = value;
                NAPI.Player.SetPlayerHeadBlend(Client, new HeadBlend()
                {
                    ShapeFirst = value,
                    ShapeMix = ShapeMix,
                    ShapeSecond = FatherId,
                    SkinMix = SkinMix,
                });
            }
        }

        private byte _fatherId;

        public byte FatherId
        {
            get => _fatherId;
            set
            {
                _fatherId = value;
                NAPI.Player.SetPlayerHeadBlend(Client, new HeadBlend()
                {
                    ShapeFirst = MotherId,
                    ShapeMix = ShapeMix,
                    ShapeSecond = value,
                    SkinMix = SkinMix,
                });
            }
        }

        private float _shapeMix;

        public float ShapeMix
        {
            get => _shapeMix;
            set
            {
                _shapeMix = value;
                NAPI.Player.SetPlayerHeadBlend(Client, new HeadBlend()
                {
                    ShapeFirst = MotherId,
                    ShapeMix = value,
                    ShapeSecond = FatherId,
                    SkinMix = SkinMix,
                });
            }
        }

        private float _skinMix;

        public float SkinMix
        {
            get => _skinMix;
            set
            {
                _skinMix = value;
                NAPI.Player.SetPlayerHeadBlend(Client, new HeadBlend()
                {
                    ShapeFirst = MotherId,
                    ShapeMix = ShapeMix,
                    ShapeSecond = FatherId,
                    SkinMix = value,
                });
            }
        }

        private byte _eyeBrowsId;

        public byte EyeBrowsId
        {
            get => _eyeBrowsId;
            set
            {
                _eyeBrowsId = value;

                //SET_PED_HEAD_OVERLAY(Ped ped, byte overlayID, byte index, float opacity)
                NAPI.Player.SetPlayerHeadOverlay(Client, 2, new HeadOverlay()
                {
                    Index = value,
                    Opacity = EyeBrowsOpacity
                });
            }
        }

        private float _eyeBrowsOpacity;

        public float EyeBrowsOpacity
        {
            get => _eyeBrowsOpacity;
            set
            {
                _eyeBrowsOpacity = value;
                NAPI.Player.SetPlayerHeadOverlay(Client, 2, new HeadOverlay()
                {
                    Index = EyeBrowsId,
                    Opacity = value
                });
            }
        }

        private byte _firstEyeBrowsColor;

        public byte FirstEyeBrowsColor
        {
            get => _firstEyeBrowsColor;
            set
            {
                _firstEyeBrowsColor = value;
                //_SET_PED_HEAD_OVERLAY_COLOR(Ped ped, byte overlayID, byte colorType, byte colorID, byte secondColorID)
                NAPI.Native.SendNativeToPlayer(Client, Hash._SET_PED_HEAD_OVERLAY_COLOR, Client.Handle, 2, 1,
                    value, _secondEyeBrowsColor);
            }
        }

        private byte _secondEyeBrowsColor;

        public byte SecondEyeBrowsColor
        {
            get => _secondEyeBrowsColor;
            set
            {
                _secondEyeBrowsColor = value;
                NAPI.Native.SendNativeToPlayer(Client, Hash._SET_PED_HEAD_OVERLAY_COLOR, Client.Handle, 2, 1,
                    _firstEyeBrowsColor, value);
            }
        }

        private byte _lipstickId;

        public byte LipstickId
        {
            get => _lipstickId;
            set
            {
                _lipstickId = value;
                //SET_PED_HEAD_OVERLAY(Ped ped, byte overlayID, byte index, float opacity) 

                NAPI.Player.SetPlayerHeadOverlay(Client, 8, new HeadOverlay()
                {
                    Index = value,
                    Opacity = LipstickOpacity
                });
            }
        }

        private float _lipstickOpacity;

        public float LipstickOpacity
        {
            get => _lipstickOpacity;
            set
            {
                _lipstickOpacity = value;
                NAPI.Player.SetPlayerHeadOverlay(Client, 8, new HeadOverlay()
                {
                    Index = LipstickId,
                    Opacity = value
                });
            }
        }

        private byte _firstLipstickColor;

        public byte FirstLipstickColor
        {
            get => _firstLipstickColor;
            set
            {
                _firstLipstickColor = value;
                //_SET_PED_HEAD_OVERLAY_COLOR(Ped ped, byte overlayID, byte colorType, byte colorID, byte secondColorID)
                //2
                NAPI.Native.SendNativeToPlayer(Client, Hash._SET_PED_HEAD_OVERLAY_COLOR, Client.Handle, 8, 1,
                    value, _secondLipstickColor);
            }
        }

        private byte _secondLipstickColor;

        public byte SecondLipstickColor
        {
            get => _secondLipstickColor;
            set
            {
                _secondLipstickColor = value;
                //_SET_PED_HEAD_OVERLAY_COLOR(Ped ped, byte overlayID, byte colorType, byte colorID, byte secondColorID)
                //2
                NAPI.Native.SendNativeToPlayer(Client, Hash._SET_PED_HEAD_OVERLAY_COLOR, Client.Handle, 8, 1,
                    _firstEyeBrowsColor, value);
            }
        }

        private byte _makeupId;

        public byte MakeupId
        {
            get => _makeupId;
            set
            {
                _makeupId = value;
                //SET_PED_HEAD_OVERLAY(Ped ped, byte overlayID, byte index, float opacity) 
                NAPI.Player.SetPlayerHeadOverlay(Client, 4, new HeadOverlay()
                {
                    Index = value,
                    Opacity = MakeupOpacity
                });
            }
        }

        private float _makeupOpacity;

        public float MakeupOpacity
        {
            get => _makeupOpacity;
            set
            {
                _makeupOpacity = value;
                //SET_PED_HEAD_OVERLAY(Ped ped, byte overlayID, byte index, float opacity) 
                NAPI.Player.SetPlayerHeadOverlay(Client, 4, new HeadOverlay()
                {
                    Index = MakeupId,
                    Opacity = value
                });
            }
        }

        private byte _firstMakeupColor;

        public byte FirstMakeupColor
        {
            get => _firstMakeupColor;
            set
            {
                _firstMakeupColor = value;
                //_SET_PED_HEAD_OVERLAY_COLOR(Ped ped, byte overlayID, byte colorType, byte colorID, byte secondColorID)
                //0
                NAPI.Native.SendNativeToPlayer(Client, Hash._SET_PED_HEAD_OVERLAY_COLOR, Client.Handle, 4, 1,
                    value, _secondMakeupColor);
            }
        }

        private byte _secondMakeupColor;

        public byte SecondMakeupColor
        {
            get => _secondMakeupColor;
            set
            {
                _secondMakeupColor = value;
                //_SET_PED_HEAD_OVERLAY_COLOR(Ped ped, byte overlayID, byte colorType, byte colorID, byte secondColorID)
                //0
                NAPI.Native.SendNativeToPlayer(Client, Hash._SET_PED_HEAD_OVERLAY_COLOR, Client.Handle, 4, 1,
                    _firstMakeupColor, value);
            }
        }

        public void SetPlayerFaceFeatures(Dictionary<byte, float> faceFeatures)
        {
            if (faceFeatures.Count <= 21)
            {
                foreach (KeyValuePair<byte, float> item in faceFeatures)
                {
                    //_SET_PED_FACE_FEATURE(Ped ped, byte index, float scale)
                    NAPI.Native.SendNativeToPlayer(Client, Hash._SET_PED_FACE_FEATURE, Client.Handle, item.Key, item.Value);
                }
            }
        }

        private byte _legsId;

        public byte LegsId
        {
            get => _legsId;
            set
            {
                _legsId = value;
                NAPI.Player.SetPlayerClothes(Client, 4, value, _legsTexture);
            }
        }

        private byte _legsTexture;

        public byte LegsTexture
        {
            get => _legsTexture;
            set
            {
                _legsTexture = value;
                NAPI.Player.SetPlayerClothes(Client, 4, _legsId, value);
            }
        }

        private byte _feetsId;

        public byte FeetsId
        {
            get => _feetsId;
            set
            {
                _feetsId = value;
                NAPI.Player.SetPlayerClothes(Client, 6, value, _feetsTexture);
            }
        }

        private byte _feetsTexture;

        public byte FeetsTexture
        {
            get => _feetsTexture;
            set
            {
                _feetsTexture = value;
                NAPI.Player.SetPlayerClothes(Client, 6, _feetsId, value);
            }
        }

        private byte _accessoryId;

        public byte AccessoryId
        {
            get => _accessoryId;
            set
            {
                _accessoryId = value;
                NAPI.Player.SetPlayerClothes(Client, 7, value, _accessoryTexture);
            }
        }

        private byte _accessoryTexture;

        public byte AccessoryTexture
        {
            get => _accessoryTexture;
            set
            {
                _accessoryTexture = value;
                NAPI.Player.SetPlayerClothes(Client, 7, _accessoryId, value);
            }
        }

        private byte _hatId;

        public byte HatId
        {
            get => _hatId;
            set
            {
                _hatId = value;
                NAPI.Player.SetPlayerClothes(Client, 0, value, _hatTexture);
            }
        }

        private byte _hatTexture;

        public byte HatTexture
        {
            get => _hatTexture;
            set
            {
                _hatTexture = value;
                NAPI.Player.SetPlayerClothes(Client, 0, _hatId, value);
            }
        }

        private byte _glassesId;

        public byte GlassesId
        {
            get => _glassesId;
            set
            {
                _glassesId = value;
                NAPI.Player.SetPlayerClothes(Client, 1, value, _glassesTexture);
            }
        }

        private byte _glassesTexture;

        public byte GlassesTexture
        {
            get => _glassesTexture;
            set
            {
                _glassesTexture = value;
                NAPI.Player.SetPlayerClothes(Client, 1, _glassesId, value);
            }
        }

        private byte _earsId;

        public byte EarsId
        {
            get => _earsId;
            set
            {
                _earsId = value;
                NAPI.Player.SetPlayerClothes(Client, 2, value, _earsTexture);
            }
        }

        private byte _earsTexture;

        public byte EarsTexture
        {
            get => _earsTexture;
            set
            {
                _earsTexture = value;
                NAPI.Player.SetPlayerClothes(Client, 2, _earsId, value);
            }
        }

        private byte _topId;

        public byte TopId
        {
            get => _topId;
            set
            {
                _topId = value;
                NAPI.Player.SetPlayerClothes(Client, 11, value, _topTexture);
            }
        }

        private byte _topTexture;

        public byte TopTexture
        {
            get => _topTexture;
            set
            {
                _topTexture = value;
                NAPI.Player.SetPlayerClothes(Client, 11, _topId, value);
            }
        }

        private byte _torsoId;

        public byte TorsoId
        {
            get => _torsoId;
            set
            {
                _torsoId = value;
                NAPI.Player.SetPlayerClothes(Client, 3, value, 0);
            }
        }

        private byte _undershirtId;

        public byte UndershirtId
        {
            get => _undershirtId;
            set
            {
                _undershirtId = value;
                NAPI.Player.SetPlayerClothes(Client, 8, value, _undershirtTexture);
            }
        }

        private byte _undershirtTexture;

        public byte UndershirtTexture
        {
            get => _undershirtTexture;
            set
            {
                _undershirtTexture = value;
                NAPI.Player.SetPlayerClothes(Client, 8, _undershirtId, value);
            }
        }
    }
}