public class Day_Three_ThirdChoice_Raquel : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.PlayerAnswerWrQuestion();
        context.ChangeChoiceState(HistoryPartState.Part_Four);
    }
}





