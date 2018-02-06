using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Serverside.Core.Database;
using Serverside.Core.Database.Models;
using Serverside.Core.Interfaces;

namespace Serverside.Core.Repositories
{
    public class GroupWarehouseItemsRepository : IRepository<GroupWarehouseItemModel>
    {
        private RoleplayContext Context { get; } = RolePlayContextFactory.NewContext();

        public void Insert(GroupWarehouseItemModel model) => Context.GroupWarehouseItems.Add(model);

        public bool Contains(GroupWarehouseItemModel model)
        {
            return Context.GroupWarehouseItems.Contains(model);
        }

        public void Update(GroupWarehouseItemModel model)
        {
            Context.Entry(model).State = EntityState.Modified;
        }

        public void Delete(long id)
        {
            var account = Context.GroupWarehouseItems.Find(id);
            Context.GroupWarehouseItems.Remove(account);
        }

        public GroupWarehouseItemModel Get(long id) => GetAll().Single(b => b.Id == id);

        public IEnumerable<GroupWarehouseItemModel> GetAll()
        {
            return Context.GroupWarehouseItems.ToList();
        }

        public void Save() => Context.SaveChanges();

        public void Dispose() => Context?.Dispose();
    }
}