using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace VRP.Core.Database.Models.Character
{
    public class CharacterLookModel
    {
        public int Id { get; set; }

        public byte? AccessoryId { get; set; }
        public byte? AccessoryTexture { get; set; }
        public byte? EarsId { get; set; }
        public byte? EarsTexture { get; set; }
        public byte? EyebrowsId { get; set; }
        public float? EyeBrowsOpacity { get; set; }
        public byte? FatherId { get; set; }
        public byte? ShoesId { get; set; }
        public byte? ShoesTexture { get; set; }
        public byte? FirstEyebrowsColor { get; set; }
        public byte? FirstLipstickColor { get; set; }
        public byte? FirstMakeupColor { get; set; }
        public byte? GlassesId { get; set; }
        public byte? GlassesTexture { get; set; }
        public byte? HairId { get; set; }
        public byte? HairTexture { get; set; }
        public byte? HairColor { get; set; }
        public byte? HatId { get; set; }
        public byte? HatTexture { get; set; }
        public byte? LegsId { get; set; }
        public byte? LegsTexture { get; set; }
        public float? LipstickOpacity { get; set; }
        public byte? MakeupId { get; set; }
        public float? MakeupOpacity { get; set; }
        public byte? MotherId { get; set; }
        public byte? SecondEyebrowsColor { get; set; }
        public byte? SecondLipstickColor { get; set; }
        public byte? SecondMakeupColor { get; set; }
        public float? ShapeMix { get; set; }
        public byte? TopId { get; set; }
        public byte? TopTexture { get; set; }
        public byte? TorsoId { get; set; }
        public byte? UndershirtId { get; set; }

        // navigation properties
        public virtual CharacterModel CharacterModel { get; set; }
    }
}
