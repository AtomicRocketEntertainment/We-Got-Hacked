using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ImBackupHandler : MonoBehaviour
{
    [SerializeField] private Button _backupButton;
    [SerializeField] private TextMeshProUGUI _date;
    [SerializeField] private TextMeshProUGUI _hour;
    private SiteBackup _backupInfo;

    void OnEnable()
    {
        _backupButton.onClick.AddListener(ClickBackup);
    }

    void OnDisable()
    {
        _backupButton.onClick.RemoveListener(ClickBackup);
    }

    public void UpdateInfo(SiteBackup backup)
    {
        _backupInfo = backup;
        _date.text = _backupInfo.Date;
        _hour.text = _backupInfo.Hour;
    }

    public void ClickBackup()
    {
        EventManager.OpenBackup(_backupInfo);
    }
}
