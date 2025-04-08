using System.Collections.Generic;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

public class TicketsManager : MonoBehaviour, INeedOpenCanvas
{
    [Header("UI Dependencies")]
    [SerializeField] private Button _sendTicketBtn;
    [SerializeField] private Button _newTicketBtn;
    [SerializeField] private Button _currentTicketBtn;
    [SerializeField] private Button _doneTicketBtn;
    [SerializeField] private Button _playbookBtn;   

    [Header("Screens")]
    [SerializeField] private GameObject _blockedCanvas;
    [SerializeField] private GameObject _mainCanvas;
    [SerializeField] private GameObject _newTicketCanvas;
    [SerializeField] private GameObject _currentTicketCanvas;
    [SerializeField] private GameObject _doneTicketCanvas;
    [SerializeField] private GameObject _playbooksCanvas;
    
    [Header("Lore to Update State")]
    [SerializeField] private readonly string _emailLoreToOpen = "Lore 2";  
    [SerializeField] private readonly string _emailLoreToUnlockSend = "Lore 4";

    
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
        OpenScreen(_newTicketBtn);
    }

    void OnDisable()
    {
        EventManager.OnEventEmailHandlerIsOpen -= UpdateState;
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
                ticketUpdater.UpdateInfos(ticketUpdater.CurrentType, _siem);
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
            ticketUpdater.UpdateInfos(ScreenType.NewTicket, _siem);
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

        if(ticket.CheckInfo(_correctTicket) && !_ticketCreatedWrongOneTime)
        {
            EventManager.SpawnEmail(EmailType.LORE);
            EventManager.CorrectChoice();
            _ticketCreatedWrongOneTime = true;
            return;
        }

        if(ticket.CheckInfo(_correctTicket) && _ticketCreatedWrongOneTime)
        {
            EventManager.CreateEspecificEmail(_ticketAdjusteEmailToSend, shouldAdvaneHistory: true);
            EventManager.CorrectChoice();
            return;
        }

        if(!ticket.CheckInfo(_correctTicket))
        {
            EventManager.CreateEspecificEmail(_firstWrongEmailToSend, shouldAdvaneHistory: false);
            EventManager.WrongChoice();
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
