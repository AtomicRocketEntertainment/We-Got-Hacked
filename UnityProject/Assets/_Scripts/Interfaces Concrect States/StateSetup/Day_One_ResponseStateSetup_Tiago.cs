using System.Collections.Generic;

public class Day_One_ResponseStateSetup_Tiago : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Tiago_Day_One, HistoryPartState.Part_One), new Day_One_FirstResponse_Tiago());
    }
}
