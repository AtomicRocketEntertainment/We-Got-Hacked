public class Day_One_FirstChoiceDesconex_Rafael : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.CorrectChoice();
        EventManager.SpawnEmail(EmailType.LORE);
        context.ChangeChoiceState(HistoryPartState.Part_Two);
    }
}
