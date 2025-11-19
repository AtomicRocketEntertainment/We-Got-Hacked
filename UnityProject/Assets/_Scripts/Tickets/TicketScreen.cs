using System.Collections.Generic;
using System.Linq;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TicketScreen : MonoBehaviour, IScreenInfoUpdater
{
    [SerializeField] private ScreenType _screenType;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _playbookDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _idDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _ipODp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _ipDDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _geolocationDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _deviceType;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _originType;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _dateDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private ToggleGroup _RisktoggleGroup;
    
    [BoxGroup("Pichacao Toggles"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private Transform _SitetoggleGroup;

    [BoxGroup("Ransomware Infos"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _criptoWallet;
    [BoxGroup("Ransomware Infos"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _hashDp;

    [BoxGroup("Data Leak Infos"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _clientDataleakDp;
    [BoxGroup("Data Leak Infos"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _dateDataleakDp;

    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private GameObject _blockedCanvas;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private GameObject _emptyPlaybookScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private GameObject _pichacaoScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private GameObject _phishingScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private GameObject _ransowareScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private GameObject _dataLeakScreen;

    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))][SerializeField] private Transform _objectiveList;
    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))][SerializeField] private Button _showCurrentInfoBtn;
    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))][SerializeField] private GameObject _currentScreenPopUp;
    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))][SerializeField] private GameObject _mainCurrentObjectiveScreen;
    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))][SerializeField] private GameObject _noCurrentTicketScreen;
    [BoxGroup("Current Ticket Componentes"), ShowIf(nameof(CurrentTicketEditorChecker))][SerializeField] private GameObject _objectivePrefab;


    private List<GameObject> _objectivesActive = new List<GameObject>();
    private List<GameObject> _playbookScreens => new List<GameObject>
    {
        _emptyPlaybookScreen,
        _pichacaoScreen,
        _phishingScreen,
        _ransowareScreen,
        _dataLeakScreen
    };

    private readonly int _maxObjectiveWithoutScale = 4;
    private int _lastExtraObjective = -1;
    private readonly float _scaleGapForObjective = -25f;

    private string _playbookSelect = "";
    private string _idSelect = "";
    private string _ipOSelect = "";
    private string _ipDSelect = "";
    private string _geolocationSelect = "";
    private string _deviceSelect = "";
    private string _originSelect = "";
    private string _dateSelect = "";
    private string _walletSelect = "";
    private string _hashSelect = "";
    private string _dataLeakClientSelect = "";
    private string _dataLeakDateSelect = "";
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
        _deviceType?.onValueChanged.AddListener(UpdateDeviceSelected);
        _originType?.onValueChanged.AddListener(UpdateOriginSelected);
        _dateDp?.onValueChanged.AddListener(UpdateDateSelected);
        _criptoWallet?.onValueChanged.AddListener(UpdateWalletSelected);
        _hashDp?.onValueChanged.AddListener(UpdateHashSelected);
        _clientDataleakDp?.onValueChanged.AddListener(UpdateDataLeakClientSelected);
        _dateDataleakDp?.onValueChanged.AddListener(UpdateDataLeakDateSelected);
    }


    void OnDisable()
    {
        _showCurrentInfoBtn?.onClick.RemoveListener(ShowCurrentPopUp);
        _playbookDp?.onValueChanged.RemoveListener(UpdatePlaybookSelected);
        _idDp?.onValueChanged.RemoveListener(UpdateIdSelected);
        _ipODp?.onValueChanged.RemoveListener(UpdateIpOSelected);
        _ipDDp?.onValueChanged.RemoveListener(UpdateIpDSelected);
        _geolocationDp?.onValueChanged.RemoveListener(UpdateLocationSelected);
        _deviceType?.onValueChanged.RemoveListener(UpdateDeviceSelected);
        _originType?.onValueChanged.RemoveListener(UpdateOriginSelected);
        _dateDp?.onValueChanged.RemoveListener(UpdateDateSelected);
        _criptoWallet?.onValueChanged.RemoveListener(UpdateWalletSelected);
        _hashDp?.onValueChanged.RemoveListener(UpdateHashSelected);

    }

    private void UpdatePlaybookSelected(int value)
    {
        _playbookSelect = _playbookDp.options[value].text;

        if (_playbookSelect != PlaybookType.VazamentoDeDados.ToString())
            EnableAllDps();
        else
            DisableDps();

        foreach (GameObject screen in _playbookScreens)
                screen.SetActive(false);

        if (value >= 0 && value <= _playbookScreens.Count)
            _playbookScreens[value].SetActive(true);
    }

    private void DisableDps()
    {
        if(_idDp != null)
        {
            _idDp.interactable = false;
            _idDp.value = 0;   
        }

        if(_ipODp != null)
        {
            _ipODp.interactable = false;
            _ipODp.value = 0;   
        }

        if(_geolocationDp != null)
        {
            _geolocationDp.interactable = false;
            _geolocationDp.value = 0;   
        }

        if(_dateDp != null)
        {
            _dateDp.interactable = false;
            _dateDp.value = 0;   
        }

        if(_originType != null)
        {
            _originType.interactable = false;
            _originType.value = 0;   
        }
    }

    private void EnableAllDps()
    {
        if(_idDp != null)
            _idDp.interactable = true;

        if(_ipODp != null)
            _ipODp.interactable = true;

        if(_geolocationDp != null)
            _geolocationDp.interactable = true;

        if(_dateDp != null)
            _dateDp.interactable = true;

        if(_originType != null)
            _originType.interactable = true;
    }

    private void UpdateIdSelected(int value) { _idSelect = _idDp.options[value].text; }
    private void UpdateIpOSelected(int value) { _ipOSelect = _ipODp.options[value].text; }
    private void UpdateIpDSelected(int value) { _ipDSelect = _ipDDp.options[value].text; }
    private void UpdateLocationSelected(int value) { _geolocationSelect = _geolocationDp.options[value].text; }
    private void UpdateDeviceSelected(int value) { _deviceSelect = _deviceType.options[value].text; }
    private void UpdateOriginSelected(int value) { _originSelect = _originType.options[value].text; }
    private void UpdateDateSelected(int value) { _dateSelect = _dateDp.options[value].text; }
    private void UpdateWalletSelected(int value) { _walletSelect = _criptoWallet.options[value].text; }
    private void UpdateHashSelected(int value) { _hashSelect = _hashDp.options[value].text; }
    private void UpdateDataLeakDateSelected(int value) { _dataLeakDateSelect = _dateDataleakDp.options[value].text; }
    private void UpdateDataLeakClientSelected(int value) { _dataLeakClientSelect = _clientDataleakDp.options[value].text; }




    public void UpdateInfos(ScreenType typeScreen, SO_TicketList ticketList, Ticket currentTicket, SoftwareState softwareState, GameObject _notifier)
    {
        switch (typeScreen)
        {
            case ScreenType.NewTicket:
                UpdateNewTicket(ticketList, softwareState);
                break;
            case ScreenType.CurrentTicket:
                _notifier.SetActive(false);
                UpdateCurrentTicket(currentTicket);
                break;
            case ScreenType.TicketDone:
                UpdateDoneTicket();
                break;
            case ScreenType.Playbook:
                UpdatePlaybook();
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
        _deviceType.ClearOptions();
        _originType.ClearOptions();
        _dateDp.ClearOptions();
        _criptoWallet.ClearOptions();
        _hashDp.ClearOptions();
        _dateDataleakDp.ClearOptions();
        _clientDataleakDp.ClearOptions();

        if (softwareState != SoftwareState.FullAccess) return;

        List<string> playBookOptions = new List<string>
        {
            _playbookSelect,
            nameof(PlaybookType.Pichacao),
            nameof(PlaybookType.Phishing),
            nameof(PlaybookType.Ransomware),
            nameof(PlaybookType.VazamentoDeDados)
        };


        List<string> idOptions = new List<string> { _idSelect };
        List<string> ipOOptions = new List<string> { _ipOSelect };
        List<string> ipDOptions = new List<string> { _ipDSelect };
        List<string> geolocationOptions = new List<string> { _geolocationSelect };
        List<string> deviceOptions = new List<string> { _deviceSelect };
        List<string> originOptions = new List<string> { _originSelect };
        List<string> dateOptions = new List<string> { _dateSelect };
        List<string> criptWalletOptions = new List<string> { _walletSelect };
        List<string> hashOptions = new List<string> { _hashSelect };
        List<string> clientOptions = new List<string> { _dataLeakClientSelect };
        List<string> dateDataLeakOptions = new List<string> { _dataLeakDateSelect };



        foreach (SO_Ticket ticket in ticketList.Tickets)
        {
            if (!idOptions.Contains(ticket.Location))
                idOptions.Add(ticket.ID);

            if (!ipOOptions.Contains(ticket.Location))
                ipOOptions.Add(ticket.IPOrigem);

            if (!ipDOptions.Contains(ticket.Location))
                ipDOptions.Add(ticket.IPDestiny);
            
            if (!geolocationOptions.Contains(ticket.Location))
                geolocationOptions.Add(ticket.Location);

            if (!deviceOptions.Contains(ticket.DeviceAttacked.ToString()))
                deviceOptions.Add(ticket.DeviceAttacked.ToString());

            if (!originOptions.Contains(ticket.Origin.ToString()))
                originOptions.Add(ticket.Origin.ToString());

            if (!criptWalletOptions.Contains(ticket.RansomwareInformation.CriptoWallet))
                criptWalletOptions.Add(ticket.RansomwareInformation.CriptoWallet);

            if (!hashOptions.Contains(ticket.RansomwareInformation.Hash))
                hashOptions.Add(ticket.RansomwareInformation.Hash);

            if (!clientOptions.Contains(ticket.DataLeakInformation.CompanyName))
                clientOptions.Add(ticket.DataLeakInformation.CompanyName);

            if (!dateDataLeakOptions.Contains(ticket.DataLeakInformation.DateLeaked))
                dateDataLeakOptions.Add(ticket.DataLeakInformation.DateLeaked);

            dateOptions.Add($"{ticket.DateDay} - {ticket.DateHour}");
        }

        IListExtensions.Shuffle(idOptions);
        IListExtensions.Shuffle(ipOOptions);
        IListExtensions.Shuffle(ipDOptions);
        IListExtensions.Shuffle(geolocationOptions);
        IListExtensions.Shuffle(deviceOptions);
        IListExtensions.Shuffle(originOptions);
        IListExtensions.Shuffle(dateOptions);
        IListExtensions.Shuffle(clientOptions);
        IListExtensions.Shuffle(dateDataLeakOptions);

        _playbookDp.AddOptions(playBookOptions);
        _idDp.AddOptions(idOptions);
        _ipODp.AddOptions(ipOOptions);
        _ipDDp.AddOptions(ipDOptions);
        _geolocationDp.AddOptions(geolocationOptions);
        _deviceType.AddOptions(deviceOptions);
        _originType.AddOptions(originOptions);
        _dateDp.AddOptions(dateOptions);
        _criptoWallet.AddOptions(criptWalletOptions);
        _hashDp.AddOptions(hashOptions);
        _clientDataleakDp.AddOptions(clientOptions);
        _dateDataleakDp.AddOptions(dateDataLeakOptions);
    }

    private void UpdateCurrentTicket(Ticket currentTicket)
    {
        int completedObjectives = currentTicket.GetObjectivesCompletedQuantity();
        bool shouldShowObjectives = completedObjectives > 0;
        CheckCurrentTicketStatus(shouldShowObjectives);

        if (completedObjectives == 0)
        {
            _canOpenPopUp = false;
            return;
        }

        _currentScreenPopUp.TryGetComponent(out TicketInfoPopUpScreen popUp);
        _canOpenPopUp = true;
        popUp.UpdateInfo(currentTicket);

        CheckObjectivesPanel(currentTicket, completedObjectives);
    }

    private void CheckCurrentTicketStatus(bool hasCurrentTicket)
    {
        _mainCurrentObjectiveScreen.SetActive(hasCurrentTicket);
        _noCurrentTicketScreen.SetActive(!hasCurrentTicket);
    }

    private void ShowCurrentPopUp()
    {
        if (_canOpenPopUp)
            _currentScreenPopUp.SetActive(true);
        else
            EventManager.MakePlayerThink(ThoughtKey.WrongTimeOpenTicket);
    }

    private void CheckObjectivesPanel(Ticket currentTicket, int completedObjectives)
    {
        int needToShowNext = currentTicket.IsCompleted ? 0 : 1; //if ticket is completed, we dont add the next. We going to get miss reference otherwise

        for (int i = _objectivesActive.Count - 1; i >= 0; i--)
            Destroy(_objectivesActive[i]);

        for (int i = 0; i < completedObjectives + needToShowNext; i++)
        {
            GameObject obj = SpawnObjective();
            _objectivesActive.Add(obj);
            UpdateObjective(obj, currentTicket, i);
        }

        ScaleObjectiveScreen(completedObjectives + needToShowNext);
    }

    private void ScaleObjectiveScreen(int objectiveQuantity)
    {
        int extraObjective = objectiveQuantity - _maxObjectiveWithoutScale;
        if (extraObjective <= 0) return;
        if (_lastExtraObjective == extraObjective) return; //Fix the problem that scaling forever

        _lastExtraObjective = extraObjective;

        _mainCurrentObjectiveScreen.TryGetComponent(out RectTransform rect);
        float newBottom = rect.offsetMin.y + (extraObjective * _scaleGapForObjective);
        rect.offsetMin = new Vector2(rect.offsetMin.x, newBottom);
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
        return CheckSelectedInfos();
    }

    public bool CheckInfo(Ticket ticket)
    {
        Debug.Log($"[CheckInfo] Entrou na função. Ticket: ID={ticket.ID}, Playbook={ticket.Playbook}, IPOrigem={ticket.IPOrigem}, IPDestiny={ticket.IPDestiny}");

        Debug.Log($"[CheckInfo] Comparando Playbook selecionado '{_playbookSelect}' com Ticket.Playbook '{ticket.Playbook}'");
        if (ticket.Playbook.ToString() != _playbookSelect) 
        {
            Debug.Log("[CheckInfo] Falhou: Playbook selecionado é diferente do ticket, retornando FALSE.");
            return false;
        }

        bool isCommonInfoCorrect = VerifyCommonInfos(ticket);
        Debug.Log($"[CheckInfo] Resultado de VerifyCommonInfos: {isCommonInfoCorrect}");

        bool isPlaybookCorrect = VerifyPlaybookInfos(_playbookSelect, ticket);
        Debug.Log($"[CheckInfo] Resultado de VerifyPlaybookInfos: {isPlaybookCorrect}");

        return isCommonInfoCorrect && isPlaybookCorrect;
    }

    private bool CheckSelectedInfos()
    {
        Debug.Log("[CheckSelectedInfos] Entrou na função.");
        Debug.Log($"[CheckSelectedInfos] _playbookDp.value: {_playbookDp.value}");
        if (_playbookSelect == "")
        {
            Debug.Log("[CheckSelectedInfos] Playbook não selecionado, retornando FALSE.");
            return false;
        }

        bool isPlaybookInfoSelected = PlaybookInfosAreSelected(_playbookSelect);
        Debug.Log($"[CheckSelectedInfos] PlaybookInfosAreSelected: {isPlaybookInfoSelected}");

        bool allSelected;

        if (_playbookSelect != PlaybookType.VazamentoDeDados.ToString())
        {
            allSelected = isPlaybookInfoSelected &&
                 _idSelect != "" &&
                 _ipOSelect != "" &&
                 _ipDSelect != "" &&
                 _geolocationSelect != "" &&
                 _deviceSelect != "" &&
                 _originSelect != "" &&
                 _dateSelect != "";
        }
        else
        {
            allSelected = isPlaybookInfoSelected &&
                 _ipDSelect != "" &&
                 _deviceSelect != "" &&
                 _dataLeakClientSelect != "" &&
                 _dataLeakDateSelect != "";
        }

        Debug.Log($"[CheckSelectedInfos] Resultado final: {allSelected}");
        return allSelected;
    }

    private bool PlaybookInfosAreSelected(string playbook)
    {
        Debug.Log($"[PlaybookInfosAreSelected] Entrou na função. Playbook: {playbook}");
        bool areSelected = false;

        switch (playbook)
        {
            case nameof(PlaybookType.Pichacao):
                int pichacaoCount = GetPichacaoSiteSelected().Count;
                Debug.Log($"[PlaybookInfosAreSelected] Pichacao sites selecionados: {pichacaoCount}");
                areSelected = pichacaoCount > 0;
                break;
            case nameof(PlaybookType.Ransomware):
                Debug.Log($"[PlaybookInfosAreSelected] Wallet selecionado: {_walletSelect}, Hash selecionado: {_hashSelect}");
                areSelected = !string.IsNullOrEmpty(_hashSelect) && !string.IsNullOrEmpty(_walletSelect);
                break;
            case nameof(PlaybookType.VazamentoDeDados):
                areSelected = !string.IsNullOrEmpty(_dataLeakClientSelect) && !string.IsNullOrEmpty(_dataLeakDateSelect);
                break;
            default:
                Debug.Log("[PlaybookInfosAreSelected] Nenhuma verificação especial para esse playbook.");
                break;
        }

        Debug.Log($"[PlaybookInfosAreSelected] Resultado final: {areSelected}");
        return areSelected;
    }

    private bool VerifyPlaybookInfos(string playbook, Ticket ticket)
    {
        Debug.Log($"[VerifyPlaybookInfos] Entrou na função. Playbook: {playbook}, Ticket: ID={ticket.ID}");
        bool isCorrect = false;

        switch (playbook)
        {
            case nameof(PlaybookType.Pichacao):
                Debug.Log($"[VerifyPlaybookInfos] Verificando Pichacao. Ticket.Site={ticket.Site}");
                isCorrect = VerifyPichacao(ticket.Site);
                break;
            case nameof(PlaybookType.Ransomware):
                Debug.Log($"[VerifyPlaybookInfos] Verificando Ransomware. Ticket.Ransomware={ticket.RansomwareInfos.RansomwareName}, Wallet={ticket.RansomwareInfos.CriptoWallet}");
                isCorrect = VerifyRansomware(ticket.RansomwareInfos.CriptoWallet, ticket.RansomwareInfos.Hash);
                break;
            case nameof(PlaybookType.VazamentoDeDados):
                isCorrect = VerifyDataleak(ticket.DataLeakInfos.CompanyName, ticket.DataLeakInfos.DateLeaked);
                break;
            default:
                Debug.Log("[VerifyPlaybookInfos] Playbook sem verificações específicas.");
                break;
        }

        Debug.Log($"[VerifyPlaybookInfos] Resultado final: {isCorrect}");
        return isCorrect;
    }

    private bool VerifyCommonInfos(Ticket ticket)
    {
        Debug.Log($"[VerifyCommonInfos] Entrou na função. Ticket ID={ticket.ID}");

        string selectedId = _idDp.options[_idDp.value].text;
        string selectedIpD = _ipDDp.options[_ipDDp.value].text;
        string selectedIpO = _ipODp.options[_ipODp.value].text;
        string selectedLocation = _geolocationDp.options[_geolocationDp.value].text;
        string selectedDevice = _deviceType.options[_deviceType.value].text;
        string selectedOrigin = _originType.options[_originType.value].text;
        string selectedDate = _dateDp.options[_dateDp.value].text;

        Toggle selectedRiskToggle = _RisktoggleGroup.ActiveToggles().FirstOrDefault();
        int selectedRisk = 0;
        if (selectedRiskToggle != null && selectedRiskToggle.TryGetComponent(out ImRiskHolder riskLevel))
            selectedRisk = riskLevel.RiskLevel;

        Debug.Log($"[VerifyCommonInfos] Comparando cada campo com o ticket:");
        Debug.Log($"ID: {selectedId} == {ticket.ID}");
        Debug.Log($"IPOrigem: {selectedIpO} == {ticket.IPOrigem}");
        Debug.Log($"IPDestiny: {selectedIpD} == {ticket.IPDestiny}");
        Debug.Log($"Location: {selectedLocation} == {ticket.Location}");
        Debug.Log($"Device: {selectedDevice} == {ticket.DeviceAttacked}");
        Debug.Log($"Origin: {selectedOrigin} == {ticket.Origin}");
        Debug.Log($"Date: {selectedDate} == {ticket.DateDay} - {ticket.DateHour}");
        Debug.Log($"Risk: {selectedRisk} == {ticket.RiskLevel}");

        bool isCorrect;

        if (_playbookSelect != PlaybookType.VazamentoDeDados.ToString())
        {
            isCorrect = selectedId == ticket.ID &&
            selectedIpO == ticket.IPOrigem &&
            selectedIpD == ticket.IPDestiny &&
            selectedLocation == ticket.Location &&
            selectedDevice == ticket.DeviceAttacked.ToString() &&
            selectedOrigin == ticket.Origin.ToString() &&
            selectedDate == $"{ticket.DateDay} - {ticket.DateHour}" &&
            selectedRisk == ticket.RiskLevel;
        }
        else
        {
            isCorrect = selectedIpD == ticket.IPDestiny &&
            selectedDevice == ticket.DeviceAttacked.ToString();
        }


        Debug.Log($"[VerifyCommonInfos] Resultado final: {isCorrect}");
        return isCorrect;
    }

    private bool VerifyPichacao(SiteType site)
    {
        Debug.Log($"[VerifyPichacao] Entrou na função. Site esperado: {site}");

        List<SiteType> selectedSites = new List<SiteType>();

        foreach (var toggle in GetPichacaoSiteSelected())
        {
            if (toggle.TryGetComponent(out ImSiteHolder siteHolder))
            {
                selectedSites.Add(siteHolder.Site);
                Debug.Log($"[VerifyPichacao] Site selecionado: {siteHolder.Site}");
            }
        }

        Debug.Log($"[VerifyPichacao] Total de sites selecionados: {selectedSites.Count}");
        bool isCorrectSiteSelected = selectedSites.Count == 1 && selectedSites[0] == site;

        Debug.Log($"[VerifyPichacao] Comparação: (Count==1 && Selected[0]==SiteEsperado) => {isCorrectSiteSelected}");
        return isCorrectSiteSelected;
    }

    private bool VerifyRansomware(string wallet, string hash)
    {
        Debug.Log($"[VerifyRansomware] Entrou na função. Wallet esperada: {wallet}, Hash esperada: {hash}");
        Debug.Log($"[VerifyRansomware] Wallet selecionada: {_walletSelect}, Hash selecionada: {_hashSelect}");

        bool isCorrect = _walletSelect == wallet && _hashSelect == hash;

        Debug.Log($"[VerifyRansomware] Comparação: (SelectedName==Esperado && SelectedWallet==Esperada) => {isCorrect}");
        return isCorrect;
    }

    private bool VerifyDataleak(string client, string date)
    {
        bool isCorrect = _dataLeakDateSelect == date && _dataLeakClientSelect == client;

        Debug.Log($"[VerifyDataleak] Comparação: (_dataLeakDateSelect==Esperado && _dataLeakClientSelect==Esperada) => {isCorrect}");
        return isCorrect;
    }

    public void ResetNewTicketInfos()
    {
        _playbookSelect = "";
        _idSelect = "";
        _ipOSelect = "";
        _ipDSelect = "";
        _geolocationSelect = "";
        _deviceSelect = "";
        _originSelect = "";
        _dateSelect = "";

        _playbookDp.ClearOptions();
        _idDp.ClearOptions();
        _ipODp.ClearOptions();
        _ipDDp.ClearOptions();
        _geolocationDp.ClearOptions();
        _deviceType.ClearOptions();
        _originType.ClearOptions();
        _dateDp.ClearOptions();
        _criptoWallet.ClearOptions();
        _hashDp.ClearOptions();
        _clientDataleakDp.ClearOptions();
        _dateDataleakDp.ClearOptions();

        foreach (var toggle in _RisktoggleGroup.GetComponentsInChildren<Toggle>())
            toggle.isOn = false;

        foreach (Transform child in _SitetoggleGroup)
        {
            Toggle toggle = child.GetComponent<Toggle>();
            if (toggle != null)
                toggle.isOn = false;
        }
    }

    public List<Toggle> GetPichacaoSiteSelected()
    {
        List<Toggle> selected = new List<Toggle>();

        foreach (Transform child in _SitetoggleGroup)
        {
            Toggle toggle = child.GetComponent<Toggle>();
            if (toggle != null && toggle.isOn)
                selected.Add(toggle);
        }

        return selected;
    }
}

//Get at Unity discussion forum
public static class IListExtensions {
	/// <summary>
	/// Shuffles the element order of the specified list.
	/// </summary>
	public static void Shuffle<T>(this IList<T> ts) {
		var count = ts.Count;
		var last = count - 1;
		for (var i = 1; i < last; ++i) {
			var r = Random.Range(i, count);
			var tmp = ts[i];
			ts[i] = ts[r];
			ts[r] = tmp;
		}
	}
}
