using System.Collections.Generic;

public class Day_Three_ChoiceStateSetup_Raquel : IChoiceStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IChoiceStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Raquel_Day_Three, HistoryPartState.Part_One), new Day_Three_FirstChoice_Raquel());
        stateHandlers.Add((Character.Raquel_Day_Three, HistoryPartState.Part_Two), new Day_Three_SecondChoice_Raquel());
        stateHandlers.Add((Character.Raquel_Day_Three, HistoryPartState.Part_Three), new Day_Three_ThirdChoice_Raquel());
    }
}



