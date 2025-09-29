public class Day_Three_SecondWrite_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        context.ChangeWriteState(HistoryPartState.Part_Three);
    }
}

