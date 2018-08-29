namespace VRP.BLL.Interfaces
{
    public interface IMapper<out TDestination, in TSource>
    {
        TDestination Map(TSource source);
    }
}