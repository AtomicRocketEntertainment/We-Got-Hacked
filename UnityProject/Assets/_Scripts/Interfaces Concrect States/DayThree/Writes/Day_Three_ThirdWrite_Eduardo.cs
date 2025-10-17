public class Day_Three_ThirdWrite_Eduardo : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.DisablePlayerWriteEmail();
        context.ChangeWriteState(HistoryPartState.Part_Four);
    }
}



