using System.Collections.Generic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

public class TicketScreen : MonoBehaviour, IScreenInfoUpdater
{
    [SerializeField] private ScreenType _screenType;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _playbookDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _idDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _ipDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _geolocationDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _typeDp;
    [BoxGroup("New Ticket Components"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private TMP_Dropdown _dateDp;

    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _blockedCanvas;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _pichacaoScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _phishingScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _ransowareScreen;
    [BoxGroup("Playbook's Screens"), ShowIf(nameof(NewTicketEditorChecker))] [SerializeField] private GameObject _dataLeakScreen;

    private List<GameObject> _playbookScreens => new List<GameObject> 
    {
        _pichacaoScreen,
        _phishingScreen,
        _ransowareScreen,
        _dataLeakScreen
    };

    private string _playbookSelect = "";
    private string _idSelect = "";
    private string _ipSelect = "";
    private string _geolocationSelect = "";
    private string _typeSelect = "";
    private string _dateSelect = "";

    public ScreenType CurrentType => _screenType;

    void OnEnable()
    {
        _playbookDp?.onValueChanged.AddListener(UpdatePlaybookSelected);
        _idDp?.onValueChanged.AddListener(UpdateIdSelected);
        _ipDp?.onValueChanged.AddListener(UpdateIpSelected);
        _geolocationDp?.onValueChanged.AddListener(UpdateLocationSelected);
        _typeDp?.onValueChanged.AddListener(UpdateDeviceSelected);
        _dateDp?.onValueChanged.AddListener(UpdateDateSelected);
    }

    void OnDisable()
    {
        _playbookDp?.onValueChanged.RemoveListener(UpdatePlaybookSelected);
        _idDp?.onValueChanged.RemoveListener(UpdateIdSelected);
        _ipDp?.onValueChanged.RemoveListener(UpdateIpSelected);
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
    private void UpdateIpSelected(int value) { _ipSelect = _ipDp.options[value].text; }
    private void UpdateLocationSelected(int value) { _geolocationSelect = _geolocationDp.options[value].text; }
    private void UpdateDeviceSelected(int value) { _typeSelect = _typeDp.options[value].text; }
    private void UpdateDateSelected(int value) { _dateSelect = _dateDp.options[value].text; }


    public void UpdateInfos(ScreenType typeScreen, SiemManager siem)
    {
        switch(typeScreen)
        {
            case ScreenType.NewTicket: UpdateNewTicket(siem);
            break;
            case ScreenType.CurrentTicket: UpdateCurrentTicket();
            break;
            case ScreenType.TicketDone: UpdateDoneTicket();
            break;
            case ScreenType.Playbook: UpdatePlaybook();
            break;
        }
    }

    private void UpdateNewTicket(SiemManager siem)
    {
        _playbookDp.ClearOptions();
        _idDp.ClearOptions();
        _ipDp.ClearOptions();
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
        
        List<string> idOptions = new List<string> { _idSelect };
        List<string> ipOptions = new List<string> { _ipSelect };
        List<string> geolocationOptions = new List<string> { _geolocationSelect };
        List<string> typeOptions = new List<string> { _typeSelect };
        List<string> dateOptions = new List<string> { _dateSelect };

        foreach(Ticket ticket in siem.ActiveTickets)
        {
            idOptions.Add(ticket.ID);
            ipOptions.Add(ticket.IP);
            geolocationOptions.Add(ticket.Location);
            typeOptions.Add(ticket.Dispositive.Type.ToString());
            dateOptions.Add(ticket.Date);
        }

        _playbookDp.AddOptions(playBookOptions);
        _idDp.AddOptions(idOptions);
        _ipDp.AddOptions(ipOptions);
        _geolocationDp.AddOptions(geolocationOptions);
        _typeDp.AddOptions(typeOptions);
        _dateDp.AddOptions(dateOptions);
    }

    private void UpdateCurrentTicket()
    {

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


}
