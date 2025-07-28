public class Day_One_FirstWrite_Tiago : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.ShowStoryBoard();
    }
}
