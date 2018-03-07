namespace VRP.Serverside.Entities.Interfaces
{
    public interface IEntityFactory<TEntity, TModel>
    {
        TEntity Create(TModel model);
    }
}
