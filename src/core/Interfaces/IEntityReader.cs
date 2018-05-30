namespace VRP.Core.Interfaces
{
    public interface IEntityReader<T>
    {
        T GetAll();
        T Get();
    }
}