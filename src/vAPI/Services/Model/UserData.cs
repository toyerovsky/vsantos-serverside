/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Enums;

namespace VRP.vAPI.Services.Model
{
    public class UserData
    {
        public int AccountId { get; set; }
        public int CharacterId { get; set; }
        public string Token { get; set; }
        public BroadcasterActionType ActionType { get; set; }
    }
}