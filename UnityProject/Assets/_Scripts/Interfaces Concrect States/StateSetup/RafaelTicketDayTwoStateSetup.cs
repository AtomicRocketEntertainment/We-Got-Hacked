using System.Collections.Generic;

public class RafaelTicketDayTwoStateSetup : IStateSetup
{
    public void RegisterStates(Dictionary<(Character, SoftwareState), ISoftwareStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Rafael_Day_Two, SoftwareState.FirstTimeOpened), new Day_Two_FirstTimeOpenedStateTickTask_Rafael());
    }
}
