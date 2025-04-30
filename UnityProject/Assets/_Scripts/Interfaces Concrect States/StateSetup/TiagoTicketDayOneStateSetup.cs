using System.Collections.Generic;

public class TiagoTicketDayOneStateSetup : IStateSetup
{
    public void RegisterStates(Dictionary<(Character, SoftwareState), ISoftwareStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Tiago_Day_One, SoftwareState.Blocked), new Day_One_BlockedState_Tiago());
        stateHandlers.Add((Character.Tiago_Day_One, SoftwareState.FirstTimeOpened), new Day_One_FirstTimeOpenedState_Tiago());
    }
}