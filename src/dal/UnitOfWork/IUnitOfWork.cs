using System;
using System.Threading.Tasks;
using VRP.DAL.Database.Models.Account;
using VRP.DAL.Database.Models.Building;
using VRP.DAL.Database.Models.Character;
using VRP.DAL.Database.Models.CrimeBot;
using VRP.DAL.Database.Models.Group;
using VRP.DAL.Database.Models.Item;
using VRP.DAL.Database.Models.Misc;
using VRP.DAL.Database.Models.Telephone;
using VRP.DAL.Database.Models.Ticket;
using VRP.DAL.Database.Models.Vehicle;
using VRP.DAL.Database.Models.Warehouse;
using VRP.DAL.Interfaces;

namespace VRP.DAL.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IJoinableRepository<AccountModel> AccountsRepository { get; set; }
        IJoinableRepository<BuildingModel> BuildingsRepository { get; set; }
        IJoinableRepository<CharacterModel> CharactersRepository { get; set; }
        IJoinableRepository<CrimeBotModel> CrimeBotsRepository { get; set; }
        IJoinableRepository<GroupModel> GroupsRepository { get; set; }
        IJoinableRepository<GroupWarehouseItemModel> GroupWarehouseItemsRepository { get; set; }
        IJoinableRepository<GroupWarehouseOrderModel> GroupWarehouseOrdersRepository { get; set; }
        IJoinableRepository<ItemModel> ItemsRepository { get; set; }
        IJoinableRepository<PenaltyModel> PenaltiesRepository { get; set; }
        IJoinableRepository<TelephoneContactModel> TelephoneContactsRepository { get; set; }
        IRepository<TelephoneMessageModel> TelephoneMessagesRepository { get; set; }
        IJoinableRepository<VehicleModel> VehiclesRepository { get; set; }
        IJoinableRepository<WorkerModel> WorkersRepository { get; set; }
        IRepository<ZoneModel> ZonesRepository { get; set; }
        IJoinableRepository<GroupRankModel> GroupRanksRepository { get; set; }
        IJoinableRepository<TicketModel> TicketsRepository { get; set; }

        Task SaveAsync();
        void Save();
    }
}
