using System.Collections.Generic;

public class Day_One_ChoiceStateSetup_Rafael : IChoiceStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IChoiceStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Rafael_Day_One, HistoryPartState.Part_One), new Day_One_FirstChoice_Rafael());
    }
}

