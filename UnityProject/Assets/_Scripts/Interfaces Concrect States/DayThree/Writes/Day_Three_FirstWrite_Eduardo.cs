public class Day_Three_FirstWrite_Eduardo : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
        EventManager.DisablePlayerWriteEmail();
        context.ChangeWriteState(HistoryPartState.Part_Two);
    }
}


