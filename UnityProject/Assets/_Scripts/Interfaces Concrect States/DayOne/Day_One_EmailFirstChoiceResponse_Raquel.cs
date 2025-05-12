public class Day_One_EmailFirstChoiceResponse_Raquel : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.ShowStoryBoard();
    }
}
