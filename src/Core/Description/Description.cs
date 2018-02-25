/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by Przemysław Postrach <przemyslaw.postrach@hotmail.com> December 2017
 */

using GTANetworkAPI;
using GTANetworkInternals;
using Serverside.Entities.Base;
using Serverside.Entities.Core;

namespace Serverside.Core.Description
{
    public class Description : GameEntity
    {
        private TextLabel DescriptionLabel { get; }

        public Description(AccountEntity player)
        {
            DescriptionLabel = NAPI.TextLabel.CreateTextLabel("", player.Client.Position, 10f, 1f, 1, new Color(255, 255, 255), true,
                player.Client.Dimension);

            DescriptionLabel.Color = new Color(192, 192, 192);
            DescriptionLabel.Collisionless = true;
            DescriptionLabel.Seethrough = true;
            DescriptionLabel.Invincible = true;
            DescriptionLabel.Transparency = 250;
            DescriptionLabel.AttachTo(player.Client, "SKEL_ROOT", new Vector3(50f, 1f, 1f), player.Client.Rotation);

            //Jak gracz zmieni wymiar to żeby miał opis np. jak wejdzie do interioru
            //to zmieniamy wymiar temu labelowi opisu

            player.CharacterEntity.OnPlayerDimensionChanged += (e, args) =>
            {
                NAPI.Entity.SetEntityDimension(DescriptionLabel.Handle, args.CurrentDimension);
            };
        }


        private string _value;

        public string Value
        {
            get => _value;
            set
            {
                _value = value;
                DescriptionLabel.Text = value;
            }
        }

        public void ResetCurrentDescription()
        {
            Value = string.Empty;
        }

        public override void Dispose()
        {
            DescriptionLabel.Detach(true);
            DescriptionLabel.Delete();
        }
    }
}