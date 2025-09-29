public class Day_Three_FirstResponse_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.CreateEspecificEmail(PointEmailKey.LOREA05DAY3);
    }
}

