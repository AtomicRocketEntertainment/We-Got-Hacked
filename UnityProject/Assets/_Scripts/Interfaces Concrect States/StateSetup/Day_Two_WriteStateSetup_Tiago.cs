using System.Collections.Generic;

public class Day_Two_WriteStateSetup_Tiago : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Tiago_Day_Two, HistoryPartState.Part_One), new Day_Two_FirstWrite_Tiago());
    }
}
