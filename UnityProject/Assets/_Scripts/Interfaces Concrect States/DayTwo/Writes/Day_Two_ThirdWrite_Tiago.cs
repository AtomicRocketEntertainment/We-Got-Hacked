public class Day_Two_ThirdWrite_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.CreateEspecificEmail(PointEmailKey.RaquelEmailAboutApkConversationWithTiago);
    }
}
