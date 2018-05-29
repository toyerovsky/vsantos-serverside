using VRP.Core.Interfaces;

namespace VRP.Core.Validators
{
    public class MoneyValidator : IValidator<decimal>
    {
        public bool IsValid(decimal value)
        {
            return value >= 0m;
        }
    }
}