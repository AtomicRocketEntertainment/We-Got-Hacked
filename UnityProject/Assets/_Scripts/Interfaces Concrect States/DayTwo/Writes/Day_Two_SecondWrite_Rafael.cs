public class Day_Two_SecondWrite_Rafael : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.NEWS);
        EventManager.DisablePlayerWriteEmail();
        EventManager.TicketObjectiveCompleted();
        context.ChangeWriteState(HistoryPartState.Part_Three);
    }
}
