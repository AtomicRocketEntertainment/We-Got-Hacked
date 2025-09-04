public class Day_Two_FirstTimeOpenedStateTickTask_Rafael : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        EventManager.EnablePlayerSearchMalwhere();
        context.ChangeSoftwareState(SoftwareState.Empty);
    }
}
