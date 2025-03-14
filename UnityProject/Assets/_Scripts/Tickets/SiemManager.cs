using System.Collections;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SiemManager : MonoBehaviour
{
    [SerializeField] private List<SO_Ticket> _tickets;

    [HorizontalLine(color: EColor.Red)]
    [BoxGroup("Header")] [SerializeField] private Button _alertBtn;
    [BoxGroup("Header")] [SerializeField] private Button _yanomamitBtn;

    [HorizontalLine(color: EColor.Green)]
    [BoxGroup("Screens")] [SerializeField] private GameObject _alertScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _yanomamiScreen;

    [HorizontalLine(color: EColor.Yellow)]
    [BoxGroup("Alert")] [SerializeField] private RectTransform _alertRect;

    [HorizontalLine(color: EColor.Black)]
    [BoxGroup("Prefabs")] [SerializeField] private GameObject _alertPrefab;




    private List<Ticket> _instanceTickets;
    private int _currentTicket = 0;

    public List<Ticket> ActiveTickets => _instanceTickets;
    void OnEnable()
    {
        _instanceTickets = new List<Ticket>();
        SpawnAlert();
    }

    [ContextMenu("Spawn Alert")]
    public void SpawnAlert()
    {
        if(_currentTicket >= _tickets.Count) return;

        Ticket ticket = new Ticket(_tickets[_currentTicket]);

        GameObject instanceTicket = Instantiate(_alertPrefab, Vector3.zero, Quaternion.identity);
        instanceTicket.transform.SetParent(_alertRect);
        instanceTicket.transform.localScale = new Vector3(1, 1, 1);
        instanceTicket.name = ticket.ID;

        if(instanceTicket.TryGetComponent(out AlertInstance instance))
        {
            instance.Init(ticket);
        }

        if(!_instanceTickets.Contains(ticket))
            _instanceTickets.Add(ticket);
        
        _currentTicket++;
    }
}

