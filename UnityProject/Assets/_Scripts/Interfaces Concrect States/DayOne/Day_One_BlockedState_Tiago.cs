public class Day_One_BlockedState_Tiago : ISoftwareStateHandler
{
    public void Handle(ISoftwareContext context)
    {
        EventManager.FirstTimeOpenSoftware();
    }
}

