public class Day_One_FirstWrite_Rafael : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.SpawnEmail(EmailType.NEWS);
        context.ChangeWriteState(HistoryPartState.Part_Two);
    }
}
