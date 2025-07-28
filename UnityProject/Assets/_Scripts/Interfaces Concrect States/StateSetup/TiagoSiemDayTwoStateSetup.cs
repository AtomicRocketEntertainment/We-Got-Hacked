using System.Collections.Generic;

public class TiagoSiemDayTwoStateSetup : IStateSetup
{
    public void RegisterStates(Dictionary<(Character, SoftwareState), ISoftwareStateHandler> stateHandlers)
    {
        stateHandlers.Add((Character.Tiago_Day_Two, SoftwareState.Empty), new Day_Two_EmptySiem_Tiago());
    }
}
