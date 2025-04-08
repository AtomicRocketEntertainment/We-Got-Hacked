using System;
using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SiemManager : MonoBehaviour, INeedOpenCanvas
{
    [SerializeField] private List<SO_Ticket> _tickets;

    [HorizontalLine(color: EColor.Red)]
    [BoxGroup("Header")] [SerializeField] private Button _alertBtn;
    [BoxGroup("Header")] [SerializeField] private Button _yanomamitBtn;

    [HorizontalLine(color: EColor.Green)]
    [BoxGroup("Screens")] [SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _blockedCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _alertScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _alertPopupScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _yanomamiScreen;

    [HorizontalLine(color: EColor.Yellow)]
    [BoxGroup("Alert")] [SerializeField] private RectTransform _alertRect;

    [HorizontalLine(color: EColor.Black)]
    [BoxGroup("Prefabs")] [SerializeField] private GameObject _alertPrefab;

    [BoxGroup("Lore to Update State"), HorizontalLine(color: EColor.White)]
    [SerializeField] private const string _emailLoreToOpen = "Lore 1"; 
    [SerializeField] private const string _emailLoreToSpawnAlerts = "Lore 4";


    private List<Ticket> _instanceTickets = new List<Ticket>();
    private int _currentTicket = 0;
    private SoftwareState _currentState = SoftwareState.Blocked;


    public List<Ticket> ActiveTickets => _instanceTickets;

    void OnEnable()
    {
        EventManager.OnAlertIsOpen += OpenAlert;
        EventManager.OnEventEmailHandlerIsOpen += UpdateState;
    }

    void OnDisable()
    {
        EventManager.OnAlertIsOpen -= OpenAlert;
        EventManager.OnEventEmailHandlerIsOpen -= UpdateState;

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
            instance.Init(ticket);

        if(!_instanceTickets.Contains(ticket))
            _instanceTickets.Add(ticket);
        
        _currentTicket++;
    }

    private void OpenAlert(Ticket alert, Color ticketColor)
    {
        _alertPopupScreen.TryGetComponent(out PopupInfoHolder holder);
        holder.UpdateInfos(alert.ID, alert.IP, alert.Date, alert.Location, alert.Dispositive.Icon, ticketColor);
        _alertPopupScreen.SetActive(true);
    }

    private void HandleCurrentState()
    {
        switch(_currentState)
        {
            case SoftwareState.Blocked:
                EventManager.FirstTimeOpenSoftware();
                break;

            case SoftwareState.FirstTimeOpened: 
                _blockedCanvas.SetActive(false);
                EventManager.SpawnEmail(EmailType.SPAM);
                EventManager.SpawnEmail(EmailType.LORE);
                EventManager.SpawnEmail(EmailType.NEWS);
                EventManager.SpawnEmail(EmailType.SPAM);
                _currentState = SoftwareState.Opened;
                break;
        }
    }

    private void UpdateState(string emailIndex)
    {
        switch(emailIndex)
        {
            case _emailLoreToOpen:
                _currentState = SoftwareState.FirstTimeOpened;
                break;
            case _emailLoreToSpawnAlerts:
                for(int i = 0; i < 5; i++)
                    SpawnAlert();
                break;
  
        }
    }

    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);
        HandleCurrentState();
    }

    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }
}

