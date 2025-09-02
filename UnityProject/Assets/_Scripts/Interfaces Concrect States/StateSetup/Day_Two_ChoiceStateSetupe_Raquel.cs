using System.Collections.Generic;

public class Day_Two_ChoiceStateSetupe_Raquel : IChoiceStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IChoiceStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Raquel_Day_Two, HistoryPartState.Part_One), new Day_Two_FirstChoice_Raquel());
        stateHandlers.Add((Character.Raquel_Day_Two, HistoryPartState.Part_Two), new Day_Two_SecondChoice_Raquel());
        stateHandlers.Add((Character.Raquel_Day_Two, HistoryPartState.Part_Three), new Day_Two_ThirdChoice_Raquel());
    }
}

