public class Day_One_EmailFirstChoiceResponse_Tiago : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.SpawnEmail(EmailType.NEWS);
    }
}
