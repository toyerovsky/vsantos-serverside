namespace VRP.Serverside.Interfaces
{
    public interface IEntityFactory<TEntity, TModel>
    {
        TEntity Create(TModel model);
    }
}
