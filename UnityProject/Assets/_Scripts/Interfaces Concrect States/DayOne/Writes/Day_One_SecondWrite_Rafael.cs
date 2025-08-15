public class Day_One_SecondWrite_Rafael : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
        EventManager.SpawnEmail(EmailType.NEWS);
        EventManager.TicketObjectiveCompleted();
    }
}
