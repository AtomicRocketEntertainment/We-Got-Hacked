public class Day_One_FirstTimeOpenedState_Rafael : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
        EventManager.SpawnEmail(EmailType.NEWS);
        context.ChangeSoftwareState(SoftwareState.Empty);
    }
}
