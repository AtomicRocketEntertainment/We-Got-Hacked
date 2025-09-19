public class Day_Two_FirstWrite_Rafael : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.NEWS);
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.DisablePlayerWriteEmail();
        EventManager.TicketObjectiveCompleted();
        context.ChangeWriteState(HistoryPartState.Part_Two);
    }
}
