
public interface IScreenInfoUpdater
{
    public void UpdateInfos(ScreenType typeScreen, SO_TicketList ticketList, Ticket ticket, SoftwareState state);
}


public enum ScreenType
{
    NewTicket, CurrentTicket, TicketDone, Playbook
}