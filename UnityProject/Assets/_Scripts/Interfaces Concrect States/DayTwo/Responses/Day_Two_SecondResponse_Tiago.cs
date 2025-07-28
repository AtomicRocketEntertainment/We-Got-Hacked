public class Day_Two_SecondResponse_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        context.CheckApkSideStory(PointEmailKey.ThirdApkResponseDayTwoN1);
    }
}

