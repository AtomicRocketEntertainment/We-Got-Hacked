using UnityEngine;

public class Day_One_FirstTimeOpenedState_Tiago : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        context.ChangeBlockedCanvasStatus(false);
        EventManager.SpawnEmail(EmailType.LORE);
        context.ChangeSoftwareState(SoftwareState.Empty);
    }
}
