using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MalWhereManager : MonoBehaviour, INeedOpenCanvas, IChoiceContext
{
    [BoxGroup("Screens"), SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Screens"), SerializeField] private GameObject _domainCanvas;
    [BoxGroup("Screens"), SerializeField] private GameObject _hashCanvas;
    [BoxGroup("Screens"), SerializeField] private GameObject _ipCanvas;
    [BoxGroup("Screens"), SerializeField] private GameObject _header;

    [BoxGroup("Buttons"), SerializeField] private Button _domainBtn;
    [BoxGroup("Buttons"), SerializeField] private Button _hashBtn;
    [BoxGroup("Buttons"), SerializeField] private Button _ipBtn;

    [BoxGroup("Main UI Elements"), SerializeField] private Sprite _iconCorrect;
    [BoxGroup("Main UI Elements"), SerializeField] private Sprite _iconIncorrect;

    [BoxGroup("Domain UI Elements"), SerializeField] private TMP_Dropdown _searchDomainDp;
    [BoxGroup("Domain UI Elements"), SerializeField] private GameObject _domainInfoScreen;
    [BoxGroup("Domain UI Elements"), SerializeField] private GameObject _domainSearchScreen;
    [BoxGroup("Domain UI Elements"), SerializeField] private Button _confirmDomainSearchBtn;
    [BoxGroup("Domain UI Elements"), SerializeField] private Button _backfromDomainBtn;

    [BoxGroup("IP UI Elements"), SerializeField] private TMP_Dropdown _searchIpDp;
    [BoxGroup("IP UI Elements"), SerializeField] private GameObject _ipInfoScreen;
    [BoxGroup("IP UI Elements"), SerializeField] private GameObject _ipSearchScreen;
    [BoxGroup("IP UI Elements"), SerializeField] private Button _confirmIpSearchBtn;
    [BoxGroup("IP UI Elements"), SerializeField] private Button _backfromIpBtn;

    [BoxGroup("Hash UI Elements"), SerializeField] private TMP_Dropdown _searchHashDp;
    [BoxGroup("Hash UI Elements"), SerializeField] private GameObject _hashInfoScreen;
    [BoxGroup("Hash UI Elements"), SerializeField] private GameObject _hashSearchScreen;
    [BoxGroup("Hash UI Elements"), SerializeField] private Button _confirmHashSearchBtn;
    [BoxGroup("Hash UI Elements"), SerializeField] private Button _backfromHashBtn;




    [BoxGroup("Infos Dependencies")][SerializeField] private SO_TicketList _ticketList;
    [BoxGroup("Infos Dependencies")][SerializeField] private SO_Ticket _correctTicket;

    [SerializeField, BoxGroup("State Fluxogram")] private HistoryPartState _currentChoiceState = HistoryPartState.Part_One;
    [SerializeField, BoxGroup("State Fluxogram")] private Character _currentCharacter = Character.None;

    //private string _dropdownSelection;
    private MalWhereState _currentState;
    private Dictionary<(Character, HistoryPartState), IChoiceStateHandler> _choiceStateHandlers;
    private Dictionary<Button, (GameObject, GameObject, GameObject)> _screens;
    private GameObject _currentScreen;
    private GameObject _currentSearchScreen;



    private readonly string _lore11Day2 = "Lore 11 Day 2";
    private readonly string _lore15Day2 = "Lore 15 Day 2";
    private readonly string _lore9Day3 = "Lore 9 Day 3";
    private readonly string _lore13Day3 = "Lore 13 Day 3";
    private readonly string _lore15Day3 = "Lore 15 Day 3";
    

    private void Awake()
    {
        InitSearchingOptions();
        _currentState = MalWhereState.CantSearch;
        _currentScreen = _domainCanvas;
        _currentSearchScreen = _domainSearchScreen;
        _choiceStateHandlers = new();

        IChoiceStateSetup choiceSetup = _currentCharacter switch
        {
            Character.Rafael_Day_Two => new Day_Two_ChoiceStateSetupMalwhere_Rafael(),
            Character.Eduardo_Day_Three => new Day_Three_ChoiceStateSetupMalwhere_Eduardo(),
            _ => null
        };

        _screens = new Dictionary<Button, (GameObject, GameObject, GameObject)>
        {
            { _domainBtn, (_domainCanvas, _domainSearchScreen, _domainInfoScreen)},
            { _ipBtn, (_ipCanvas, _ipSearchScreen, _ipInfoScreen)},
            { _hashBtn, (_hashCanvas, _hashSearchScreen, _hashInfoScreen) },
        };

        choiceSetup?.RegisterStates(_choiceStateHandlers);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerCanSearchMalwhere += EnableSearch;
        EventManager.OnPlayerCantSearchMalwhere += DisableSearch;
        EventManager.OnEventEmailHandlerIsOpen += CheckState;

        _confirmDomainSearchBtn.onClick.AddListener(TrySearchDomain);
        _confirmHashSearchBtn.onClick.AddListener(TrySearchHash);
        _confirmIpSearchBtn.onClick.AddListener(TrySearchIp);
        _domainBtn.onClick.AddListener(() => OpenScreen(_domainBtn));
        _hashBtn.onClick.AddListener(() => OpenScreen(_hashBtn));
        _ipBtn.onClick.AddListener(() => OpenScreen(_ipBtn));
        _backfromHashBtn.onClick.AddListener(BackToMain);
        _backfromIpBtn.onClick.AddListener(BackToMain);
        _backfromDomainBtn.onClick.AddListener(BackToMain);
    }

    private void OnDisable()
    {
        EventManager.OnPlayerCanSearchMalwhere -= EnableSearch;
        EventManager.OnPlayerCantSearchMalwhere -= DisableSearch;
        EventManager.OnEventEmailHandlerIsOpen -= CheckState;

        _confirmDomainSearchBtn.onClick.RemoveListener(TrySearchDomain);
        _confirmHashSearchBtn.onClick.RemoveListener(TrySearchHash);
        _confirmIpSearchBtn.onClick.RemoveListener(TrySearchIp);
        _domainBtn.onClick.RemoveAllListeners();
        _hashBtn.onClick.RemoveAllListeners();
        _ipBtn.onClick.RemoveAllListeners();
        _backfromHashBtn.onClick.RemoveListener(BackToMain);
        _backfromIpBtn.onClick.RemoveListener(BackToMain);
        _backfromDomainBtn.onClick.RemoveListener(BackToMain);
    }

    private void TrySearchDomain()
    {
        if (_searchDomainDp.value == 0) return; //Pesquisar selected

        if (!CanSearchWithNotify()) return;

        int optionIndex = _searchDomainDp.value - 1;
        SO_Ticket selectedIdTicket = _ticketList.Tickets[optionIndex];
        bool isCorrectTicket = selectedIdTicket.ID == _correctTicket.ID;

        _domainInfoScreen.SetActive(true);
        _domainSearchScreen.SetActive(false);
        _header.SetActive(false);
        UpdateDomainCanvas(isCorrectTicket, selectedIdTicket);
    }

    private void TrySearchHash()
    {
        if (_searchHashDp.value == 0) return; //Pesquisar selected

        if (!CanSearchWithNotify()) return;

        int optionIndex = _searchHashDp.value - 1;
        SO_Ticket selectedIdTicket = _ticketList.Tickets[optionIndex];
        bool isCorrectTicket = selectedIdTicket.ID == _correctTicket.ID;

        _hashInfoScreen.SetActive(true);
        _hashSearchScreen.SetActive(false);
        _header.SetActive(false);
        UpdateHashCanvas(isCorrectTicket, selectedIdTicket);
    }

    private void TrySearchIp()
    {
        if (_searchIpDp.value == 0) return; //Pesquisar selected

        if (!CanSearchWithNotify()) return;

        int optionIndex = _searchIpDp.value - 1;
        SO_Ticket selectedIdTicket = _ticketList.Tickets[optionIndex];
        bool isCorrectTicket = selectedIdTicket.ID == _correctTicket.ID;

        _ipInfoScreen.SetActive(true);
        _ipSearchScreen.SetActive(false);
        _header.SetActive(false);
        UpdateIpCanvas(isCorrectTicket, selectedIdTicket);
    }

    private void UpdateIpCanvas(bool isCorrectTicket, SO_Ticket ticket)
    {
        /*
            Verificar possibilidade de adicionarmos eventos de gameplay nesses casos. Malwhere é responsável por alertar que verificamos o IP correto. O estado (Day_Two_SecondMalwhereChoice_Rafael ou outro)
            fica responsável por agir - ou trocar para o próximo estado - quando isso acontece.
        */
        bool isRafaelDayTwoTime = _currentChoiceState == HistoryPartState.Part_Two && _currentCharacter == Character.Rafael_Day_Two;
        bool isEduardoDayThreeTime = _currentChoiceState == HistoryPartState.Part_One && _currentCharacter == Character.Eduardo_Day_Three;

        if (isCorrectTicket && (isRafaelDayTwoTime || isEduardoDayThreeTime))
            HandleState();

        _ipInfoScreen.TryGetComponent(out MalWhereIPUpdater updater);
        updater?.UpdateIpInfos(ticket.IPOrigem, ticket.RansomwareInformation, isCorrectTicket ? _iconIncorrect : _iconCorrect);
    }

    private void UpdateDomainCanvas(bool isCorrectTicket, SO_Ticket ticket)
    {
        bool isEduardoDayThreeTime = (_currentChoiceState ==  HistoryPartState.Part_Two || _currentChoiceState == HistoryPartState.Part_Three) && _currentCharacter == Character.Eduardo_Day_Three;

        if (isCorrectTicket && isEduardoDayThreeTime)
            HandleState();

        _domainInfoScreen.TryGetComponent(out MalWhereDomainUpdater updater);
        updater?.UpdateDomainInfos(ticket.DataLeakInformation.DomainInfo);
    }

    private void UpdateHashCanvas(bool isCorrectTicket, SO_Ticket ticket)
    {
        bool isRafaelDayTwoTime = _currentChoiceState == HistoryPartState.Part_One && _currentCharacter == Character.Rafael_Day_Two;

        if (isCorrectTicket && isRafaelDayTwoTime)
            HandleState();

        _hashInfoScreen.TryGetComponent(out MalWhereHashUpdater updater);
        updater?.UpdateHashInfos(ticket.RansomwareInformation, isCorrectTicket ? _iconIncorrect : _iconCorrect);
    }

    private void InitSearchingOptions()
    {
        _searchDomainDp.ClearOptions();
        _searchHashDp.ClearOptions();
        _searchIpDp.ClearOptions();

        List<string> domainOptions = new List<string>() { "hash, domínio, endereço IP, DNS ou URL" };
        List<string> ipOptions = new List<string>() { "hash, domínio, endereço IP, DNS ou URL" };
        List<string> hashOptions = new List<string>() { "hash, domínio, endereço IP, DNS ou URL" };


        foreach (SO_Ticket ticket in _ticketList.Tickets)
        {
            hashOptions.Add(ticket.RansomwareInformation.Hash);
            ipOptions.Add(ticket.IPOrigem);
            domainOptions.Add(ticket.DataLeakInformation.DomainInfo.Name);
        }

        _searchDomainDp.AddOptions(domainOptions);
        _searchHashDp.AddOptions(hashOptions);
        _searchIpDp.AddOptions(ipOptions);
    }

    private bool CanSearchWithNotify()
    {
        bool canSearch = _currentState == MalWhereState.CanSearch;

        if (!canSearch)
        {
            EventManager.WrongChoice();
            EventManager.MakePlayerThink(ThoughtKey.ShouldntSearchOnMalwhere);
        }

        return canSearch;
    }

    private void CheckState(string emailIndex)
    {
        if (emailIndex == _lore15Day2 || emailIndex == _lore9Day3 || emailIndex == _lore13Day3 || emailIndex == _lore15Day3)
            EnableSearch();

        if (emailIndex == _lore11Day2)
            DisableSearch();
    }
    
    private void BackToMain()
    {
        DesactiveAllScreens();
        _header.SetActive(true);
        _currentScreen.SetActive(true);
        _currentSearchScreen.SetActive(true);
    }

    private void OpenScreen(Button btn)
    {
        DesactiveAllScreens();
        _screens.TryGetValue(btn, out var tuple);
        _currentScreen = tuple.Item1;
        _currentSearchScreen = tuple.Item2;
        _currentScreen.SetActive(true);
        _currentSearchScreen.SetActive(true);
    }

    private void DesactiveAllScreens()
    {
        foreach (var screen in _screens.Values)
        {
            screen.Item1.SetActive(false);
            screen.Item2.SetActive(false);
            screen.Item3.SetActive(false);
        }
    }

    public void CloseCanvas() => _mainCanvas.SetActive(false);
    public void OpenCanvas() => _mainCanvas.SetActive(true);
    private void EnableSearch() => _currentState = MalWhereState.CanSearch;
    private void DisableSearch() => _currentState = MalWhereState.CantSearch;

    public void ChangeChoiceState(HistoryPartState state)
    {
        _currentChoiceState = state;
    }

    private void HandleState()
    {
        if (_choiceStateHandlers.TryGetValue((_currentCharacter, _currentChoiceState), out var handler))
        {
            handler.Handle(this);
        }
        else
        {
            Debug.LogWarning("Nenhum handler para este estado/personagem.");
        }
    }
}


public enum MalWhereState
{
    CanSearch, CantSearch
}