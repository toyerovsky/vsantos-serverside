using VRP.Core.Interfaces;

namespace VRP.Core.Validators
{
    public class CellphoneNumberValidator : IValidator<string>
    {
        public bool IsValid(string value)
        {
            return int.TryParse(value, out int converted) 
                   && converted.ToString().Length > 0 
                   && converted.ToString().Length <= 6;
        }
    }
}