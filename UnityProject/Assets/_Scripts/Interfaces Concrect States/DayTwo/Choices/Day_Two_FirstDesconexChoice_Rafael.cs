public class Day_Two_FirstDesconexChoice_Rafael : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.CorrectChoice();
        EventManager.EnablePlayerWriteEmail();
        EventManager.TicketObjectiveCompleted();
        EventManager.ChangeRestoreState(RestoreState.None);
        context.ChangeChoiceState(HistoryPartState.Part_Two);
    }
}

