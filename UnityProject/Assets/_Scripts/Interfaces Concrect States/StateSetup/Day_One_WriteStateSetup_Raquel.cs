using System.Collections.Generic;

public class Day_One_WriteStateSetup_Raquel : IEmailStateSetup
{
    public void RegisterStates(Dictionary<(Character, HistoryPartState), IEmailStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Raquel_Day_One, HistoryPartState.Part_One), new Day_One_FirstWrite_Raquel());
    }
}
