using System.Collections.Generic;

public class Day_Three_WriteStateSetup_Tiago : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Tiago_Day_Three, HistoryPartState.Part_One), new Day_Three_FirstWrite_Tiago());
        stateHandlers.Add((Character.Tiago_Day_Three, HistoryPartState.Part_Two), new Day_Three_SecondWrite_Tiago());
        stateHandlers.Add((Character.Tiago_Day_Three, HistoryPartState.Part_Three), new Day_Three_ThirdWrite_Tiago());
        stateHandlers.Add((Character.Tiago_Day_Three, HistoryPartState.Part_Four), new Day_Three_FourWrite_Tiago());
    }
}

