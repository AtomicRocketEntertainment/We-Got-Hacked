public class Day_One_EmailFirstChoiceResponse_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.SpawnEmail(EmailType.NEWS);
    }
}
