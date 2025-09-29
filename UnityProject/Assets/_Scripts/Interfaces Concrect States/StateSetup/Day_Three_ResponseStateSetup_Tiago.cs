using System.Collections.Generic;

public class Day_Three_ResponseStateSetup_Tiago : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Tiago_Day_Three, HistoryPartState.Part_One), new Day_Three_FirstResponse_Tiago());
    }
}
