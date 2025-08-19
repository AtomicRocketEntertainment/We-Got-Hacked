public class Day_Two_FirstWrite_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.SpawnEmail(EmailType.LORE);
        EventManager.DisablePlayerWriteEmail();
        context.ChangeWriteState(HistoryPartState.Part_Two);
    }
}
