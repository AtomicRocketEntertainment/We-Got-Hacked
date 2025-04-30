using System.Collections.Generic;

public class RafaelTicketDayOneStateSetup : IStateSetup
{
    public void RegisterStates(Dictionary<(Character, SoftwareState), ISoftwareStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Rafael_Day_One, SoftwareState.FirstTimeOpened), new Day_One_FirstTimeOpenedState_Rafael());
    }
}
