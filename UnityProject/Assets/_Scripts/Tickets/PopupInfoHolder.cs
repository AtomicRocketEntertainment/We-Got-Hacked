using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PopupInfoHolder : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _id;
    [SerializeField] private TextMeshProUGUI _ipO;
    [SerializeField] private TextMeshProUGUI _ipD;
    [SerializeField] private TextMeshProUGUI _timestamp;
    [SerializeField] private TextMeshProUGUI _location;
    [SerializeField] private Image _icon;
    [SerializeField] private Image _alertColor;
    [SerializeField] private RectTransform _locationPoint;

    private readonly Dictionary<string, Vector2> _locationDictionary = new()
    {
        { "America do Sul", new Vector2(-85f, -30f) },
        { "America do Norte", new Vector2(-160f, 55f) },
        { "Africa", new Vector2(0f, 0f) },
        { "Europa", new Vector2(25f, 60f) },
        { "Asia", new Vector2(105f, 60f) }
    };


    public void UpdateInfos(string id, string ipO, string ipD, string day, string hour, string location, Sprite icon, Color alertRisk)
    {
        _id.text = $"ID Alert:{id}";
        _ipO.text = ipO;
        _ipD.text = ipD;
        _timestamp.text = $"{day} - {hour}";
        _location.text = location;
        _icon.sprite = icon;
        _alertColor.color = alertRisk;
        
        string continent = location.Split('-')[0].Trim();

        if (_locationDictionary.TryGetValue(continent, out Vector2 position))
            _locationPoint.anchoredPosition = position;
    }
}
