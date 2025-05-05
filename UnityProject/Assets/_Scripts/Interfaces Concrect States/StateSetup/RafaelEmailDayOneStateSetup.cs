using System.Collections.Generic;

public class RafaelEmailDayOneStateSetup : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Rafael_Day_One, HistoryPartState.Part_One), new Day_One_EmailFirstChoiceResponse_Rafael());
        stateHandlers.Add((Character.Rafael_Day_One, HistoryPartState.Part_Two), new Day_One_EmailSecondChoiceResponse_Rafael());
    }
}
