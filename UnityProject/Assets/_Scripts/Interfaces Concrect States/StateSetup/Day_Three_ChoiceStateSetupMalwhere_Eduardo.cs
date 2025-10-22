using System.Collections.Generic;

public class Day_Three_ChoiceStateSetupMalwhere_Eduardo : IChoiceStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IChoiceStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Eduardo_Day_Three, HistoryPartState.Part_One), new Day_Three_FirstMalwhereChoice_Eduardo());
        stateHandlers.Add((Character.Eduardo_Day_Three, HistoryPartState.Part_Two), new Day_Three_SecondMalwhereChoice_Eduardo());
        stateHandlers.Add((Character.Eduardo_Day_Three, HistoryPartState.Part_Three), new Day_Three_ThirdMalwhereChoice_Eduardo());
    }
}
