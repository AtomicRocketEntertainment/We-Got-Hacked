public class Day_Two_ThirdWrite_Rafael : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.ShowStoryBoard();
    }
}
