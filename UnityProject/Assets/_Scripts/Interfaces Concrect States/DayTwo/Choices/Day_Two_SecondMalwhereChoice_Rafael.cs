public class Day_Two_SecondMalwhereChoice_Rafael : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.EnablePlayerWriteEmail();
        EventManager.SpawnEmail(EmailType.NEWS);
    }
}
