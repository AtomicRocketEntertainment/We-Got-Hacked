public class Day_Two_FirstChoice_Raquel : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.PlayerAnswerWrQuestion();
        context.ChangeChoiceState(HistoryPartState.Part_Two);
    }
}
