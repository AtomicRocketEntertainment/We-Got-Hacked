using System.Collections.Generic;

public interface IChoiceStateSetup
{
    void RegisterStates(Dictionary<(Character, HistoryPartState), IChoiceStateHandler> stateHandlers);
}

