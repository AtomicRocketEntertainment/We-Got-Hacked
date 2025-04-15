
public interface IScreenInfoUpdater
{
    public void UpdateInfos(ScreenType typeScreen, SiemManager siem, Ticket ticket);
}


public enum ScreenType
{
    NewTicket, CurrentTicket, TicketDone, Playbook
}