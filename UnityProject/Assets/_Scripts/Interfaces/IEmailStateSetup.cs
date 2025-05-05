using System.Collections.Generic;

public interface IEmailStateSetup
{
    void RegisterStates(Dictionary<(Character, HistoryPartState), ISoftwareStateHandler> stateHandlers);
}

