using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class StockInformationHolder : MonoBehaviour
{
    [SerializeField] private Image _sprite;
    [SerializeField] private TextMeshProUGUI _header;
    [SerializeField] private TextMeshProUGUI _content;

    public void UpdateNewsInfo(Sprite sprite, string title, string description)
    {
        _sprite.sprite = sprite;
        _header.text = title;
        _content.text = description;
    }
}
