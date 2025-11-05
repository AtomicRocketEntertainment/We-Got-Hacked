using System.Collections.Generic;

public class Day_Three_ChoiceStateDesconexSetup_Eduardo : IChoiceStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IChoiceStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Eduardo_Day_Three, HistoryPartState.Part_One), new Day_Three_FirstDesconexChoice_Eduardo());
    }
}



