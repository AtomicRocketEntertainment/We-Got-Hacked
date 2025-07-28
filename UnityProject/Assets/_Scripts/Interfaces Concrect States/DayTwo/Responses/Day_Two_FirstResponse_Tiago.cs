public class Day_Two_FirstResponse_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        context.CheckApkSideStory(PointEmailKey.SecondApkResponseDayTwoN1);
    }
}
