public class Day_Three_Part_Two_SecondChoice_Raquel : IChoiceStateHandler
{
    public void Handle(IChoiceContext context)
    {
        EventManager.PlayerAnswerWrQuestion();
        context.ChangeChoiceState(HistoryPartState.Part_Three);
    }
}



