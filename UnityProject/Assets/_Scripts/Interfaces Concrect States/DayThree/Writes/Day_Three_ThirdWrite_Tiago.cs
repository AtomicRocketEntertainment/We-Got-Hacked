public class Day_Three_ThirdWrite_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.CreateEspecificEmail(PointEmailKey.LOREA02DAY3);
        context.ChangeWriteState(HistoryPartState.Part_Four);
    }
}
