using System.ComponentModel;

namespace VRP.DAL.Enums
{
    public enum TicketType
    {
        [Description("Bug na forum")]
        BugForum,
        [Description("Bug w grze")]
        BugGame,
        [Description("Wniosek FCK")]
        Fck,
        [Description("Wniosek FCJ")]
        Fcj,
        [Description("Inne")]
        Other
    }
}
