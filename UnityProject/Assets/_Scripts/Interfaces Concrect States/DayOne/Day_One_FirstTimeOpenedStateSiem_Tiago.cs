public class Day_One_FirstTimeOpenedStateSiem_Tiago : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        context.ChangeBlockedCanvasStatus(false);
        EventManager.SpawnEmail(EmailType.LORE);
        EventManager.SpawnEmail(EmailType.NEWS);
        context.ChangeSoftwareState(SoftwareState.Opened);
    }
}
