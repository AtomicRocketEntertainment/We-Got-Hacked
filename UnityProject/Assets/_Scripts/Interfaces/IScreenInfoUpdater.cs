
using UnityEngine;

public interface IScreenInfoUpdater
{
    public void UpdateInfos(ScreenType typeScreen, SO_TicketList ticketList, Ticket ticket, SoftwareState state, GameObject notifier);
}


public enum ScreenType
{
    NewTicket, CurrentTicket, TicketDone, Playbook
}