using System.Collections.Generic;

public class Day_Two_ResponseStateSetup_Tiago : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Tiago_Day_Two, HistoryPartState.Part_One), new Day_Two_FirstResponse_Tiago());
        stateHandlers.Add((Character.Tiago_Day_Two, HistoryPartState.Part_Two), new Day_Two_SecondResponse_Tiago());
        stateHandlers.Add((Character.Tiago_Day_Two, HistoryPartState.Part_Three), new Day_Two_ThirdResponse_Tiago());
    }
}
