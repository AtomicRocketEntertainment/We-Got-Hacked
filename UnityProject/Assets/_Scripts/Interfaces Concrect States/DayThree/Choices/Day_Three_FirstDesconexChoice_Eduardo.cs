public class Day_Three_FirstDesconexChoice_Eduardo : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.TicketObjectiveCompleted();
        context.ChangeChoiceState(HistoryPartState.Part_Two);
    }
}





