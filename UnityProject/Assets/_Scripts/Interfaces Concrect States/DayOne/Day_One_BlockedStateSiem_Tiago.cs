public class Day_One_BlockedStateSiem_Tiago : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        EventManager.FirstTimeOpenSoftware();
    }
}
