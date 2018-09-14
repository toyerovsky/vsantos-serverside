using System.ComponentModel;
namespace VRP.DAL.Enums
{
    public enum TicketStatusType
    {
        [Description("Oczekujący")]
        Expectant,
        [Description("W trakcie")]
        During,
        [Description("Zamknięte")]
        Closed
    }
}
