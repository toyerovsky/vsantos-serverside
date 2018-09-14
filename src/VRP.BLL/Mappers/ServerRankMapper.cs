using VRP.BLL.Interfaces;
using VRP.DAL.Enums;

namespace VRP.BLL.Mappers
{
    public class ServerRankMapper : IMapper<ServerRank, long>
    {
        public ServerRank Map(long source)
        {
            switch (source)
            {
                case 3:
                    return ServerRank.Uzytkownik;
                case 4:
                    return ServerRank.Zarzad;
                case 6:
                    return ServerRank.Uzytkownik;
                case 7:
                    return ServerRank.AdministratorRozgrywki;
                case 9:
                    return ServerRank.Donator;
                case 12:
                    return ServerRank.Support;
                case 13:
                    return ServerRank.AdministratorRozgrywki;
                default:
                    return ServerRank.Uzytkownik;
            }
        }
    }
}