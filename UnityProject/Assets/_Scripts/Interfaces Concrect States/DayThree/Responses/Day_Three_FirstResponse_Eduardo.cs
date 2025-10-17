public class Day_Three_FirstResponse_Eduardo : IEmailStateHandler
{
    public void Handle(IEmailContext context)
    {
        EventManager.EnablePlayerWriteEmail();
        context.ChangeResponseState(HistoryPartState.Part_Two);
    }
}


