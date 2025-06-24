using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using System;
using Unity.VisualScripting;

public class RestoreManager : MonoBehaviour, INeedOpenCanvas
{
    [BoxGroup("Infos Dependencies")] [SerializeField] private SO_TicketList _ticketList;
    [BoxGroup("UI Dependencies - General")] [SerializeField] private Button _loggerBtn;
    [BoxGroup("UI Dependencies - General")] [SerializeField] private Button _backupBtn;
    [BoxGroup("UI Dependencies - General")] [SerializeField] private Button _controlBtn;
    [BoxGroup("UI Dependencies - CMD")] [SerializeField] private TextMeshProUGUI _cmdLoggerText;
    [BoxGroup("UI Dependencies - Control")] [SerializeField] private List<OnOffLoggerToggle> _controlToggles;
    [BoxGroup("UI Dependencies - Backup")] [SerializeField] private Button _confirmBackupBtn;
    [BoxGroup("UI Dependencies - Backup")] [SerializeField] private TextMeshProUGUI _confirmBackupWebsite;

    [BoxGroup("Screens")] [SerializeField] private GameObject _mainCanvas;
    [BoxGroup("Screens")] [SerializeField] private GameObject _loggerScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _backupScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _controlScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _cmdScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _confirmBackupScreen;

    [BoxGroup("Spawn Dependencies")] [SerializeField] private List<Transform> _loggersParents;
    [BoxGroup("Spawn Dependencies")] [SerializeField] private List<Transform> _backupsParents;
    [BoxGroup("Spawn Dependencies")] [SerializeField] private GameObject _loggerPrefab;
    [BoxGroup("Spawn Dependencies")] [SerializeField] private GameObject _backupPrefab;

    private RestoreState _currentState = RestoreState.None;
    private Dictionary<Button, GameObject> _restoreScreens;
    
    private const string WRONG_BACKUP = "Hm acho que devo analisar melhor qual backup restaurar.";
    private const string ITS_NOT_TIME_FOR_THIS = "Droga! Esse não era o momento de fazer isso.";
    
    void Awake()
    {
        _restoreScreens = new Dictionary<Button, GameObject> 
        {
            {_loggerBtn, _loggerScreen},
            {_backupBtn, _backupScreen},
            {_controlBtn , _controlScreen}
        };

        CreateLoggers();
        CreateBackups();
    }

    void OnEnable()
    {
        EventManager.OnSiteIsOff += ChangeState;
        EventManager.OnOpenLog += ShowCmd;
        EventManager.OnOpenBackup += ShowConfirmBackup;
        EventManager.OnEventEmailHandlerIsOpen += UpdateState;
        EventManager.OnGenericResponseIsMaded += UpdateState;
        EventManager.OnEmailIsAnswered += UpdateState;
        EventManager.OnEmailIsWriten += UpdateState;

        
        foreach (var key in _restoreScreens)
            key.Key.onClick.AddListener(() => OpenScreen(key.Key));
    }

    private void ChangeState(RestoreState state)
    {
        _currentState = state;

        if(_currentState != RestoreState.OnOff)
            UpdateTurnToggles(false);
    }

    private void OpenScreen(Button key)
    {
        foreach(var sceen in _restoreScreens)
            sceen.Value.SetActive(false);

        if(_restoreScreens.ContainsKey(key))
            _restoreScreens[key].SetActive(true);
    }

    void OnDisable()
    {
        EventManager.OnSiteIsOff -= ChangeState;
        EventManager.OnOpenLog -= ShowCmd;
        EventManager.OnOpenBackup -= ShowConfirmBackup;
        EventManager.OnEventEmailHandlerIsOpen -= UpdateState;
        EventManager.OnGenericResponseIsMaded -= UpdateState;
        EventManager.OnEmailIsAnswered -= UpdateState;
        EventManager.OnEmailIsWriten -= UpdateState;

        _confirmBackupBtn.onClick.RemoveAllListeners();
        
        foreach(var key in _restoreScreens)
            key.Key.onClick.RemoveAllListeners();
    }

