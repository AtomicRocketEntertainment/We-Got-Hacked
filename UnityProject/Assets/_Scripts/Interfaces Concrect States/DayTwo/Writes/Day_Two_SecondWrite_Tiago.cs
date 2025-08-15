public class Day_Two_SecondWrite_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.CreateEspecificEmail(PointEmailKey.FirstApkResponseDayTwoN1);
        context.ChangeWriteState(HistoryPartState.Part_Three);
    }
}
