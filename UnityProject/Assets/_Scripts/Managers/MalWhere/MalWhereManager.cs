using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MalWhereManager : MonoBehaviour, INeedOpenCanvas, IChoiceContext
{
    [BoxGroup("Screens"), SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Screens"), SerializeField] private GameObject _mainContentCanvas;
    [BoxGroup("Screens"), SerializeField] private GameObject _ipCanvas;
    [BoxGroup("Screens"), SerializeField] private GameObject _hashCanvas;

    [BoxGroup("Main UI Elements"), SerializeField] private TMP_Dropdown _searchingElementsDp;
    [BoxGroup("Main UI Elements"), SerializeField] private Button _trySearchBtn;

    [BoxGroup("IP UI Elements"), SerializeField] private Button _backfromIpBtn;
    [BoxGroup("IP UI Elements"), SerializeField] private TextMeshProUGUI _ipText;


    [BoxGroup("Hash UI Elements"), SerializeField] private Button _backfromHashBtn;
    [BoxGroup("Hash UI Elements"), SerializeField] private TextMeshProUGUI _hashText;

    [BoxGroup("Infos Dependencies")][SerializeField] private SO_TicketList _ticketList;
    [BoxGroup("Infos Dependencies")][SerializeField] private SO_Ticket _correctTicket;

    [SerializeField, BoxGroup("State Fluxogram")] private HistoryPartState _currentChoiceState = HistoryPartState.Part_One;
    [SerializeField, BoxGroup("State Fluxogram")] private Character _currentCharacter = Character.None;

    private string _dropdownSelection;
    private MalWhereState _currentState;
    private Dictionary<(Character, HistoryPartState), IChoiceStateHandler> _choiceStateHandlers;



    private readonly string _lore11Day2 = "Lore 11 Day 2";
    private readonly string _lore15Day2 = "Lore 15 Day 2";

    private void Awake()
    {
        InitSearchingOptions();
        _dropdownSelection = "";
        _currentState = MalWhereState.CantSearch;

        _choiceStateHandlers = new();

        IChoiceStateSetup choiceSetup = _currentCharacter switch
        {
            Character.Rafael_Day_Two => new Day_Two_ChoiceStateSetupMalwhere_Rafael(),
            _ => null
        };

        choiceSetup?.RegisterStates(_choiceStateHandlers);
    }

    private void OnEnable()
    {
        EventManager.OnPlayerCanSearchMalwhere += EnableSearch;
        EventManager.OnEventEmailHandlerIsOpen += CheckState;

        _trySearchBtn.onClick.AddListener(TrySearching);
        _searchingElementsDp.onValueChanged.AddListener(UpdateDropdownSelection);
        _backfromHashBtn.onClick.AddListener(BackToMain);
        _backfromIpBtn.onClick.AddListener(BackToMain);
    }

    private void OnDisable()
    {
        EventManager.OnPlayerCanSearchMalwhere -= EnableSearch;
        EventManager.OnEventEmailHandlerIsOpen -= CheckState;

        _trySearchBtn.onClick.RemoveListener(TrySearching);
        _searchingElementsDp.onValueChanged.RemoveListener(UpdateDropdownSelection);
        _backfromHashBtn.onClick.RemoveListener(BackToMain);
        _backfromIpBtn.onClick.RemoveListener(BackToMain);

    }

    private void UpdateDropdownSelection(int value) => _dropdownSelection = _searchingElementsDp.options[value].text;

    private void TrySearching()
    {
        if (_searchingElementsDp.value == 0) return; //Pesquisar selected

        if (_currentState == MalWhereState.CantSearch)
        {
            EventManager.WrongChoice();
            EventManager.MakePlayerThink(ThoughtKey.ShouldntSearchOnMalwhere);
            return;
        }

        bool isIpSearch = _dropdownSelection.Length <= 15;
        int optionIndex = _searchingElementsDp.value - 1;
        int ticketIndex = optionIndex / 2;
        SO_Ticket selectedIdTicket = _ticketList.Tickets[ticketIndex];
        bool isCorrectTicket = selectedIdTicket.ID == _correctTicket.ID;

        _mainContentCanvas.SetActive(false);

        if (isIpSearch)
        {
            _ipCanvas.SetActive(true);
            UpdateIpCanvas(isCorrectTicket, selectedIdTicket);
            return;
        }

        _hashCanvas.SetActive(true);
        UpdateHashCanvas(isCorrectTicket, selectedIdTicket);
    }

    private void UpdateIpCanvas(bool isCorrectTicket, SO_Ticket ticket)
    {
        int randomReports = Random.Range(50, 1000);
        bool isRafaelDayTwoTime = _currentChoiceState == HistoryPartState.Part_Two;

        if (isCorrectTicket && isRafaelDayTwoTime)
        {
            _ipText.SetText($"{ticket.IPOrigem} foi encontrando no banco de dados e reportado {randomReports} vezes! Tem 100% de probabilidade de ser malicioso.");
            HandleState();
        }
        else
            _ipText.SetText("O IP informado não possui denúncias.");

    }

    private void UpdateHashCanvas(bool isCorrectTicket, SO_Ticket ticket)
    {
        if (isCorrectTicket)
            HandleState();

        _hashText.SetText($"A Hash pesquisada ({ticket.RansomwareInformation.Hash}) é responsável pelo ransomware de nome: {ticket.RansomwareInformation.RansomwareName}");
    }

    private void InitSearchingOptions()
    {
        _searchingElementsDp.ClearOptions();
        List<string> searchingOptions = new List<string>() { "Pesquisar" };

        foreach (SO_Ticket ticket in _ticketList.Tickets)
        {
            searchingOptions.Add(ticket.RansomwareInformation.Hash);
            searchingOptions.Add(ticket.IPOrigem);
        }

        _searchingElementsDp.AddOptions(searchingOptions);
    }

    private void CheckState(string emailIndex)
    {
        if (emailIndex == _lore15Day2)
            EnableSearch();

        if (emailIndex == _lore11Day2)
            DisableSearch();
    }
    private void BackToMain()
    {
        _mainContentCanvas.SetActive(true);
        _ipCanvas.SetActive(false);
        _hashCanvas.SetActive(false);
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