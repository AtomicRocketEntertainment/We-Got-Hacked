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

    [BoxGroup("Ransomware Infos"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _ransomwareName;
    [BoxGroup("Ransomware Infos"), ShowIf(nameof(NewTicketEditorChecker))][SerializeField] private TMP_Dropdown _criptoWallet;

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

    private readonly int _maxObjectiveWithoutScale = 6;
    private readonly float _scaleGapForObjective = -50f;

    private string _playbookSelect = "";
    private string _idSelect = "";
    private string _ipOSelect = "";
    private string _ipDSelect = "";
    private string _geolocationSelect = "";
    private string _deviceSelect = "";
    private string _originSelect = "";
    private string _dateSelect = "";
    private string _ransomwareSelect = "";
    private string _walletSelect = "";
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
        _ransomwareName?.onValueChanged.AddListener(UpdateRansomwareSelected);
        _criptoWallet?.onValueChanged.AddListener(UpdateWalletSelected);

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
        _ransomwareName?.onValueChanged.RemoveListener(UpdateRansomwareSelected);
        _criptoWallet?.onValueChanged.RemoveListener(UpdateWalletSelected);
    }

    private void UpdatePlaybookSelected(int value)
    {
        _playbookSelect = _playbookDp.options[value].text;

        foreach (GameObject screen in _playbookScreens)
            screen.SetActive(false);

        if (value >= 0 && value <= _playbookScreens.Count)
            _playbookScreens[value].SetActive(true);
    }
    private void UpdateIdSelected(int value) { _idSelect = _idDp.options[value].text; }
    private void UpdateIpOSelected(int value) { _ipOSelect = _ipODp.options[value].text; }
    private void UpdateIpDSelected(int value) { _ipDSelect = _ipDDp.options[value].text; }
    private void UpdateLocationSelected(int value) { _geolocationSelect = _geolocationDp.options[value].text; }
    private void UpdateDeviceSelected(int value) { _deviceSelect = _deviceType.options[value].text; }
    private void UpdateOriginSelected(int value) { _originSelect = _originType.options[value].text; }
    private void UpdateDateSelected(int value) { _dateSelect = _dateDp.options[value].text; }
    private void UpdateRansomwareSelected(int value) { _ransomwareSelect = _ransomwareName.options[value].text; }
    private void UpdateWalletSelected(int value) { _walletSelect = _criptoWallet.options[value].text; }



    public void UpdateInfos(ScreenType typeScreen, SO_TicketList ticketList, Ticket currentTicket, SoftwareState softwareState)
    {
        switch (typeScreen)
        {
            case ScreenType.NewTicket:
                UpdateNewTicket(ticketList, softwareState);
                break;
            case ScreenType.CurrentTicket:
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
        _ransomwareName.ClearOptions();
        _criptoWallet.ClearOptions();

        List<string> playBookOptions = new List<string>
        {
            _playbookSelect,
            nameof(PlaybookType.Pichacao),
            nameof(PlaybookType.Phishing),
            nameof(PlaybookType.Ransomware),
            nameof(PlaybookType.VazamentoDeDados)
        };

        if (softwareState != SoftwareState.FullAccess) return;

        List<string> idOptions = new List<string> { _idSelect };
        List<string> ipOOptions = new List<string> { _ipOSelect };
        List<string> ipDOptions = new List<string> { _ipDSelect };
        List<string> geolocationOptions = new List<string> { _geolocationSelect };
        List<string> deviceOptions = new List<string> { _deviceSelect };
        List<string> originOptions = new List<string> { _originSelect };
        List<string> dateOptions = new List<string> { _dateSelect };
        List<string> ransomwareOptions = new List<string> { _ransomwareSelect };
        List<string> criptWalletOptions = new List<string> { _walletSelect };


        foreach (SO_Ticket ticket in ticketList.Tickets)
        {
            idOptions.Add(ticket.ID);
            ipOOptions.Add(ticket.IPOrigem);
            ipDOptions.Add(ticket.IPDestiny);

            if (!geolocationOptions.Contains(ticket.Location))
                geolocationOptions.Add(ticket.Location);

            if (!deviceOptions.Contains(ticket.DeviceAttacked.ToString()))
                deviceOptions.Add(ticket.DeviceAttacked.ToString());


            if (!originOptions.Contains(ticket.Origin.ToString()))
                originOptions.Add(ticket.Origin.ToString());

            if (!ransomwareOptions.Contains(ticket.RansomwareName))
                ransomwareOptions.Add(ticket.RansomwareName);

            if (!criptWalletOptions.Contains(ticket.CriptoWallet))
                criptWalletOptions.Add(ticket.CriptoWallet);

            dateOptions.Add($"{ticket.DateDay} - {ticket.DateHour}");
        }

        IListExtensions.Shuffle(idOptions);
        IListExtensions.Shuffle(ipOOptions);
        IListExtensions.Shuffle(ipDOptions);
        IListExtensions.Shuffle(geolocationOptions);
        IListExtensions.Shuffle(deviceOptions);
        IListExtensions.Shuffle(originOptions);
        IListExtensions.Shuffle(dateOptions);

        _playbookDp.AddOptions(playBookOptions);
        _idDp.AddOptions(idOptions);
        _ipODp.AddOptions(ipOOptions);
        _ipDDp.AddOptions(ipDOptions);
        _geolocationDp.AddOptions(geolocationOptions);
        _deviceType.AddOptions(deviceOptions);
        _originType.AddOptions(originOptions);
        _dateDp.AddOptions(dateOptions);
        _ransomwareName.AddOptions(ransomwareOptions);
        _criptoWallet.AddOptions(criptWalletOptions);
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

        ScaleObjectiveScreen(completedObjectives);
    }

    private void ScaleObjectiveScreen(int objectiveQuantity)
    {
        int extraObjective = objectiveQuantity - _maxObjectiveWithoutScale;
        if (extraObjective <= 0) return;

        _mainCurrentObjectiveScreen.TryGetComponent(out RectTransform rect);
        float newBottom = rect.offsetMin.y - (extraObjective * _scaleGapForObjective);
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
        if (ticket.Playbook.ToString() != _playbookSelect) return false;

        bool isCommonInfoCorrect = VerifyCommonInfos(ticket);
        bool isPlaybookCorrect = VerifyPlaybookInfos((PlaybookType)_playbookDp.value - 1 , ticket);//first selected is the index 0

        return isCommonInfoCorrect && isPlaybookCorrect;
    }

    private bool CheckSelectedInfos()
    {
        if (_playbookDp.value == 0)  return false;

        bool isPlaybookInfoSelected = PlaybookInfosAreSelected((PlaybookType)_playbookDp.value - 1); //first selected is the index 0

        return isPlaybookInfoSelected &&
            _idSelect != "" &&
            _ipOSelect != "" &&
            _ipDSelect != "" &&
            _geolocationSelect != "" &&
            _deviceSelect != "" &&
            _originSelect != "" &&
            _dateSelect != "";
    }

    private bool PlaybookInfosAreSelected(PlaybookType playbook)
    {
        bool areSelected = false;

        switch (playbook)
        {
            case PlaybookType.Pichacao:
                areSelected = GetPichacaoSiteSelected().Count > 0;
                break;
            case PlaybookType.Ransomware:
                areSelected = _ransomwareSelect != "" && _walletSelect != "";
                break;
        }

        return areSelected;
    }


    private bool VerifyPlaybookInfos(PlaybookType playbook, Ticket ticket)
    {
        bool isCorrect = false;

        switch (playbook)
        {
            case PlaybookType.Pichacao:
                isCorrect = VerifyPichacao(ticket.Site);
                break;
            case PlaybookType.Ransomware:
                isCorrect = VerifyRansomware(ticket.RansomwareName, ticket.CriptoWallet);
                break;
        }

        return isCorrect;
    }

    private bool VerifyCommonInfos(Ticket ticket)
    {
        string selectedId = _idDp.options[_idDp.value].text;
        string selectedIpD = _ipDDp.options[_ipDDp.value].text;
        string selectedIpO = _ipODp.options[_ipODp.value].text;
        string selectedLocation = _geolocationDp.options[_geolocationDp.value].text;
        string selectedDevice = _deviceType.options[_deviceType.value].text;
        string selectedOrigin = _originType.options[_originType.value].text;
        string selectedDate = _dateDp.options[_dateDp.value].text;
        int selectedRisk = 0;

        Toggle selectedRiskToggle = _RisktoggleGroup.ActiveToggles().FirstOrDefault();

        if (selectedRiskToggle != null)
        {
            selectedRiskToggle.TryGetComponent(out ImRiskHolder riskLevel);
            selectedRisk = riskLevel.RiskLevel;
        }

        return
        selectedId == ticket.ID &&
        selectedIpO == ticket.IPOrigem &&
        selectedIpD == ticket.IPDestiny &&
        selectedLocation == ticket.Location &&
        selectedDevice == ticket.DeviceAttacked.ToString() &&
        selectedOrigin == ticket.Origin.ToString() &&
        selectedDate == $"{ticket.DateDay} - {ticket.DateHour}" &&
        selectedRisk == ticket.RiskLevel;
    }

    private bool VerifyPichacao(SiteType site)
    {
        List<SiteType> selectedSites = new List<SiteType>();

        foreach (var toggle in GetPichacaoSiteSelected())
            if (toggle.TryGetComponent(out ImSiteHolder siteHolder))
                selectedSites.Add(siteHolder.Site);

        bool isCorrectSiteSelected = selectedSites.Count == 1 && selectedSites[0] == site;

        return isCorrectSiteSelected;
    }

    private bool VerifyRansomware(string ransomware, string wallet)
    {
        string selectedName = _ransomwareName.options[_ransomwareName.value].text;
        string selectedWallet = _criptoWallet.options[_criptoWallet.value].text;


        bool isCorrect = selectedName == ransomware && selectedWallet == wallet;

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
        _ransomwareName.ClearOptions();
        _criptoWallet.ClearOptions();

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
