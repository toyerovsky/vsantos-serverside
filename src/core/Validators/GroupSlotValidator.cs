using VRP.Core.Interfaces;

namespace VRP.Core.Validators
{
    public class GroupSlotValidator : IValidator<byte>
    {
        public bool IsValid(byte value)
        {
            return value <= 3 && value >= 1;
        }
    }
}