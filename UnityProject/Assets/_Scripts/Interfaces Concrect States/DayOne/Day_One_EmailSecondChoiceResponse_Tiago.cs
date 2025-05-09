public class Day_One_EmailSecondChoiceResponse_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.StoryBoardIsEnded();
    }
}
