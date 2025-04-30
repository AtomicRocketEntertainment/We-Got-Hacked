using System.Collections.Generic;

public interface IStateSetup
{
    void RegisterStates(Dictionary<(Character, SoftwareState), ISoftwareStateHandler> stateHandlers);
}
