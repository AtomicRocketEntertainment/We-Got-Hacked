using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class TicketsManager : MonoBehaviour, INeedOpenCanvas, ISoftwareContext
{
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _sendTicketBtn;
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _newTicketBtn;
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _currentTicketBtn;
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _doneTicketBtn;
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _playbookBtn;   

    [BoxGroup("Other Dependencies")] [SerializeField] private SO_TicketList _listOfTickets;


    [BoxGroup("Screens")] [SerializeField] private GameObject _blockedCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _newTicketCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _currentTicketCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _doneTicketCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _playbooksCanvas;
    
    [BoxGroup("Lore to Update State")] [SerializeField] private readonly string _emailLoreToOpen = "Lore 2";  
    [BoxGroup("Lore to Update State")] [SerializeField] private readonly string _unlockSendN1 = "Lore 4";
    [BoxGroup("Lore to Update State")] [SerializeField] private readonly string _unlockSendN2 = "Lore 9";


    [BoxGroup("Emails to send - Pichacao Lore")] [SerializeField] private SO_Ticket _correctTicketSO;
    [BoxGroup("Emails to send - Pichacao Lore")] [SerializeField] private SO_Email _firstWrongEmailToSend;
    [BoxGroup("Emails to send - Pichacao Lore")] [SerializeField] private SO_Email _ticketAdjusteEmailToSend;

    private Dictionary<(Character, SoftwareState), ISoftwareStateHandler> _stateHandlers;


    private const string _onTryCreateTicket = "Não preciso fazer isso agora.";
    private const string _onTryCreateEmptyTicket = "Preciso preencher todas as informações.";

    
    [SerializeField] private SoftwareState _currentState = SoftwareState.Blocked;
    [SerializeField] private Character _currentCharacter = Character.None;

    private bool _ticketCreatedWrongOneTime = false;
    private Ticket _correctTicket;
    private Button _lastButtonClicked;

    private Dictionary<Button, GameObject> _screens = new Dictionary<Button, GameObject>();

    private void Start()
    {

        _stateHandlers = new();

        IStateSetup setup = _currentCharacter switch
        {
            Character.Tiago_Day_One => new TiagoTicketDayOneStateSetup(),
            Character.Rafael_Day_One => new RafaelTicketDayOneStateSetup(),
            _ => null
        };

        setup?.RegisterStates(_stateHandlers);

        _correctTicket = new Ticket(_correctTicketSO);
        _lastButtonClicked = null;

        if(!_screens.ContainsKey(_newTicketBtn)) _screens.Add(_newTicketBtn, _newTicketCanvas);
        if(!_screens.ContainsKey(_currentTicketBtn)) _screens.Add(_currentTicketBtn, _currentTicketCanvas);
        if(!_screens.ContainsKey(_doneTicketBtn)) _screens.Add(_doneTicketBtn, _doneTicketCanvas);
        if(!_screens.ContainsKey(_playbookBtn)) _screens.Add(_playbookBtn, _playbooksCanvas);

        _sendTicketBtn.onClick.AddListener(TrySendTicket);
        _newTicketBtn.onClick.AddListener(() => OpenScreen(_newTicketBtn));
        _currentTicketBtn.onClick.AddListener(() => OpenScreen(_currentTicketBtn));
        _doneTicketBtn.onClick.AddListener(() => OpenScreen(_doneTicketBtn));
        _playbookBtn.onClick.AddListener(() => OpenScreen(_playbookBtn));

        EventManager.OnEventEmailHandlerIsOpen += UpdateState;
        EventManager.OnCompletedTicketObjective += UpdateTicketProgress;
        OpenScreen(_newTicketBtn);
    }

    private void UpdateTicketProgress()
    {
        _correctTicket.ObjectiveCompleted();
    }

    void OnDisable()
    {
        EventManager.OnEventEmailHandlerIsOpen -= UpdateState;
        EventManager.OnCompletedTicketObjective -= UpdateTicketProgress;
        _sendTicketBtn.onClick.RemoveListener(TrySendTicket);
        _newTicketBtn.onClick.RemoveAllListeners();
        _currentTicketBtn.onClick.RemoveAllListeners();
        _doneTicketBtn.onClick.RemoveAllListeners();
        _playbookBtn.onClick.RemoveAllListeners();
    }

    private void OpenScreen(Button button)
    {
        foreach(var screen in _screens)
        {
            if(screen.Key == button)
            {
                _lastButtonClicked = button;
                GameObject screenObj = screen.Value;
                screenObj.TryGetComponent(out TicketScreen ticketUpdater);
                ticketUpdater.UpdateInfos(ticketUpdater.CurrentType, _listOfTickets, _correctTicket, _currentState);
                screenObj.SetActive(true);
            }
            else
                screen.Value.SetActive(false);
        }
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

        if(emailIndex == _emailLoreToOpen)
            _currentState = SoftwareState.FirstTimeOpened;
        else if(emailIndex == _unlockSendN1 || emailIndex == _unlockSendN2)
        {
            _currentState = SoftwareState.FullAccess;
            _newTicketCanvas.TryGetComponent(out TicketScreen ticketUpdater);
            ticketUpdater.UpdateInfos(ScreenType.NewTicket, _listOfTickets, _correctTicket, _currentState);
        }
    }

    private void TrySendTicket()
    {
        if(_currentState != SoftwareState.FullAccess)
        {
            EventManager.MakePlayerThink(_onTryCreateTicket);
            return;
        }

        _newTicketCanvas.TryGetComponent(out TicketScreen ticket);

        if(!ticket.AllInfoAreSelected())
        {
            EventManager.MakePlayerThink(_onTryCreateEmptyTicket);
            return;
        }

        if(!ticket.CheckInfo(_correctTicket))
        {
            EventManager.CreateEspecificEmail(_firstWrongEmailToSend, shouldAdvaneHistory: false, spawnOnTime: false);
            _ticketCreatedWrongOneTime = true;
            EventManager.WrongChoice();
            return;
        }
        
        if(ticket.CheckInfo(_correctTicket) && _ticketCreatedWrongOneTime)
        {
            EventManager.CreateEspecificEmail(_ticketAdjusteEmailToSend, shouldAdvaneHistory: true, spawnOnTime: false);
            UpdateTicketProgress();
            ticket.ResetNewTicketInfos();
            EventManager.CorrectChoice();
            _currentState = SoftwareState.Opened;
            return;
        }
        
        if(ticket.CheckInfo(_correctTicket) && !_ticketCreatedWrongOneTime)
        {
            EventManager.SpawnEmail(EmailType.LORE);
            UpdateTicketProgress();
            ticket.ResetNewTicketInfos();
            EventManager.CorrectChoice();
            _currentState = SoftwareState.Opened;
            return;
        }
    }

    public void OpenCanvas()
    {
        HandleCurrentState();
        OpenScreen(_lastButtonClicked);
        _mainCanvas.SetActive(true);
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
