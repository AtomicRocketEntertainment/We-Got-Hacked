using System.Collections.Generic;

public class Day_One_WriteStateSetup_Rafael : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Rafael_Day_One, HistoryPartState.Part_One), new Day_One_FirstWrite_Rafael());
        stateHandlers.Add((Character.Rafael_Day_One, HistoryPartState.Part_Two), new Day_One_SecondWrite_Rafael());
    }
}
