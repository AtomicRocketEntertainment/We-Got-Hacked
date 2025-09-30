public class Day_Three_SecondWrite_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.SpawnEmail(EmailType.LORE);
        context.ChangeWriteState(HistoryPartState.Part_Three);
    }
}

