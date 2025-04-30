using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoggerInstance : MonoBehaviour
{
    [SerializeField] private Button _loggerButton;
    [SerializeField] private TextMeshProUGUI _id;
    [SerializeField] private TextMeshProUGUI _date;
    [SerializeField] private TextMeshProUGUI _hour;
    private List<TicketLog> _logs;


    private void OnEnable()
    {
        _loggerButton.onClick.AddListener(ClickLogger);
    }


    private void OnDisable()
    {
        _loggerButton.onClick.RemoveListener(ClickLogger);
    }

    private void ClickLogger()
    {
        EventManager.OpenLog(_logs);
    }

    public void UpdateInfo(string id, string date, string hour,  List<TicketLog> logs)
    {
        _logs = logs;
        _id.text = id;
        _date.text = date;
        _hour.text = hour;
    }

}
