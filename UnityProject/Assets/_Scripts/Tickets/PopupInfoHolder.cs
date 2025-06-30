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
        { "Brasil", new Vector2(-85f, -30f) },
        { "Bolivia", new Vector2(-110f, -40f) },
        { "Estados Unidos", new Vector2(-150f, 40f) },
        { "Canada", new Vector2(-175f, 70f) },
        { "Itália", new Vector2(-2f, 40f) }
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
        
        string country = location.Split('-')[1].Trim();

        if (_locationDictionary.TryGetValue(country, out Vector2 position))
            _locationPoint.anchoredPosition = position;
    }
}
