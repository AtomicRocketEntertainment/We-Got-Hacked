using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TicketsManager : MonoBehaviour, INeedOpenCanvas, ISoftwareContext, INotificator
{
    [BoxGroup("UI Dependencies")][SerializeField] private Button _sendTicketBtn;
    [BoxGroup("UI Dependencies")][SerializeField] private Button _completeTicket;
    [BoxGroup("UI Dependencies")][SerializeField] private Button _newTicketBtn;
    [BoxGroup("UI Dependencies")][SerializeField] private Button _currentTicketBtn;
    //[BoxGroup("UI Dependencies")] [SerializeField] private Button _doneTicketBtn;
    [BoxGroup("UI Dependencies")][SerializeField] private Button _playbookBtn;

    [BoxGroup("Other Dependencies")][SerializeField] private SO_TicketList _listOfTickets;


    [BoxGroup("Screens")][SerializeField] private GameObject _blockedCanvas;
    [BoxGroup("Screens")][SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Screens")][SerializeField] private GameObject _newTicketCanvas;
    [BoxGroup("Screens")][SerializeField] private GameObject _currentTicketCanvas;
    //[BoxGroup("Screens")] [SerializeField] private GameObject _doneTicketCanvas;
    [BoxGroup("Screens")][SerializeField] private GameObject _playbooksCanvas;

    [BoxGroup("Lore to Update State")][SerializeField] private readonly string _lore2Day1 = "Lore 2";
    [BoxGroup("Lore to Update State")][SerializeField] private readonly string _lore4Day1 = "Lore 4";
    [BoxGroup("Lore to Update State")][SerializeField] private readonly string _lore9Day1 = "Lore 9";
    [BoxGroup("Lore to Update State")][SerializeField] private readonly string _lore8Day2 = "Lore 8 Day 2";


    [BoxGroup("Emails to send - Pichacao Lore")][SerializeField] private SO_Ticket _correctTicketSO;
    [BoxGroup("Emails to send - Pichacao Lore")][SerializeField] private SO_Email _firstWrongEmailToSend;
    [BoxGroup("Emails to send - Pichacao Lore")][SerializeField] private SO_Email _ticketAdjusteEmailToSend;

    private Dictionary<(Character, SoftwareState), ISoftwareStateHandler> _stateHandlers;


    [SerializeField] private SoftwareState _currentState = SoftwareState.Blocked;
    [SerializeField] private Character _currentCharacter = Character.None;

    private bool _ticketCreatedWrongOneTime = false;
    private Ticket _correctTicket;

    private Dictionary<Button, GameObject> _screens = new Dictionary<Button, GameObject>();
    private Button _lastClickedBtn;

    private void Start()
    {
        _stateHandlers = new();
        _completeTicket.interactable = false;

        IStateSetup setup = _currentCharacter switch
        {
            Character.Tiago_Day_One => new TiagoTicketDayOneStateSetup(),
            Character.Rafael_Day_One => new RafaelTicketDayOneStateSetup(),
            _ => null
        };

        setup?.RegisterStates(_stateHandlers);

        _correctTicket = new Ticket(_correctTicketSO);

        if (!_screens.ContainsKey(_newTicketBtn)) _screens.Add(_newTicketBtn, _newTicketCanvas);
        if (!_screens.ContainsKey(_currentTicketBtn)) _screens.Add(_currentTicketBtn, _currentTicketCanvas);
        //if(!_screens.ContainsKey(_doneTicketBtn)) _screens.Add(_doneTicketBtn, _doneTicketCanvas);
        if (!_screens.ContainsKey(_playbookBtn)) _screens.Add(_playbookBtn, _playbooksCanvas);
        _lastClickedBtn = _newTicketBtn;

        _completeTicket.onClick.AddListener(EndDay);
        _sendTicketBtn.onClick.AddListener(TrySendTicket);
        _newTicketBtn.onClick.AddListener(() => OpenScreen(_newTicketBtn));
        _currentTicketBtn.onClick.AddListener(() => OpenScreen(_currentTicketBtn));
        //_doneTicketBtn.onClick.AddListener(() => OpenScreen(_doneTicketBtn));
        _playbookBtn.onClick.AddListener(() => OpenScreen(_playbookBtn));

        EventManager.OnEventEmailHandlerIsOpen += UpdateState;
        EventManager.OnCompletedTicketObjective += UpdateTicketProgress;
    }

    private void UpdateTicketProgress()
    {
        _correctTicket.ObjectiveCompleted();

        if (!_mainCanvas.activeSelf)
            NotifyBar();

        if (_correctTicket.IsCompleted)
            _completeTicket.interactable = true;
    }

    void OnDisable()
    {
        EventManager.OnEventEmailHandlerIsOpen -= UpdateState;
        EventManager.OnCompletedTicketObjective -= UpdateTicketProgress;
        _completeTicket.onClick.RemoveListener(EndDay);
        _sendTicketBtn.onClick.RemoveListener(TrySendTicket);
        _newTicketBtn.onClick.RemoveAllListeners();
        _currentTicketBtn.onClick.RemoveAllListeners();
        //_doneTicketBtn.onClick.RemoveAllListeners();
        _playbookBtn.onClick.RemoveAllListeners();
    }

    private void EndDay()
    {
        EventManager.ShowStoryBoard();
    }

    private void OpenScreen(Button button)
    {
        foreach (var screen in _screens)
        {
            if (screen.Key == button)
            {
                GameObject screenObj = screen.Value;
                screenObj.TryGetComponent(out TicketScreen ticketUpdater);
                ticketUpdater.UpdateInfos(ticketUpdater.CurrentType, _listOfTickets, _correctTicket, _currentState);
                _lastClickedBtn = button;
                EventSystem.current.SetSelectedGameObject(null);
                EventSystem.current.SetSelectedGameObject(button.gameObject);
                screenObj.SetActive(true);
            }
            else
            {
                screen.Value.SetActive(false);
            }
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

        if (emailIndex == _lore2Day1)
            _currentState = SoftwareState.FirstTimeOpened;
        else if (emailIndex == _lore4Day1 || emailIndex == _lore9Day1 || emailIndex == _lore8Day2)
        {
            _currentState = SoftwareState.FullAccess;
            _newTicketCanvas.TryGetComponent(out TicketScreen ticketUpdater);
            ticketUpdater.UpdateInfos(ScreenType.NewTicket, _listOfTickets, _correctTicket, _currentState);
        }

        if (emailIndex == "Lore 5") //not very good, we need to change this later
            NotifyBar();            
    }

    private void TrySendTicket()
    {
        if(_currentState != SoftwareState.FullAccess)
        {
            EventManager.MakePlayerThink(ThoughtKey.WrongTimeCreateTicket);
            return;
        }

        _newTicketCanvas.TryGetComponent(out TicketScreen ticket);

        if(!ticket.AllInfoAreSelected())
        {
            EventManager.MakePlayerThink(ThoughtKey.TicketWithoutInfo);
            return;
        }

        if(!ticket.CheckInfo(_correctTicket))
        {
            EventManager.CreateEspecificEmail(PointEmailKey.OneTimeWrongTicketCreated);
            _ticketCreatedWrongOneTime = true;
            EventManager.WrongChoice();
            return;
        }
        
        if(ticket.CheckInfo(_correctTicket) && _ticketCreatedWrongOneTime)
        {
            EventManager.CreateEspecificEmail(PointEmailKey.CorrectTicketAfterWrongCreated);
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
        OpenScreen(_lastClickedBtn);
        _mainCanvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        foreach (var screen in _screens)
            screen.Value.SetActive(false);

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

    public void NotifyBar()
    {
        EventManager.NotifyBar(this.gameObject);
    }
}
