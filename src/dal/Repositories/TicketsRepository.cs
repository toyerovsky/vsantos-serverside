using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using VRP.DAL.Database;
using VRP.DAL.Database.Models.Ticket;
using VRP.DAL.Interfaces;
using VRP.DAL.Repositories.Base;

namespace VRP.DAL.Repositories
{
    public class TicketsRepository : Repository<RoleplayContext, TicketModel>, IJoinableRepository<TicketModel>
    {
       public TicketsRepository(RoleplayContext context) : base(context)
       {
       }

        public override TicketModel Get(Func<TicketModel, bool> func) => GetAll(func).FirstOrDefault();
        public override async Task<TicketModel> GetAsync(Func<TicketModel, bool> func)
        {
            throw new NotImplementedException();
        }


        public override IEnumerable<TicketModel> GetAll(Func<TicketModel, bool> func = null)
        {
            IEnumerable<TicketModel> tickets = func != null ? Context.Tickets.Where(func) : Context.Tickets;
            return tickets;
        }

        public TicketModel JoinAndGet(int id) => JoinAndGetAll(ticket => ticket.Id == id).SingleOrDefault();
        public async Task<TicketModel> JoinAndGetAsync(int id)
        {
            return await JoinAndGetAll(account => account.Id == id).AsQueryable().SingleOrDefaultAsync();
        }


        public TicketModel JoinAndGet(Expression<Func<TicketModel, bool>> expression = null) =>
            JoinAndGetAll(expression).FirstOrDefault();

        public async Task<TicketModel> JoinAndGetAsync(Expression<Func<TicketModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().FirstOrDefaultAsync();
        }


        public IEnumerable<TicketModel> JoinAndGetAll(Expression<Func<TicketModel, bool>> expression = null)
        {
            IQueryable<TicketModel> tickets = expression != null ? Context.Tickets.Where(expression) : Context.Tickets;

            return tickets
                .Include(ticket => ticket.MessageContent)
                .Include(ticket => ticket.InvolvedAccounts)
                    .ThenInclude(ticket => ticket.User)
                .Include(ticket => ticket.InvolvedAdmins)
                    .ThenInclude(ticket => ticket.Admin);
        }

        public async Task<IEnumerable<TicketModel>> JoinAndGetAllAsync(Expression<Func<TicketModel, bool>> expression = null)
        {
            return await JoinAndGetAll(expression).AsQueryable().ToArrayAsync();
        }
    }
}
