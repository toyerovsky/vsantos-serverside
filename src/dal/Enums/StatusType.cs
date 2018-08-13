using System;
using System.ComponentModel;
namespace VRP.DAL.Enums
{
    public enum StatusType
    {
        [Description("Oczekujący")]
        Expectant,
        [Description("W trakcie")]
        During,
        [Description("Zamknięte")]
        Closed
    }
}
