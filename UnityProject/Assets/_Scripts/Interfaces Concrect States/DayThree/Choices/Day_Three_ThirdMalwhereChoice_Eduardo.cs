public class Day_Three_ThirdMalwhereChoice_Eduardo : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.EnablePlayerWriteEmail();
        EventManager.DisablePlayerSearchMalwhere();
        EventManager.TicketObjectiveCompleted();
        context.ChangeChoiceState(HistoryPartState.Part_Four);
    }
}


