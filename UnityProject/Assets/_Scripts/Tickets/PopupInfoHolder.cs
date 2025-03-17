using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfoHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _id;
    [SerializeField] private TextMeshProUGUI _ip;
    [SerializeField] private TextMeshProUGUI _timestamp;
    [SerializeField] private TextMeshProUGUI _location;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _alertColor;

    public void UpdateInfos(string id, string ip, string timestamp, string location, Sprite icon, Color alertRisk)
    {
        _id.text = $"ID Alert:{id}";
        _ip.text = ip;
        _timestamp.text = timestamp;
        _location.text = location;
        _icon.sprite = icon;
        _alertColor.color = alertRisk;
    }
}
