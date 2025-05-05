public class Day_One_EmailFirstChoiceResponse_Rafael : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.SpawnEmail(EmailType.NEWS);
    }
}