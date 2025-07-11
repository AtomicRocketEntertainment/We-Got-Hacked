public class Day_One_FirstTimeOpenedState_Rafael : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        context.ChangeSoftwareState(SoftwareState.Empty);
    }
}
