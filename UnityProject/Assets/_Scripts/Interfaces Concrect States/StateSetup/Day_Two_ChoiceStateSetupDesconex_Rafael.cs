using System.Collections.Generic;

public class Day_Two_ChoiceStateSetupDesconex_Rafael : IChoiceStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IChoiceStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Rafael_Day_Two, HistoryPartState.Part_One), new Day_Two_FirstDesconexChoice_Rafael());
    }
}


