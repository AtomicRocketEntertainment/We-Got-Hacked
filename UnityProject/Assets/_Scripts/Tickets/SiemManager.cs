using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class SiemManager : MonoBehaviour, INeedOpenCanvas, ISoftwareContext
{
    [SerializeField] private List<SO_Ticket> _tickets;

    [HorizontalLine(color: EColor.Green)]
    [BoxGroup("Screens")][SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Screens")][SerializeField] private GameObject _blockedCanvas;
    [BoxGroup("Screens")][SerializeField] private GameObject _alertScreen;
    [BoxGroup("Screens")][SerializeField] private GameObject _alertPopupScreen;
    [BoxGroup("Screens")][SerializeField] private GameObject _logPopupScreen;
    [BoxGroup("Screens")][SerializeField] private GameObject _awaintingAlertScreen;

    [HorizontalLine(color: EColor.Yellow)]
    [BoxGroup("Alert")][SerializeField] private RectTransform _alertRect;
    [BoxGroup("Alert")][SerializeField] private Button _openLogBtn;


    [HorizontalLine(color: EColor.Black)]
    [BoxGroup("Prefabs")][SerializeField] private GameObject _alertPrefab;

    [SerializeField] private SoftwareState _currentState = SoftwareState.Blocked;
    [SerializeField] private Character _currentCharacter = Character.None;


    private const string _emailLoreToOpen = "Lore 1";
    private const string _emailLoreToSpawnAlerts = "Lore 4";
    private const string _emailLoreToSpawnAlertsDayTwo = "Lore 1 Day 2";
    private const string _emailLore3Day2 = "Lore 3 Day 2";

    private Dictionary<(Character, SoftwareState), ISoftwareStateHandler> _stateHandlers;
    private List<Ticket> _instanceTickets = new List<Ticket>();
    private List<GameObject> _instanceObjTickets = new List<GameObject>();
    private int _currentTicket = 0;
    private int _ticketMaxLevel = 5;


    public List<Ticket> ActiveTickets => _instanceTickets;

    void OnEnable()
    {

        _stateHandlers = new();

        IStateSetup setup = _currentCharacter switch
        {
            Character.Tiago_Day_One => new TiagoSiemDayOneStateSetup(),
            _ => null
        };

        setup?.RegisterStates(_stateHandlers);

        _openLogBtn.onClick.AddListener(FirstTimeOpenedLog);
        EventManager.OnAlertIsOpen += OpenAlert;
        EventManager.OnEventEmailHandlerIsOpen += UpdateState;
    }

    void OnDisable()
    {
        _openLogBtn.onClick.RemoveListener(FirstTimeOpenedLog);
        EventManager.OnAlertIsOpen -= OpenAlert;
        EventManager.OnEventEmailHandlerIsOpen -= UpdateState;

    }

    [ContextMenu("Spawn Alert")]
    public void SpawnAlert()
    {
        if (_currentTicket >= _tickets.Count) return;

        if (!_mainCanvas.activeSelf)
            EventManager.NotifyBar(this.gameObject);

        _awaintingAlertScreen.SetActive(false);

        Ticket ticket = new Ticket(_tickets[_currentTicket]);

        GameObject instanceTicket = Instantiate(_alertPrefab, Vector3.zero, Quaternion.identity);
        instanceTicket.transform.SetParent(_alertRect);
        instanceTicket.transform.localScale = new Vector3(1, 1, 1);
        instanceTicket.name = ticket.ID;

        if (instanceTicket.TryGetComponent(out AlertInstance instance))
            instance.Init(ticket);

        if (!_instanceTickets.Contains(ticket))
            _instanceTickets.Add(ticket);

        if (!_instanceObjTickets.Contains(instanceTicket))
            _instanceObjTickets.Add(instanceTicket);

        _currentTicket++;
    }

    private void OpenAlert(Ticket alert, Color ticketColor)
    {
        if (alert.RiskLevel != _ticketMaxLevel)
            EventManager.MakePlayerThink(ThoughtKey.WrongAlertOpen);

        _alertPopupScreen.TryGetComponent(out PopupInfoHolder holder);
        holder.UpdateInfos(alert.ID, alert.IPOrigem, alert.IPDestiny, alert.Dispositive.ToString(), alert.Origin.ToString(), alert.DateDay, alert.DateHour, alert.Location, ticketColor);

        _logPopupScreen.TryGetComponent(out SiemLogInfoHolder logHolder);
        logHolder.UpdateLog(alert.SiemLog);
        _alertPopupScreen.SetActive(true);
    }

    private void HandleCurrentState()
    {
        if (_stateHandlers.TryGetValue((_currentCharacter, _currentState), out var handler))
        {
            handler.Handle(this);
        }
        else
        {
            Debug.LogWarning("Nenhum handler para este estado/personagem.");
        }
    }

    private void UpdateState(string emailIndex)
    {
        switch (emailIndex)
        {
            case _emailLoreToOpen:
                _currentState = SoftwareState.FirstTimeOpened;
                break;
            case _emailLoreToSpawnAlerts:
                for (int i = 0; i < 5; i++)
                    SpawnAlert();
                break;
            case _emailLoreToSpawnAlertsDayTwo:
                SpawnAlert();
                break;
            case _emailLore3Day2:
                ClearListOfAlerts();
                for (int i = 0; i < 3; i++)
                    SpawnAlert();
                break;
        }
    }

    private void FirstTimeOpenedLog()
    {
        if (_currentCharacter == Character.Tiago_Day_Two && _currentState == SoftwareState.FirstTimeOpened)
        {
            EventManager.SpawnEmail(EmailType.LORE);
            EventManager.SpawnEmail(EmailType.SPAM);
            ChangeSoftwareState(SoftwareState.FullAccess);
        }
    }

    private void ClearListOfAlerts()
    {
        _instanceTickets.Clear();

        foreach (GameObject obj in _instanceObjTickets)
            Destroy(obj);
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

    public void ChangeBlockedCanvasStatus(bool status)
    {
        _blockedCanvas.SetActive(status);
    }

    public void ChangeSoftwareState(SoftwareState state)
    {
        _currentState = state;
    }
}

public enum LogState
{

}