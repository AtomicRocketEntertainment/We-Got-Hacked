using UnityEngine;
using UnityEngine.UI;

public class MonitoringHandler : MonoBehaviour
{
    [SerializeField] private Button _btn;
    [SerializeField] private SO_ConsoleInfos _infos;

    void OnEnable()
    {
        _btn.onClick.AddListener(OnMonitoringClicked);
    }

    void OnDisable()
    {
        _btn.onClick.RemoveListener(OnMonitoringClicked);
    }

    private void OnMonitoringClicked()
    {
        EventManager.OpenRestoreConsole(_infos);
    }
}
