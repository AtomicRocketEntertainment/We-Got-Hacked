using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;
using System.Collections.Generic;
using TMPro;
using System;

public class RestoreManager : MonoBehaviour
{
    [BoxGroup("Infos Dependencies")] [SerializeField] private SO_TicketList _ticketList;
    [BoxGroup("UI Dependencies - General")] [SerializeField] private Button _loggerBtn;
    [BoxGroup("UI Dependencies - General")] [SerializeField] private Button _backupBtn;
    [BoxGroup("UI Dependencies - General")] [SerializeField] private Button _controlBtn;
    [BoxGroup("UI Dependencies - CMD")] [SerializeField] private TextMeshProUGUI _cmdLoggerText;
    [BoxGroup("UI Dependencies - Backup")] [SerializeField] private Button _confirmBackupBtn;
    [BoxGroup("UI Dependencies - Backup")] [SerializeField] private TextMeshProUGUI _confirmBackupWebsite;

    [BoxGroup("Screens")] [SerializeField] private GameObject _loggerScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _backupScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _controlScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _cmdScreen;
    [BoxGroup("Screens")] [SerializeField] private GameObject _confirmBackupScreen;

    [BoxGroup("Spawn Dependencies")] [SerializeField] private List<Transform> _loggersParents;
    [BoxGroup("Spawn Dependencies")] [SerializeField] private List<Transform> _backupsParents;
    [BoxGroup("Spawn Dependencies")] [SerializeField] private GameObject _loggerPrefab;
    [BoxGroup("Spawn Dependencies")] [SerializeField] private GameObject _backupPrefab;

    private Dictionary<Button, GameObject> _restoreScreens;
    
    private const string WRONG_BACKUP = "Hm acho que devo analisar melhor qual backup restaurar.";
    
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
        EventManager.OnOpenLog += ShowCmd;
        EventManager.OnOpenBackup += ShowConfirmBackup;
        
        foreach(var key in _restoreScreens)
            key.Key.onClick.AddListener(() => OpenScreen(key.Key));
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
        EventManager.OnOpenLog -= ShowCmd;
        EventManager.OnOpenBackup += ShowConfirmBackup;
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
        if(backup.IsCorrect)
            EventManager.CorrectChoice();
        else
        {
            EventManager.WrongChoice();
            EventManager.MakePlayerThink(WRONG_BACKUP);
        }

        _confirmBackupScreen.SetActive(false);
    }
    
    private void ShowCmd(List<string> logs)
    {
        _cmdScreen.SetActive(true);
        
        /*LeanTween.cancel(_cmdScreen);
        LeanTween.scale(_cmdScreen, new Vector3(1.02f, 1.02f, 1.02f), .1f).setEase(LeanTweenType.easeOutQuad).setOnComplete(() => { 
            LeanTween.scale(_cmdScreen, Vector3.one, .1f).setEase(LeanTweenType.easeOutQuad);
        });*/
       
        _cmdLoggerText.text = "";

        foreach(string log in logs)
            _cmdLoggerText.text += $"{log}\n\n";
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


}

[System.Serializable]
public struct SiteBackup
{
    public bool IsCorrect;
    public string Website;
    public string Date;
    public string Hour;
}
