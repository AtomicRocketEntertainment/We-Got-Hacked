public class Day_One_EmailFirstChoiceResponse_Rafael : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.SPAM);
        EventManager.SpawnEmail(EmailType.NEWS);
        context.ChangeSoftwareState(HistoryPartState.Part_Two);
    }
}