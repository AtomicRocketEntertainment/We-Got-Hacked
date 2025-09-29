public class Day_Three_FirstWrite_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
        context.ChangeWriteState(HistoryPartState.Part_Two);
    }
}

