/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

using VRP.Core.Database.Models;

namespace VRP.vAPI.Model
{
    public class AppUser
    {
        public AccountModel UserAccount { get; set; }
        public CharacterModel SelectedCharacter { get; set; }

        public AppUser(AccountModel userAccount, CharacterModel selectedCharacter = null)
        {
            UserAccount = userAccount;
            SelectedCharacter = selectedCharacter;
        }
    }
}