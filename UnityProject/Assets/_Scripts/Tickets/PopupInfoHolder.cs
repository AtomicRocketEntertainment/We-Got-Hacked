using System.Collections.Generic;
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
    [SerializeField] private RectTransform _locationPoint;

    private Dictionary<string, Vector2> _locationDictionary = new Dictionary<string, Vector2>();
    private bool _oneTimeUpdate = false;

    void PrepareDictionary()
    {
        _oneTimeUpdate = true;
        if(!_locationDictionary.ContainsKey("America do Sul")) _locationDictionary.Add("America do Sul", new Vector2(-85f, -30f));
        if(!_locationDictionary.ContainsKey("America do Norte")) _locationDictionary.Add("America do Norte", new Vector2(-160f, 55f));
        if(!_locationDictionary.ContainsKey("Africa")) _locationDictionary.Add("Africa", new Vector2(0f, 0f));
        if(!_locationDictionary.ContainsKey("Europa")) _locationDictionary.Add("Europa", new Vector2(25f, 60f));
        if(!_locationDictionary.ContainsKey("Asia")) _locationDictionary.Add("Asia", new Vector2(105f, 60f));
    }

    public void UpdateInfos(string id, string ip, string timestamp, string location, Sprite icon, Color alertRisk)
    {
        if(!_oneTimeUpdate) PrepareDictionary();

        _id.text = $"ID Alert:{id}";
        _ip.text = ip;
        _timestamp.text = timestamp;
        _location.text = location;
        _icon.sprite = icon;
        _alertColor.color = alertRisk;

        string[] parts = location.Split('-');
        string continent = parts[0].Trim();

        if(_locationDictionary.ContainsKey(continent))
            _locationPoint.anchoredPosition = _locationDictionary[continent];
    }
}
