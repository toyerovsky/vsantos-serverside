using System.Threading.Tasks;
using VRP.BLL.UnitOfWork;
using VRP.DAL.Database;
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
using VRP.DAL.Repositories;

namespace VRP.DAL.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public IJoinableRepository<AccountModel> AccountsRepository { get; set; }
        public IJoinableRepository<BuildingModel> BuildingsRepository { get; set; }
        public IJoinableRepository<CharacterModel> CharactersRepository { get; set; }
        public IJoinableRepository<CrimeBotModel> CrimeBotsRepository { get; set; }
        public IJoinableRepository<GroupModel> GroupsRepository { get; set; }
        public IJoinableRepository<GroupWarehouseItemModel> GroupWarehouseItemsRepository { get; set; }
        public IJoinableRepository<GroupWarehouseOrderModel> GroupWarehouseOrdersRepository { get; set; }
        public IJoinableRepository<ItemModel> ItemsRepository { get; set; }
        public IJoinableRepository<PenaltyModel> PenaltiesRepository { get; set; }
        public IJoinableRepository<TelephoneContactModel> TelephoneContactsRepository { get; set; }
        public IRepository<TelephoneMessageModel> TelephoneMessagesRepository { get; set; }
        public IJoinableRepository<VehicleModel> VehiclesRepository { get; set; }
        public IJoinableRepository<WorkerModel> WorkersRepository { get; set; }
        public IRepository<ZoneModel> ZonesRepository { get; set; }
        public IJoinableRepository<GroupRankModel> GroupRanksRepository { get; set; }
        public IJoinableRepository<TicketModel> TicketsRepository { get; set; }

        private RoleplayContext Context { get; }

        public UnitOfWork(RoleplayContext context)
        {
            Context = context;
            AccountsRepository = new AccountsRepository(context);
            BuildingsRepository = new BuildingsRepository(context);
            CharactersRepository = new CharactersRepository(context);
            CrimeBotsRepository = new CrimeBotsRepository(context);
            GroupsRepository = new GroupsRepository(context);
            GroupWarehouseItemsRepository = new GroupWarehouseItemsRepository(context);
            GroupWarehouseOrdersRepository = new GroupWarehouseOrdersRepository(context);
            ItemsRepository = new ItemsRepository(context);
            PenaltiesRepository = new PenaltiesRepository(context);
            TelephoneContactsRepository = new TelephoneContactsRepository(context);
            TelephoneMessagesRepository = new TelephoneMessagesRepository(context);
            VehiclesRepository = new VehiclesRepository(context);
            WorkersRepository = new WorkersRepository(context);
            ZonesRepository = new ZonesRepository(context);
            GroupRanksRepository = new GroupRankRepository(context);
            TicketsRepository = new TicketsRepository(context);
        }

        public async Task SaveAsync()
        {
            await Context.SaveChangesAsync();
        }

        public void Save()
        {
            Context.SaveChanges();
        }

        public void Dispose()
        {
            Context.Dispose();
        }
    }
}