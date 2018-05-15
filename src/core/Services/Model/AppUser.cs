/* Copyright (C) Przemysław Postrach - All Rights Reserved
 * Unauthorized copying of this file, via any medium is strictly prohibited
 * Proprietary and confidential
 * Written by V Role Play team <contact@v-rp.pl> December 2017
 */

namespace VRP.Core.Services.Model
{
    public class AppUser
    {
        public int UserAccountId { get; set; }
        public int SelectedCharacterId { get; set; } = -1;
        public string Email { get; set; }
    }
}