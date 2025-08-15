public class Day_One_FirstWrite_Raquel : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.ShowStoryBoard();
    }
}
