using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class TicketsManager : MonoBehaviour, INeedOpenCanvas
{
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _sendTicketBtn;
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _newTicketBtn;
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _currentTicketBtn;
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _doneTicketBtn;
    [BoxGroup("UI Dependencies")] [SerializeField] private Button _playbookBtn;   

    [BoxGroup("Screens")] [SerializeField] private GameObject _blockedCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _newTicketCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _currentTicketCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _doneTicketCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _playbooksCanvas;
    
    [BoxGroup("Lore to Update State")] [SerializeField] private readonly string _emailLoreToOpen = "Lore 2";  
    [BoxGroup("Lore to Update State")] [SerializeField] private readonly string _emailLoreToUnlockSend = "Lore 4";

    [BoxGroup("Emails to send - Pichacao Lore")] [SerializeField] private SO_Ticket _correctTicketSO;
    [BoxGroup("Emails to send - Pichacao Lore")] [SerializeField] private SO_Email _firstWrongEmailToSend;
    [BoxGroup("Emails to send - Pichacao Lore")] [SerializeField] private SO_Email _ticketAdjusteEmailToSend;


    private const string _onTryCreateTicket = "Eu não tenho informações disponíveis.";
    private const string _onTryCreateEmptyTicket = "Preciso preencher todas as informações.";

    
    private SoftwareState _currentState = SoftwareState.Blocked;
    private bool _ticketCreatedWrongOneTime = false;
    private SiemManager _siem;
    private Ticket _correctTicket;

    private Dictionary<Button, GameObject> _screens = new Dictionary<Button, GameObject>();

    private void Start()
    {
        //problemas com o siem aqui, provavelmente é por causa disso.
        _siem = FindAnyObjectByType<SiemManager>();
        _correctTicket = new Ticket(_correctTicketSO);

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
                GameObject screenObj = screen.Value;
                screenObj.TryGetComponent(out TicketScreen ticketUpdater);
                ticketUpdater.UpdateInfos(ticketUpdater.CurrentType, _siem, _correctTicket);
                screenObj.SetActive(true);
            }
            else
                screen.Value.SetActive(false);
        }
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
                _currentState = SoftwareState.Empty;
                break;
        }
    }

    private void UpdateState(string emailIndex)
    {
        if(emailIndex == _emailLoreToOpen)
            _currentState = SoftwareState.FirstTimeOpened;
        else if(emailIndex == _emailLoreToUnlockSend)
        {
            _currentState = SoftwareState.Opened;
            _newTicketCanvas.TryGetComponent(out TicketScreen ticketUpdater);
            ticketUpdater.UpdateInfos(ScreenType.NewTicket, _siem, _correctTicket);
        }
    }

    private void TrySendTicket()
    {
        if(_currentState != SoftwareState.Opened)
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
            EventManager.CreateEspecificEmail(_firstWrongEmailToSend, shouldAdvaneHistory: false);
            _ticketCreatedWrongOneTime = true;
            EventManager.WrongChoice();
            return;
        }
        
        if(ticket.CheckInfo(_correctTicket) && _ticketCreatedWrongOneTime)
        {
            EventManager.CreateEspecificEmail(_ticketAdjusteEmailToSend, shouldAdvaneHistory: true);
            UpdateTicketProgress();
            EventManager.CorrectChoice();
            return;
        }
        
        if(ticket.CheckInfo(_correctTicket) && !_ticketCreatedWrongOneTime)
        {
            EventManager.SpawnEmail(EmailType.LORE);
            UpdateTicketProgress();
            EventManager.CorrectChoice();
            return;
        }



    }

    public void OpenCanvas()
    {
        HandleCurrentState();
        _mainCanvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }
}
