public class Day_Two_ThirdResponse_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.SpawnEmail(EmailType.LORE);
    }
}
