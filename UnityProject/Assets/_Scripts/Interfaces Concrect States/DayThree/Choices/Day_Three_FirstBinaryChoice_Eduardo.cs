public class Day_Three_FirstBinaryChoice_Eduardo : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
        EventManager.SpawnEmail(EmailType.SPAM);
        context.ChangeChoiceState(HistoryPartState.Part_Two);
    }
}

