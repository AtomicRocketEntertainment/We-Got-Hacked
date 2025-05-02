using System;
using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicketScreen : MonoBehaviour, IScreenInfoUpdater
{
    [SerializeField] private ScreenType _screenType;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _playbookDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _idDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _ipODp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _ipDDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _geolocationDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _typeDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _dateDp;

    [BoxGroup("Pichacao Toggles"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private ToggleGroup _RisktoggleGroup;
    [BoxGroup("Pichacao Toggles"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private Transform _SitetoggleGroup;

    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _blockedCanvas;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _pichacaoScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _phishingScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _ransowareScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _dataLeakScreen;

    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))] [SerializeField] private Transform _objectiveList;
    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))] [SerializeField] private Button _showCurrentInfoBtn;
    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))] [SerializeField] private GameObject _currentScreenPopUp;
    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))] [SerializeField] private GameObject _objectivePrefab;

    private readonly string CANT_OPEN_POPUP = "Ainda não tenho ticket para verificar";

    private List<GameObject> _objectivesActive = new List<GameObject>();
    private List<GameObject> _playbookScreens => new List<GameObject> 
    {
        _pichacaoScreen,
        _phishingScreen,
        _ransowareScreen,
        _dataLeakScreen
    };

    private string _playbookSelect = "";
    private string _idSelect = "";
    private string _ipOSelect = "";
    private string _ipDSelect = "";
    private string _geolocationSelect = "";
    private string _typeSelect = "";
    private string _dateSelect = "";
    private bool _canOpenPopUp = false;

    public ScreenType CurrentType => _screenType;

    void OnEnable()
    {
        _showCurrentInfoBtn?.onClick.AddListener(ShowCurrentPopUp);
        _playbookDp?.onValueChanged.AddListener(UpdatePlaybookSelected);
        _idDp?.onValueChanged.AddListener(UpdateIdSelected);
        _ipODp?.onValueChanged.AddListener(UpdateIpOSelected);
        _ipDDp?.onValueChanged.AddListener(UpdateIpDSelected);
        _geolocationDp?.onValueChanged.AddListener(UpdateLocationSelected);
        _typeDp?.onValueChanged.AddListener(UpdateDeviceSelected);
        _dateDp?.onValueChanged.AddListener(UpdateDateSelected);
    }


    void OnDisable()
    {
        _showCurrentInfoBtn?.onClick.RemoveListener(ShowCurrentPopUp);
        _playbookDp?.onValueChanged.RemoveListener(UpdatePlaybookSelected);
        _idDp?.onValueChanged.RemoveListener(UpdateIdSelected);
        _ipODp?.onValueChanged.RemoveListener(UpdateIpOSelected);
        _ipDDp?.onValueChanged.RemoveListener(UpdateIpDSelected);
        _geolocationDp?.onValueChanged.RemoveListener(UpdateLocationSelected);
        _typeDp?.onValueChanged.RemoveListener(UpdateDeviceSelected);
        _dateDp?.onValueChanged.RemoveListener(UpdateDateSelected);
    }

    private void UpdatePlaybookSelected(int value) 
    { 
        _playbookSelect = _playbookDp.options[value].text;

        foreach(GameObject screen in _playbookScreens)
            screen.SetActive(false);

        if (value > 0 && value <= _playbookScreens.Count) 
            _playbookScreens[value - 1].SetActive(true);
    }
    private void UpdateIdSelected(int value) { _idSelect = _idDp.options[value].text; }
    private void UpdateIpOSelected(int value) { _ipOSelect = _ipODp.options[value].text; }
    private void UpdateIpDSelected(int value) { _ipDSelect = _ipDDp.options[value].text; }
    private void UpdateLocationSelected(int value) { _geolocationSelect = _geolocationDp.options[value].text; }
    private void UpdateDeviceSelected(int value) { _typeSelect = _typeDp.options[value].text; }
    private void UpdateDateSelected(int value) { _dateSelect = _dateDp.options[value].text; }


    public void UpdateInfos(ScreenType typeScreen, SO_TicketList ticketList, Ticket currentTicket, SoftwareState softwareState)
    {
        switch(typeScreen)
        {
            case ScreenType.NewTicket: UpdateNewTicket(ticketList, softwareState);
            break;
            case ScreenType.CurrentTicket: UpdateCurrentTicket(currentTicket);
            break;
            case ScreenType.TicketDone: UpdateDoneTicket();
            break;
            case ScreenType.Playbook: UpdatePlaybook();
            break;
        }
    }

    private void UpdateNewTicket(SO_TicketList ticketList, SoftwareState softwareState)
    {
        _playbookDp.ClearOptions();
        _idDp.ClearOptions();
        _ipODp.ClearOptions();
        _ipDDp.ClearOptions();
        _geolocationDp.ClearOptions();
        _typeDp.ClearOptions();
        _dateDp.ClearOptions();
        
        List<string> playBookOptions = new List<string> 
        { 
            _playbookSelect,
            PlaybookType.Pichacao.ToString(),
            PlaybookType.Phishing.ToString(),
            PlaybookType.Ransomware.ToString(),
            PlaybookType.DataLeak.ToString()
        };
        
        if(softwareState != SoftwareState.FullAccess) return;
        
        List<string> idOptions = new List<string> { _idSelect };
        List<string> ipOOptions = new List<string> { _ipOSelect };
        List<string> ipDOptions = new List<string> { _ipDSelect };
        List<string> geolocationOptions = new List<string> { _geolocationSelect };
        List<string> typeOptions = new List<string> { _typeSelect };
        List<string> dateOptions = new List<string> { _dateSelect };

        foreach(SO_Ticket ticket in ticketList.Tickets)
        {
            idOptions.Add(ticket.ID);
            ipOOptions.Add(ticket.IPOrigem);
            ipDOptions.Add(ticket.IPDestiny);
            
            if(!typeOptions.Contains(ticket.Location))
                geolocationOptions.Add(ticket.Location);
            
            if(!typeOptions.Contains(ticket.Dispositive.Type.ToString()))
                typeOptions.Add(ticket.Dispositive.Type.ToString());
            
            dateOptions.Add($"{ticket.DateDay} - {ticket.DateHour}");
        }

        _playbookDp.AddOptions(playBookOptions);
        _idDp.AddOptions(idOptions);
        _ipODp.AddOptions(ipOOptions);
        _ipDDp.AddOptions(ipDOptions);
        _geolocationDp.AddOptions(geolocationOptions);
        _typeDp.AddOptions(typeOptions);
        _dateDp.AddOptions(dateOptions);
    }

    private void UpdateCurrentTicket(Ticket currentTicket)
    {
        int completedObjectives = currentTicket.GetObjectivesCompletedQuantity();
        if(completedObjectives == 0) 
        {
            _canOpenPopUp = false;
            return;
        }

        _currentScreenPopUp.TryGetComponent(out TicketInfoPopUpScreen popUp);
        _canOpenPopUp = true;
        popUp.UpdateInfo(currentTicket);

        CheckObjectivesPanel(currentTicket, completedObjectives);
    }
    private void ShowCurrentPopUp()
    {
        if(_canOpenPopUp)
            _currentScreenPopUp.SetActive(true);
        else
            EventManager.MakePlayerThink(CANT_OPEN_POPUP);
    }

    private void CheckObjectivesPanel(Ticket currentTicket, int index)
    {
        for(int i = _objectivesActive.Count - 1; i >= 0; i--) 
            Destroy(_objectivesActive[i]);

        for(int i = 0; i < index + 1; i++)
        {
            GameObject obj = SpawnObjective();
            _objectivesActive.Add(obj);
            UpdateObjective(obj, currentTicket, i);
        }
    }

    private GameObject SpawnObjective()
    {
        GameObject objective = Instantiate(_objectivePrefab, Vector3.zero, Quaternion.identity);
        objective.transform.SetParent(_objectiveList);
        objective.transform.localScale = Vector3.one;

        return objective;
    }

    private void UpdateObjective(GameObject obj, Ticket currentTicket, int index)
    {
        obj.TryGetComponent(out ImTicketObjetiveHolder newHolder);
        TicketObjectives newObjectiveToShow = currentTicket.Objectives[index];
        newHolder.SetInfos(newObjectiveToShow.IsCompleted, newObjectiveToShow.Name);
    }

    private void UpdateDoneTicket()
    {

    }

    private void UpdatePlaybook()
    {

    }

    private bool NewTicketEditorChecker()
    {
        return _screenType == ScreenType.NewTicket;
    }

    private bool CurrentTicketEditorChecker()
    {
        return _screenType == ScreenType.CurrentTicket;
    }

    public bool AllInfoAreSelected()
    {
        bool riskToggle = _RisktoggleGroup.ActiveToggles().Any();
        bool siteToggle = GetSelectedToggles().Count > 0;

        return _playbookSelect != "" &&
            _idSelect != "" &&
            _ipOSelect != "" &&
            _ipDSelect != "" &&
            _geolocationSelect != "" &&
            _typeSelect != "" &&
            _dateSelect != "" &&
            riskToggle &&
            siteToggle;
    }

    public bool CheckInfo(Ticket ticket)
    {
        string selectedId = _idDp.options[_idDp.value].text;
        string selectedIpD = _ipDDp.options[_ipDDp.value].text;
        string selectedIpO = _ipODp.options[_ipODp.value].text;
        string selectedLocation = _geolocationDp.options[_geolocationDp.value].text;
        string selectedType = _typeDp.options[_typeDp.value].text;
        string selectedDate = _dateDp.options[_dateDp.value].text;
        string selectedPlaybook = _playbookDp.options[_playbookDp.value].text;
        int selectedRisk = 0;
        List<SiteType> selectedSites = new List<SiteType>();

        Toggle selectedRiskToggle = _RisktoggleGroup.ActiveToggles().FirstOrDefault();
        
        if (selectedRiskToggle != null)
        {
            selectedRiskToggle.TryGetComponent(out ImRiskHolder riskLevel);
            selectedRisk = riskLevel.RiskLevel;
        }

        foreach (var toggle in GetSelectedToggles())
            if (toggle.TryGetComponent(out ImSiteHolder siteHolder))
                selectedSites.Add(siteHolder.Site);

        bool isCorrectSiteSelected = selectedSites.Count == 1 && selectedSites[0] == ticket.Site;

        return 
            selectedPlaybook == ticket.Playbook.ToString() &&
            selectedId == ticket.ID &&
            selectedIpO == ticket.IPOrigem &&
            selectedIpD == ticket.IPDestiny &&
            selectedLocation == ticket.Location &&
            selectedType == ticket.Dispositive.Type.ToString() &&
            selectedDate == $"{ticket.DateDay} - {ticket.DateHour}" &&
            selectedRisk == ticket.RiskLevel &&
            isCorrectSiteSelected;
    }

    public List<Toggle> GetSelectedToggles()
    {
        List<Toggle> selected = new List<Toggle>();

        foreach (Transform child in _SitetoggleGroup)
        {
            Toggle toggle = child.GetComponent<Toggle>();
            if (toggle != null && toggle.isOn)
            {
                selected.Add(toggle);
            }
        }

        return selected;
    }


}
