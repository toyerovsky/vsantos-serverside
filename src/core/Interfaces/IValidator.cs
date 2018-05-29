namespace VRP.Core.Interfaces
{
    public interface IValidator<in T>
    {
        bool IsValid(T value);
    }
}