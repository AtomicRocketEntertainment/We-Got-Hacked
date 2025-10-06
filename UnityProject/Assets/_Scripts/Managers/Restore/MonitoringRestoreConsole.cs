using TMPro;
using UnityEngine;
using UnityEngine.UI;
using NaughtyAttributes;

public class MonitoringRestoreConsole : MonoBehaviour
{
    [SerializeField] private GameObject _holdingScreen;
    [SerializeField] private GameObject _infosScreen;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _timestampFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _originFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _destinyFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _protocolFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _archiveFields;
    [BoxGroup("Console Dependencies"), SerializeField] private TextMeshProUGUI[] _contentFields;

    [BoxGroup("Console Dependencies"), SerializeField] private Button[] _copyBtns;


    void OnEnable()
    {
        _holdingScreen.SetActive(true);
        _infosScreen.SetActive(false);
        EventManager.OnConsoleOpened += OpenConsole;
    }

    void OnDisable()
    {
        EventManager.OnConsoleOpened -= OpenConsole;
    }

    private void OpenConsole(SO_ConsoleInfos infos)
    {
        _holdingScreen.SetActive(false);
        _infosScreen.SetActive(true);
        for (int i = 0; i < infos.Size; i++)
        {
            int index = i;
            _timestampFields[i].SetText(infos.Timestamp[i]);
            _originFields[i].SetText(infos.Origin[i]);
            _destinyFields[i].SetText(infos.Destiny[i]);
            _protocolFields[i].SetText(infos.Protocol[i]);
            _archiveFields[i].SetText(infos.Archive[i]);
            _contentFields[i].SetText(infos.Content[i]);

            _copyBtns[index].onClick.AddListener(() => CopyClicked(infos.Content[index]));
        }
    }

    private void CopyClicked(string key)
    {
        //readed by binary 64
        EventManager.CopyConsole(key);
    }
}
