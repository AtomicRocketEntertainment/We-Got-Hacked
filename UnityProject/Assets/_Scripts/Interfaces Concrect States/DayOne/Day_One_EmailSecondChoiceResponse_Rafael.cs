public class Day_One_EmailSecondChoiceResponse_Rafael : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.SpawnEmail(EmailType.NEWS);
    }
}