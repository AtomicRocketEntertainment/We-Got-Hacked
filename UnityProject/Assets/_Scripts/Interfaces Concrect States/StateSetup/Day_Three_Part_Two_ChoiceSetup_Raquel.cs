using System.Collections.Generic;

public class Day_Three_Part_Two_ChoiceSetup_Raquel : IChoiceStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IChoiceStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Raquel_Day_Three_Part_Two, HistoryPartState.Part_One), new Day_Three_Part_Two_FirstChoice_Raquel());
        stateHandlers.Add((Character.Raquel_Day_Three_Part_Two, HistoryPartState.Part_Two), new Day_Three_Part_Two_SecondChoice_Raquel());
    }
}


