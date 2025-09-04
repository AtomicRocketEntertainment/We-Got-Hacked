public class Day_Two_FirstMalwhereChoice_Rafael : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
        EventManager.TicketObjectiveCompleted();
        context.ChangeChoiceState(HistoryPartState.Part_Two);
    }
}
