public class Day_Two_ThirdChoice_Raquel : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.PlayerAnswerWrQuestion();
    }
}
