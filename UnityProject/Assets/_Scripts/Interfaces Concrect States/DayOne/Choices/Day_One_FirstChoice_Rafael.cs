public class Day_One_FirstChoice_Rafael : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.TicketObjectiveCompleted();
        EventManager.SpawnEmail(EmailType.LORE);
    }
}
