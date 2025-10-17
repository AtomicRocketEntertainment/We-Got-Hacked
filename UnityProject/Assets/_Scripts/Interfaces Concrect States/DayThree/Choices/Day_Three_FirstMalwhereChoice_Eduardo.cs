public class Day_Three_FirstMalwhereChoice_Eduardo : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
        EventManager.SpawnEmail(EmailType.NEWS);
        EventManager.DisablePlayerSearchMalwhere();
        EventManager.MakePlayerThink(ThoughtKey.ThinkAboutIPFoundOnMalwhere);
        context.ChangeChoiceState(HistoryPartState.Part_Two);
    }
}