    private void ShowConfirmBackup(SiteBackup backup)
    {
        _confirmBackupBtn.onClick.RemoveAllListeners();
        _confirmBackupBtn.onClick.AddListener(() => ConfirmBackupClicked(backup));
        _confirmBackupScreen.SetActive(true);

        _confirmBackupWebsite.text = $"Site: {backup.Website}";
    }

    private void ConfirmBackupClicked(SiteBackup backup)
    {
        if(backup.IsCorrect && _currentState == RestoreState.Backuper)
        {
            EventManager.CorrectChoice();
            EventManager.SpawnEmail(EmailType.LORE);
        }
        else
        {
            EventManager.WrongChoice();

            if(_currentState == RestoreState.Backuper)
                EventManager.MakePlayerThink(WRONG_BACKUP);
            else
                EventManager.MakePlayerThink(ITS_NOT_TIME_FOR_THIS);
        }

        _confirmBackupScreen.SetActive(false);
    }
    
    private void ShowCmd(List<TicketLog> logs)
    {
        _cmdScreen.SetActive(true);       
        _cmdLoggerText.text = "";

        foreach(TicketLog log in logs)
        {
            _cmdLoggerText.text += $"{log.Log}\n\n";
            
            if(log.IsCorrect && _currentState == RestoreState.Logger)
                EventManager.OpenGenericResponse();
        }
    }

    private void CreateLoggers()
    {
        foreach(Transform loggerParent in _loggersParents)
        {
            foreach(SO_Ticket ticket in _ticketList.Tickets)
            {
                GameObject instanceLogger = Instantiate(_loggerPrefab, Vector3.zero, Quaternion.identity);
                instanceLogger.transform.SetParent(loggerParent);
                instanceLogger.name = ticket.ID;
                instanceLogger.transform.localScale = Vector3.one;
                instanceLogger.TryGetComponent(out LoggerInstance instance);
                instance.UpdateInfo(ticket.ID, ticket.DateDay, ticket.DateHour, ticket.Loggs);
            }
        }
    }

    private void CreateBackups()
    {
        foreach(Transform backupParent in _backupsParents)
        {
            backupParent.gameObject.TryGetComponent(out ImListBackupHandler backupList);
            for(int i = 0; i < backupList.Backups.Count; i++)
            {
                GameObject backupInstance = Instantiate(_backupPrefab, Vector3.zero, Quaternion.identity);
                backupInstance.transform.SetParent(backupParent);
                backupInstance.name = "Backup Button " + i;
                backupInstance.transform.localScale = Vector3.one;
                backupInstance.TryGetComponent(out ImBackupHandler instance);
                instance.UpdateInfo(backupList.Backups[i]);
            }
        }
    }

    private void UpdateState(string emailIndex)
    {
        if(emailIndex == "Lore 11")
        {
            _currentState = RestoreState.OnOff;
            UpdateTurnToggles(true);
        }

        if (emailIndex == "Lore 15")
        {
            _currentState = RestoreState.OnOff;
            EventManager.TicketObjectiveCompleted();
            UpdateTurnToggles(true);
        }

        if (emailIndex == "Lore 13")
                _currentState = RestoreState.Backuper;

        if(emailIndex == "Response 1")
            _currentState = RestoreState.None;
    }

    private void UpdateTurnToggles(bool status)
    {
        foreach(OnOffLoggerToggle toggle in _controlToggles)
            toggle.ChangeInteractable(status);
    }

    public void OpenCanvas()
    {
        _mainCanvas.SetActive(true);
    }

    public void CloseCanvas()
    {
        _mainCanvas.SetActive(false);
    }

    public void CloseCMD()
    {
        _cmdScreen.SetActive(false);
        
        if(_currentState == RestoreState.Logger)
            EventManager.CloseResponseScreen();
    }
}

[System.Serializable]
public struct SiteBackup
{
    public bool IsCorrect;
    public string Website;
    public string Date;
    public string Hour;
}

public enum RestoreState
{
    None, OnOff, Logger, Backuper
}
