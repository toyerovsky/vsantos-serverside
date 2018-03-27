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