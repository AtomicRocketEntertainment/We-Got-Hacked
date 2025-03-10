using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SiemManager : MonoBehaviour
{
    [SerializeField] private List<SO_Ticket> _tickets;
    private List<Ticket> _instanceTickets;
    private int _currentTicket = 0;

    public List<Ticket> ActiveTickets => _instanceTickets;
    void OnEnable()
    {
        _instanceTickets = new List<Ticket>();

        foreach(SO_Ticket ticket in _tickets)
        {
            Ticket newTicket = new Ticket(ticket); 

            if(_instanceTickets.Count < 5)
                _instanceTickets.Add(newTicket);   
        }
    }
}

