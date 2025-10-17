using System.Collections.Generic;

public class Day_Three_WriteStateSetup_Eduardo : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Eduardo_Day_Three, HistoryPartState.Part_One), new Day_Three_FirstWrite_Eduardo());
        stateHandlers.Add((Character.Eduardo_Day_Three, HistoryPartState.Part_Two), new Day_Three_SecondWrite_Eduardo());
        stateHandlers.Add((Character.Eduardo_Day_Three, HistoryPartState.Part_Three), new Day_Three_ThirdWrite_Eduardo());
        stateHandlers.Add((Character.Eduardo_Day_Three, HistoryPartState.Part_Four), new Day_Three_FourWrite_Eduardo());
    }
}


