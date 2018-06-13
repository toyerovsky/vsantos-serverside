using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using VRP.Core.Database;
using VRP.Core.Database.Models.Character;
using VRP.Core.Interfaces;
using VRP.Core.Repositories.Base;

namespace VRP.Core.Repositories
{
    public class CharacterLooksRepository : Repository<RoleplayContext, CharacterLookModel>, IJoinableRepository<CharacterLookModel>
    {
        public CharacterLooksRepository(RoleplayContext context) : base(context)
        {
        }

        public CharacterLooksRepository() : base(Singletons.RoleplayContextFactory.Create())
        {
        }

        public override CharacterLookModel Get(Func<CharacterLookModel, bool> func) => GetAll(func).FirstOrDefault();

        public override IEnumerable<CharacterLookModel> GetAll(Func<CharacterLookModel, bool> func = null)
        {
            IEnumerable<CharacterLookModel> characterLooks = func != null ?
                Context.CharacterLooks.Where(func) :
                Context.CharacterLooks;

            return characterLooks;
        }

        public CharacterLookModel JoinAndGet(int id) => JoinAndGetAll(characterLook => characterLook.Id == id).SingleOrDefault();

        public CharacterLookModel JoinAndGet(Expression<Func<CharacterLookModel, bool>> expression) => JoinAndGetAll(expression).FirstOrDefault();

        public IEnumerable<CharacterLookModel> JoinAndGetAll(Expression<Func<CharacterLookModel, bool>> expression)
        {
            IQueryable<CharacterLookModel> characterLooks = expression != null ?
                Context.CharacterLooks.Where(expression) :
                Context.CharacterLooks;

            return characterLooks
                .Include(characterLook => characterLook.Character);
        }
    }
}