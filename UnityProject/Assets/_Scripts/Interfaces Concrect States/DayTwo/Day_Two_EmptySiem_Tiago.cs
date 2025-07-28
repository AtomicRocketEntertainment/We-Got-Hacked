public class Day_Two_EmptySiem_Tiago : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
        context.ChangeSoftwareState(SoftwareState.FullAccess);
    }
}
