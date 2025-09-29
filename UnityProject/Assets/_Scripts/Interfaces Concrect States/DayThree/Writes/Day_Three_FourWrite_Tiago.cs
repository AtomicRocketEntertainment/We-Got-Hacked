public class Day_Three_FourWrite_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.CreateEspecificEmail(PointEmailKey.LOREA04DAY3);
    }
}

