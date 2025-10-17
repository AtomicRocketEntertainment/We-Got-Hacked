public class Day_Three_SecondWrite_Eduardo : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
        EventManager.DisablePlayerWriteEmail();
        context.ChangeWriteState(HistoryPartState.Part_Three);
    }
}



