using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="List of Ticket Info", menuName ="Scriptable Objcts/Objects Info/List of Ticket Infos")]
public class SO_TicketList : ScriptableObject
{
    public List<SO_Ticket> Tickets;
}
