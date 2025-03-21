
public interface IScreenInfoUpdater
{
    public void UpdateInfos(ScreenType typeScreen, SiemManager siem);
}


public enum ScreenType
{
    NewTicket, CurrentTicket, TicketDone, Playbook
}