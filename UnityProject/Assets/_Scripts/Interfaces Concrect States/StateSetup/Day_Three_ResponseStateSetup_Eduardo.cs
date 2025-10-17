using System.Collections.Generic;

public class Day_Three_ResponseStateSetup_Eduardo : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Eduardo_Day_Three, HistoryPartState.Part_One), new Day_Three_FirstResponse_Eduardo());
    }
}

