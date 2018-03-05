using System;
using System.Collections.Generic;
using System.Text;

namespace Serverside.Entities.Interfaces
{
    public interface IEntityFactory<TEntity, TModel>
    {
        TEntity Create(TModel model);
    }
}
