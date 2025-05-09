using System.Collections.Generic;

public class TiagoEmailDayOneStateSetup : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Tiago_Day_One, HistoryPartState.Part_One), new Day_One_EmailFirstChoiceResponse_Tiago());
        stateHandlers.Add((Character.Tiago_Day_One, HistoryPartState.Part_Two), new Day_One_EmailSecondChoiceResponse_Tiago());
    }
}
